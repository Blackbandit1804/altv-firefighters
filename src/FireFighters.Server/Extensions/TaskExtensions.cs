using System;
using System.Threading.Tasks;

namespace FireFighters.Server.Extensions
{
    public static class TaskExtensions
    {
        public static void PerformAsyncTaskWithoutAwait(this Task task, Action<Task> exceptionHandler = null)
        {
            exceptionHandler ??= t => throw new Exception("Exception occured in async task", t.Exception);

            var dummy = task.ContinueWith(exceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
        }

        public static void BlockExecutionUntilTaskFinished(this Task task)
        {
            task.GetAwaiter().GetResult();
        }
    }
}
