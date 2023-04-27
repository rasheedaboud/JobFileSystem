using Ardalis.Result;
using Core.Entities;
using Core.Features.Estimates.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record DeleteEstimateCommand(string Id) : IRequest<bool>
    {

    }
    public class DeleteJobFileHandler : IRequestHandler<DeleteEstimateCommand, bool>
    {
        private readonly IRepository<Estimate> _context;
        public DeleteJobFileHandler(IRepository<Estimate> context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteEstimateCommand req, CancellationToken cancellationToken)
        {

            var entity = await _context.GetByIdAsync(req.Id);

            if (entity is null)
            {
                throw new EstimateNotFoundException();
            }

            entity.IsDeleted = true;

            await _context.UpdateAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
