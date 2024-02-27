namespace IngestionAPI.EventHub.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExecutionOrderAttribute : Attribute
    {
        public int Order { get; set; }
    }
}
