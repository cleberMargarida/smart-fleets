using Ingestion.Api.Handlers.Abstractions;

namespace Ingestion.Api.Handlers
{
    public class PipelineConfigurator
    {
        private readonly HashSet<Type> _pipeline = new ();

        public PipelineConfigurator Add<T>() where T : IHandler
        {
            if (!_pipeline.Add(typeof(T)))
            {
                throw new PipelineConfiguratorException($"Duplicated handler {typeof(T)}.");
            }

            return this;
        }

        public static explicit operator HashSet<Type>(PipelineConfigurator pipelineConfigurator)
        {
            return pipelineConfigurator._pipeline;
        }
    }
}

