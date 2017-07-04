using System;
using Xunit;

namespace Carbon.Net.Tests
{
    public class ListenerTests
    {
        [Fact]
        public void Parse1()
        {
            Assert.Equal(5000,  Listener.Parse("https://*:5000").Port);
            Assert.Equal(5000,  Listener.Parse("http://*:5000").Port);
            Assert.Equal(433,   Listener.Parse("https://*").Port);
            Assert.Equal(80,    Listener.Parse("http://*").Port);
        }

        [Fact]
        public void A()
        {
            // var b64 = Convert.FromBase64String("7Npest0z1zW5QVFfNDBId4BW");

            // 18 bytes
            // throw new ArgumentException(b64.Length.ToString());
        }
    }
}