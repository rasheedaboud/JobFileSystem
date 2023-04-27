using Ardalis.Result;
using Core.Entities;
using Core.Features.Estimates.Exceptions;
using Core.Specifications.Estimates;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.LineItems;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record UpdateLineItemCommand(string EstimateId,LineItemDto LineItem) : IRequest<LineItemDto>
    {

    }
    public class UpdateLineItemHandler : IRequestHandler<UpdateLineItemCommand, LineItemDto>
    {
        private readonly IRepository<Estimate> _context;
        public UpdateLineItemHandler(IRepository<Estimate> context)
        {
            _context = context;
        }

        public async Task<LineItemDto> Handle(UpdateLineItemCommand req, CancellationToken cancellationToken)
        {

            var entity = await _context.GetBySpecAsync(new GetEstimateByIdWithLineItems(req.EstimateId));

            if (entity is null) throw new EstimateNotFoundException();

            var (estimate, lineItem) = entity.UpdateLineItem(req.LineItem);

            await _context.UpdateAsync(estimate, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<LineItemDto>.Success(lineItem);
        }
    }
}
