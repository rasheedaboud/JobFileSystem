using Ardalis.Result;
using Core.Entities;
using Core.Features.MaterialTestReports.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.MaterialTestReports.Commands
{
    public record DeleteMaterialTestReportCommand(string Id) : IRequest<bool>
    {

    }
    public class DeleteJobFileHandler : IRequestHandler<DeleteMaterialTestReportCommand, bool>
    {
        private readonly IRepository<MaterialTestReport> _context;
        public DeleteJobFileHandler(IRepository<MaterialTestReport> context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteMaterialTestReportCommand req, CancellationToken cancellationToken)
        {

            var entity = await _context.GetByIdAsync(req.Id);

            if (entity is null)
            {
                throw new MaterialTestReportNotFoundException();
            }

            entity.IsDeleted = true;

            await _context.UpdateAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
