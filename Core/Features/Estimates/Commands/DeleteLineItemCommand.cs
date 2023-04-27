using Ardalis.Result;
using Core.Entities;
using Core.Features.Estimates.Exceptions;
using JobFileSystem.Shared.Interfaces;
using JobFileSystem.Shared.LineItems;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record DeleteLineItemCommand(string EstimateId,string LineItemId) : IRequest<bool>
    {

    }
    public class DeleteLineItemHandler : IRequestHandler<DeleteLineItemCommand, bool>
    {
        private readonly IRepository<Estimate> _context;
        public DeleteLineItemHandler(IRepository<Estimate> context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteLineItemCommand req, CancellationToken cancellationToken)
        {

            var entity = await _context.GetByIdAsync(req.EstimateId);

            if (entity is null) throw new EstimateNotFoundException();

            await _context.UpdateAsync(entity.RemoveLineItem(req.LineItemId), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
