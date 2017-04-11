using Carbon.Json;

using Xunit;

namespace Bitbucket.Tests
{
    public class BitbucketTests
    {
        [Fact]
        public void Bitcommit()
        {
            var x = @"

{
  ""pagelen"": 30,
  ""values"":  [
     {
      ""hash"": ""1cb4e9b58be100c638fc6f3826d83720c55bf477"",
      ""links"":  {
        ""self"":  {
          ""href"": ""https://bitbucket.org/!api/2.0/repositories/carbonmade/mason/commit/1cb4e9b58be100c638fc6f3826d83720c55bf477""
        },
        ""comments"":  {
          ""href"": ""https://bitbucket.org/!api/2.0/repositories/carbonmade/mason/commit/1cb4e9b58be100c638fc6f3826d83720c55bf477/comments""
        },
        ""patch"":  {
          ""href"": ""https://bitbucket.org/!api/2.0/repositories/carbonmade/mason/patch/1cb4e9b58be100c638fc6f3826d83720c55bf477""
        },
        ""html"":  {
          ""href"": ""https://bitbucket.org/carbonmade/mason/commits/1cb4e9b58be100c638fc6f3826d83720c55bf477""
        },
        ""diff"":  {
          ""href"": ""https://bitbucket.org/!api/2.0/repositories/carbonmade/mason/diff/1cb4e9b58be100c638fc6f3826d83720c55bf477""
        },
        ""approve"":  {
          ""href"": ""https://bitbucket.org/!api/2.0/repositories/carbonmade/mason/commit/1cb4e9b58be100c638fc6f3826d83720c55bf477/approve""
        }
      },
      ""repository"":  {
        ""links"":  {
          ""self"":  {
            ""href"": ""https://bitbucket.org/!api/2.0/repositories/carbonmade/mason""
          },
          ""avatar"":  {
            ""href"": ""https://d3oaxc4q5k2d6q.cloudfront.net/m/069454acc352/img/language-avatars/default_16.png""
          }
        },
        ""full_name"": ""carbonmade/mason"",
        ""name"": ""mason""
      },
      ""author"":  {
        ""raw"": ""Computer <computer@gmail.com>"",
        ""user"":  {
          ""username"": ""Computer"",
          ""display_name"": ""Computer""
        }
      },
      ""parents"":  [],
      ""date"": ""2014-07-28T16:49:38+00:00"",
      ""message"": ""Intial commit\n""
    }
  ],
  ""page"": 1
}";

            var r = JsonObject.Parse(x)["values"][0].As<BitbucketCommit>();

            Assert.Equal("1cb4e9b58be100c638fc6f3826d83720c55bf477", r.Hash);
            Assert.Equal("Intial commit\n", r.Message);
        }

    }
}
