using Ardalis.Result;
using Core.Entities;
using Core.Features.Contacts;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record CreateEstimateCommand(EstimateDto Estimate) : IRequest<EstimateDto>
    {

    }
    public class CreateJobFileHandler : IRequestHandler<CreateEstimateCommand, EstimateDto>
    {
        private readonly IRepository<Estimate> _context;
        public CreateJobFileHandler(IRepository<Estimate> context)
        {
            _context = context;
        }

        public async Task<EstimateDto> Handle(CreateEstimateCommand req, CancellationToken cancellationToken)
        {



            var entity = new Estimate(req.Estimate);

            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<EstimateDto>.Success(EstimateMapper.MapEstimateToDto(entity));
        }
    }
}
