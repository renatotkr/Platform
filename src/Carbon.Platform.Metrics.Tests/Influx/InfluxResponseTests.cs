using Carbon.Json;

using Xunit;

namespace Influx.Tests
{
    public class InfluxResponseTests
    {
        [Fact]
        public void A()
        {

            var text = @"{
    ""results"": [
        {
            ""statement_id"": 0,
            ""series"": [
                {
                    ""name"": ""transfer"",
                    ""columns"": [
                        ""time"",
                        ""sum""
                    ],
                    ""values"": [
                        [
                            ""2017-12-12T22:01:00Z"",
                            169323
                        ],
                        [
                            ""2017-12-12T22:02:00Z"",
                            null
                        ],
                        [
                            ""2017-12-12T22:03:00Z"",
                            null
                        ],
                        [
                            ""2017-12-12T22:04:00Z"",
                            null
                        ],
                        [
                            ""2017-12-12T22:05:00Z"",
                            null
                        ]
                    ]
                }
            ]
        }
    ]
}";


            var response = InfluxQueryResponse.FromJson(JsonObject.Parse(text));

            var series_0 = response.Results[0].Series[0];

            Assert.Equal("transfer", series_0.Name);

            Assert.Equal(new[] { "time", "sum" }, series_0.Columns);

            Assert.Equal("2017-12-12T22:01:00Z", series_0.Values[0, 0]);
            Assert.Equal(169323d,                series_0.Values[0, 1]);

            Assert.Equal("2017-12-12T22:02:00Z", series_0.Values[1, 0]);
            Assert.Null(series_0.Values[1, 1]);
        }

        [Fact]
        public void EmptyResultSet()
        {
            var text = @"{
    ""results"": [
        {
            ""statement_id"": 0
        }
    ]
}";
            
            var response = InfluxQueryResponse.FromJson(JsonObject.Parse(text));

            Assert.Equal(0, response.Results[0].StatementId);
            Assert.Empty(response.Results[0].Series);
        }
    }
}
