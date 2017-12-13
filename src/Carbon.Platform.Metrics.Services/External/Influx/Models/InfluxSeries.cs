using System;

namespace Influx
{
    public class InfluxSeries
    {
        public InfluxSeries(string name, string[] columns, object[,] values)
        {
            Name    = name    ?? throw new ArgumentNullException(nameof(name));
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
            Values  = values  ?? throw new ArgumentNullException(nameof(values));
        }

        public string Name { get; }

        public string[] Columns { get; }

        public object[,] Values { get; }
    }
}

/*
{
  "name": "transfer",
  "columns": [
      "time",
      "sum"
  ],
  "values": [
      [
          "2017-12-12T22:01:00Z",
          169323
      ],
      [
          "2017-12-12T22:02:00Z",
          null
      ]
  ]
}
*/