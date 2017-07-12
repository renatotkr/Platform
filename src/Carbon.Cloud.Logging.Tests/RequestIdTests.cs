using System;
using Carbon.Data.Sequences;

using Xunit;

namespace Carbon.Cloud.Logging
{
    public class RequestIdTests
    {
        [Fact]
        public void Serialize1()
        {
            var date = new DateTime(2015, 01, 01);

            var a = RequestId.Create(1, date, 3);

            var data = a.Serialize();

            Assert.Equal(16, data.Length);

            var b = Uid.Deserialize(data);

            Assert.Equal(4588768ul, a.Upper);

            Assert.Equal(date, RequestId.GetTimestamp(a));
            Assert.Equal(3, RequestId.GetSequenceNumber(a));
        }

        /*
        [Fact]
        public void Base62Encode()
        {
            for (var i = 0ul; i < 1_000_000; i++)
            {
                Assert.Equal(i, Base62.Decode(Base62.Encode(i)));
            }
        }
        */
        

        [Fact]
        public void Limits()
        {
            var maxId       = 2199023255552; // 2 ^ 41
            var maxSequence = (2199023255552 * 2) - 1; // 2 ^ 42

            var maxDate = new DateTime(1970 + 478, 01, 01, 18, 17, 37, 999, DateTimeKind.Utc); // 478 years

            var id = RequestId.Create(maxId, maxDate, maxSequence);
            
            Assert.Equal(4653133208748031999ul, id.Lower);

            Assert.Equal("aZl8N0ymIO65xJoLx2jXc3", id.ToString());

            Assert.Equal(maxDate,       RequestId.GetTimestamp(id));
            Assert.Equal(maxSequence,   RequestId.GetSequenceNumber(id));
        }

        [Fact]
        public void SequenceNumber()
        {
            for (var i = 0; i < 10; i++)
            {
                var id = RequestId.Create(0, new DateTime(2000, 01, 01), i);

                Assert.Equal(i, RequestId.GetSequenceNumber(id));
            }
        }


        [Fact]
        public void Uid2()
        {
            var date1 = new DateTime(1970, 01, 01, 00, 00, 00, DateTimeKind.Utc);
            var date2 = new DateTime(2000, 01, 01, 03, 17, 37, 593, DateTimeKind.Utc);
            var date3 = new DateTime(2000, 01, 01, 03, 17, 37, 594, DateTimeKind.Utc);
            var date4 = new DateTime(2099, 01, 01, 03, 17, 37, 595, DateTimeKind.Utc);
            var date5 = new DateTime(1970 + 478, 01, 01, 18, 17, 37, 999, DateTimeKind.Utc); // 478 years

            var id1 = RequestId.Create(1, date1, 1);
            var id2 = RequestId.Create(1, date1, 2);
            var id3 = RequestId.Create(1, date2, 2);
            var id4 = RequestId.Create(1, date3, 2);
            var id5 = RequestId.Create(1, date4, 2);
            var id6 = RequestId.Create(2, date4, 2);
            var id7 = RequestId.Create((long)Math.Pow(2, 41), date5, 2);
            var id8 = RequestId.Create((long)Math.Pow(2, 41), date5, (long)Math.Pow(2, 42) - 2);

            Assert.Equal("0000000hB8400000000001", id1.ToString());
            Assert.Equal("0000000hB8400000000002", id2.ToString());
            Assert.Equal("0000000iHxx5xBctuwgWJQ", id3.ToString());
            Assert.Equal("0000000iHxx5xBdIVbtTZU", id4.ToString());
            Assert.Equal("0000000mliX5xBeYlQGRfY", id5.ToString());
            Assert.Equal("0000000DWr15xBeYlQGRfY", id6.ToString());
            Assert.Equal("aZl8N0ymIO65xJnw6n6ZW2", id7.ToString());
            Assert.Equal("aZl8N0ymIO65xJoLx2jXc2", id8.ToString());

            /*
            // .PadLeft(24, '0')
            throw new Exception(string.Join(Environment.NewLine,
                new[] { id1, id2, id3, id4, id5, id6, id7, id8 }.Select(a => Base62.Encode(a))));
            */


            Assert.Equal(date4, RequestId.GetTimestamp(id5));
            Assert.Equal(date5, RequestId.GetTimestamp(id7));
            Assert.Equal(date5, RequestId.GetTimestamp(id7));


            Assert.Equal(1, RequestId.GetSequenceNumber(id1));
            Assert.Equal(2, RequestId.GetSequenceNumber(id2));
            Assert.Equal(2, RequestId.GetSequenceNumber(id3));

            Assert.Equal((long)Math.Pow(2, 41), RequestId.GetAccountId(id7));

            Assert.Equal(0,   RequestId.GetTimestamp(id1).Millisecond);
            Assert.Equal(593, RequestId.GetTimestamp(id3).Millisecond);
            Assert.Equal(594, RequestId.GetTimestamp(id4).Millisecond);
            Assert.Equal(595, RequestId.GetTimestamp(id5).Millisecond);
        }

        [Fact]
        public void Uid1()
        {
            var date = new DateTime(2000, 01, 01);

            var id = RequestId.Create(1, date, 1);

            Assert.Equal("0000000iHxu00000000001", id.ToString());

            var data = id.Serialize();
            var id2 = Uid.Deserialize(data);

            Assert.Equal(date, RequestId.GetTimestamp(id2));
            Assert.Equal(4457272ul, id2.Upper);

            Assert.Equal(1, RequestId.GetSequenceNumber(id2));

            var a = Uid.Parse(id.ToString());

            Assert.Equal("0000000iHxu00000000001", a.ToString());
        }
    
        [Fact]
        public void GuidToUidCast()
        {
            var date2 = new DateTime(2016, 12, 13);
            var uid = RequestId.Create(1, date2, 3);

            Assert.Equal("0000000jkc000000000003", RequestId.Create(1, date2, 3).ToString());

            var a = (Uid)new Guid(uid.Serialize());

            Assert.Equal("0000000jkc000000000003", a.ToString());
        }
    }
}