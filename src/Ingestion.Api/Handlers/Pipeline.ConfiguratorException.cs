namespace Ingestion.Api.Handlers
{
    public class PipelineConfiguratorException : Exception
    {
        public PipelineConfiguratorException() { }
        public PipelineConfiguratorException(string message) : base(message) { }
        public PipelineConfiguratorException(string message, Exception innerException) : base(message, innerException) { }
    }
}

