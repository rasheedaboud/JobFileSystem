using Fluxor;

namespace Client.Features.JobFiles
{
    public class Feature : Feature<JobFileState>
    {
        public override string GetName() => nameof(JobFileState);
        protected override JobFileState GetInitialState() => new();
    }
}
