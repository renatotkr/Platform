using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.CI
{
    [Dataset("Pipelines")]
    public class Pipeline
    {
        public Pipeline() { }

        public Pipeline(long id, string name, JsonObject properties)
        {
            Id = id;
            Name = name;
            Properties = properties;
        }

        [Member("id"), Key(sequenceName: "pipelineId")]
        public long Id { get; }

        [Member("name")]
        public string Name { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        // trigger
    }

    // download source
    // build 
    // test
    // deploy
    // ...

    [Dataset("PipelineStages")]
    public class PipelineStage
    {
        // pipelineId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        public string Name { get; set; }
    }


    // PipelineRun...
}
