using System;
using System.Runtime.Serialization;

namespace Influx
{
    public class InfluxResult
    {
        public InfluxResult(int statementId, InfluxSeries[] seriesItems)
        {
            StatementId = statementId;
            Series = seriesItems ?? throw new ArgumentNullException(nameof(seriesItems));
        }

        [DataMember(Name = "statement_id")]
        public int StatementId { get; }

        [DataMember(Name = "series")]
        public InfluxSeries[] Series { get; }
    }
}
