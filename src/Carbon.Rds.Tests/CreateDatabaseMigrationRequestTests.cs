using Carbon.Json;

using Xunit;

namespace Carbon.Rds.Services.Tests
{
    public class CreateDatabaseMigrationRequestTests
    {
        [Fact]
        public void EnsureSerializable()
        {
            var json = @"{ ""databaseId"": 1, ""schemaName"": ""Storage"", ""commands"": [ ""empty"" ], ""description"": ""description"" }";

            var a = JsonObject.Parse(json).As<CreateDatabaseMigrationRequest>();
            
            Assert.Equal(1, a.DatabaseId);
            Assert.Equal("Storage", a.SchemaName);
            Assert.Equal("empty", a.Commands[0]);
            Assert.Equal("description", a.Description);
        }
    }
}
