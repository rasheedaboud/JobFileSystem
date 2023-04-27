using Ardalis.Result;
using Core.Entities;
using Core.Features.Estimates.Exceptions;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.LineItems;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record CreateLineItemCommand(string EstimateId,LineItemDto LineItem) : IRequest<LineItemDto>
    {

    }
    public class CreateLineItemHandler : IRequestHandler<CreateLineItemCommand, LineItemDto>
    {
        private readonly IRepository<Estimate> _context;
        public CreateLineItemHandler(IRepository<Estimate> context)
        {
            _context = context;
        }

        public async Task<LineItemDto> Handle(CreateLineItemCommand req, CancellationToken cancellationToken)
        {

            var entity = await _context.GetByIdAsync(req.EstimateId);

            if (entity is null) throw new EstimateNotFoundException();

            var (estimate, lineItem) = entity.AddLineItem(req.LineItem);

            await _context.UpdateAsync(estimate, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<LineItemDto>.Success(lineItem);
        }
    }
}
