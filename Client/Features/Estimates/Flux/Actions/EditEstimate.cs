using JobFileSystem.Shared.Estimates;

namespace JobFileSystem.Client.Features.Estimates.Flux.Actions
{
    public record EditEstimate(EstimateDto dto,string purchaseOrderNumber)
    {
    }
}
