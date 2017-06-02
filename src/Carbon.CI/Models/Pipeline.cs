using Carbon.Data.Annotations;

namespace Carbon.CI
{
    [Dataset("Pipelines")]
    public class Pipeline
    {
        public Pipeline() { }

        public Pipeline(long id)
        {
            Id = id;
        }

        [Member("id"), Key(sequenceName: "pipelineId")]
        public long Id { get; }

        [Member("name")]
        public string Name { get; set; }

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
