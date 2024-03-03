using Orleans.Runtime;

namespace Orleans.TestingHost
{
    public static class RuntimeContext
    {
        public static void SetExecutionContext(IGrainContext context)
        {
            Runtime.RuntimeContext.SetExecutionContext(context);
        }
    }
}
