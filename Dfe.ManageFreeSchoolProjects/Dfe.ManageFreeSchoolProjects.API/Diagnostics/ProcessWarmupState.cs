namespace Dfe.ManageFreeSchoolProjects.API.Diagnostics
{
    public interface IProcessWarmupState
    {
        /// <summary>
        /// Records a business request. Returns true only for the first call in this process.
        /// </summary>
        bool MarkBusinessRequest();
    }

    public sealed class ProcessWarmupState : IProcessWarmupState
    {
        public const string HttpContextItemKey = "IsFirstBusinessRequestInProcess";

        private int _businessRequestCount;

        public bool MarkBusinessRequest() => Interlocked.Increment(ref _businessRequestCount) == 1;
    }
}
