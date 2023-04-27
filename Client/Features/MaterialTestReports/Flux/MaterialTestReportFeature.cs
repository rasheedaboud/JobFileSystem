using Fluxor;

namespace Client.Features.MaterialTestReports
{
    public class MaterialTestReportFeature : Feature<MaterialTestReportState>
    {
        public override string GetName() => nameof(MaterialTestReportState);
        protected override MaterialTestReportState GetInitialState() => new();
    }
}
