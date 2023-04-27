using Fluxor;

namespace Client.Features.Estimates
{
    public class LineItemFeature : Feature<LineItemState>
    {
        public override string GetName() => nameof(LineItemState);
        protected override LineItemState GetInitialState() => new();
    }
}
