using Fluxor;

namespace Client.Features.Estimates
{
    public class EstimateFeature : Feature<EstimateState>
    {
        public override string GetName() => nameof(EstimateState);
        protected override EstimateState GetInitialState() => new();
    }
}
