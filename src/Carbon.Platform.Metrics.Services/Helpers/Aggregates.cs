using System.Collections.Generic;
using System.Text;

namespace Carbon.Platform.Metrics
{
    public static class Aggregates
    {
        public static IEnumerable<string> GetSeriesPermutations(MetricData data, int skip = 0)
        {
            var sb = new StringBuilder();

            sb.Append(data.Name);

            if (skip == 0)
            {
                yield return sb.ToString();
            }

            for (int i = 0;  i < data.Dimensions.Length; i++)
            {
                if (i >= skip)
                {
                    sb.Append(',');

                    sb.Append(data.Dimensions[i].ToString());

                    yield return sb.ToString();
                }
            }

            if (skip != data.Dimensions.Length)
            {
                foreach (var permutation in GetSeriesPermutations(data, skip + 1))
                {
                    yield return permutation;
                }
            }
        }
    }
}

// Cube


//  The number of possible aggregations is determined by every possible combination of dimension granularities. 

// https://en.wikipedia.org/wiki/Aggregate_(data_warehouse)

// rollup

// get

// https://en.wikipedia.org/wiki/Aggregate_(data_warehouse)

// a, b, c
// a, b
// a
// a, c
