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

            Assert.Equal((ulong)1, a.Upper);

            // Assert.Equal(date, a.GetTimestamp());
            // Assert.Equal(3, a.GetSequenceNumber());
        }

        [Fact]
        public void Uid1()
        {
            var date = new DateTime(2000, 01, 01);

            var id = RequestId.Create(1, date, 1);

            Assert.Equal("0000000000000001371ab3eb00000001", id.ToString());

            var data = id.Serialize();
            var id2 = Uid.Deserialize(data);

            Assert.Equal(date,      id2.GetTimestamp());
            Assert.Equal((ulong)1,  id2.Upper);

            // Assert.Equal(1, id2.GetSequenceNumber());

            var a = Uid.Parse(id.ToString());

            Assert.Equal("0000000000000001371ab3eb00000001", a.ToString());
        }

        [Fact]
        public void ToStringTests3()
        {
            var date = new DateTime(2015, 01, 01);

            Assert.Equal("00002df3a845d7b452a8b2ac0000cc79", RequestId.Create(50524523452340, date, 52345).ToString());
            Assert.Equal("7fffffffffffffff52a8b2ac0000cc79", RequestId.Create(long.MaxValue, date, 52345).ToString());
            Assert.Equal("7ffffffffffffffe52a8b2ac0000cc79", RequestId.Create(long.MaxValue - 1, date, 52345).ToString());
            Assert.Equal("800000000000000052a8b2ac0000cc79", RequestId.Create(long.MinValue, date, 52345).ToString());
            Assert.Equal("800000000000000052a8b2ac0000cc79", Uid.Parse(RequestId.Create(long.MinValue, date, 52345).ToString()).ToString());
        }

        [Fact]
        public void ToStringTests()
        {
            var date = new DateTime(2015, 01, 01);

            var a = RequestId.Create(1, date, 3);
            var b = Uid.Parse(a.ToString());

            Assert.Equal("000000000000000152a8b2ac00000003", a.ToString());
            Assert.Equal("000000000000000152a8b2ac00000003", b.ToString());

            Assert.Equal((ulong)1, b.Upper);
            // Assert.Equal(2u, b.timestamp);
            // Assert.Equal(3, b.GetSequenceNumber());

            var date2 = new DateTime(2016, 12, 13);

            Assert.Equal("0000000000000001563d5ea400000003", RequestId.Create(1, date2, 3).ToString());
            Assert.Equal("0000000000000001563d5ea400000004", RequestId.Create(1, date2, 4).ToString());
            // Assert.Equal("0000000000000001563d5ea4003fffff", RequestId.Create(1, date2, ScopedId.MaxSequenceNumber).ToString());
        }

        [Fact]
        public void GuidToUidCast()
        {
            var date2 = new DateTime(2016, 12, 13);
            var uid = RequestId.Create(1, date2, 3);

            Assert.Equal("0000000000000001563d5ea400000003", RequestId.Create(1, date2, 3).ToString());

            var a = (Uid)new Guid(uid.Serialize());

            Assert.Equal("0000000000000001563d5ea400000003", a.ToString());
        }
    }
}