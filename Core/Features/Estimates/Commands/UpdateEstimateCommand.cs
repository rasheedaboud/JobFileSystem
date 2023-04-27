using Ardalis.Result;
using Core.Entities;
using Core.Features.Estimates.Exceptions;
using Core.Specifications.Estimates;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record UpdateEstimateCommand(EstimateDto Estimate,string purchaseOrderNumber=null) : IRequest<EstimateDto>
    {

    }
    public class UpdateJobFileHandler : IRequestHandler<UpdateEstimateCommand, EstimateDto>
    {
        private readonly IRepository<Estimate> _context;
        public UpdateJobFileHandler(IRepository<Estimate> context)
        {
            _context = context;
        }

        public async Task<EstimateDto> Handle(UpdateEstimateCommand req, CancellationToken cancellationToken)
        {

            var entity = await _context.GetBySpecAsync(new GetEstimateByIdWithLineItemsClientAndAttachments(req.Estimate.Id));

            if (entity is null)
                throw new EstimateNotFoundException();

            var result = entity.UpdateEstimate(req.Estimate.Id,
                                               req.Estimate.ShortDescription,
                                               req.Estimate.LongDescription,
                                               req.Estimate.Status,
                                               req.Estimate.Client.Id,
                                               req.Estimate.DeliveryDate,
                                               req.purchaseOrderNumber);

            await _context.UpdateAsync(result, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<EstimateDto>.Success(EstimateMapper.MapEstimateToDto(result));
        }
    }
}
