using System;
using Carbon.Json;

namespace Influx
{
    public class InfluxQueryResponse
    {
        public InfluxQueryResponse(InfluxResult[] results)
        {
            Results = results ?? throw new ArgumentNullException(nameof(results));
        }

        public InfluxResult[] Results { get; }

        public static InfluxQueryResponse FromJson(JsonObject json)
        {
            InfluxResult[] results;

            if (json.TryGetValue("results", out var resultsNode)
                && resultsNode is JsonArray resultsArray)
            {
                results = new InfluxResult[resultsArray.Count];

                for (var resultIndex = 0; resultIndex < resultsArray.Count; resultIndex++)
                {
                    var resultObject = (JsonObject)resultsArray[resultIndex];

                    InfluxSeries[] seriesItems;

                    if (resultObject.TryGetValue("series", out var seriesNode)
                        && seriesNode is JsonArray seriesArray)
                    {
                        seriesItems = new InfluxSeries[seriesArray.Count];

                        JsonNode seriesNodeItem;

                        for (var seriesIndex = 0; seriesIndex < seriesArray.Count; seriesIndex++)
                        {
                            seriesNodeItem = seriesArray[seriesIndex];

                            var columns = seriesNodeItem["columns"].ToArrayOf<string>();
                            
                            seriesItems[seriesIndex] = new InfluxSeries(
                                name   : seriesNodeItem["name"],
                                columns: columns,
                                values : ConvertValues((JsonArray)seriesNodeItem["values"], columns.Length)
                            );
                        }
                    }
                    else
                    {
                        seriesItems = Array.Empty<InfluxSeries>();
                    }

                    results[resultIndex] = new InfluxResult(
                        statementId : (int)resultObject["statement_id"], 
                        seriesItems : seriesItems
                    );
                }
            }
            else
            {
                results = Array.Empty<InfluxResult>();
            }
            
            return new InfluxQueryResponse(results);
        }

        private static object[,] ConvertValues(JsonArray valueArray, int rank)
        {
            return ConvertValues2(valueArray, rank);  
        }

        private static object[,] ConvertValues2(JsonArray valueArray, int rank)
        {
            object[,] values = new object[valueArray.Count, rank];

            for (var rowIndex = 0; rowIndex < valueArray.Count; rowIndex++)
            {
                var rowArray = (JsonArray)valueArray[rowIndex];

                for (int columnIndex = 0; columnIndex < rank; columnIndex++)
                {
                    values[rowIndex, columnIndex] = ToValue(rowArray[columnIndex]);
                }
            }

            return values;
        }


        private static object[] ConvertValueList(JsonArray valueArray)
        {
            object[] values = new object[valueArray.Count];

            for (var i = 0; i < valueArray.Count; i++)
            {
                values[i] = ToValue(valueArray[i]);
            }

            return values;
        }

        private static object ToValue(JsonNode node)
        {
            switch (node.Type)
            {
                case JsonType.Boolean   : return (bool)node;
                case JsonType.Date      : return (DateTime)node;
                case JsonType.Null      : return null;
                case JsonType.Number    : return (double)node;
                case JsonType.String    : return (string)node;
            }

            throw new Exception("unsupported value type:" + node.Type);

        }
    }

    // transfer,accountId:10000,country:US,environmentId=145,type=egress


    // transfer,accountId=100

    // transfer,accountId:10000,country:US,environmentId=145,type=egress

    // http://ec2-34-201-167-255.compute-1.amazonaws.com:8086/query?pretty=true&db=borg&q=select%20sum(value)%20from%20transfer%20where%20accountId=%2710000%27%20group%20by%20time(1m)

    // transfer,accountId=100
}
