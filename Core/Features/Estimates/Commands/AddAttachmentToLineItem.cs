using Ardalis.Result;
using AutoMapper;
using Core.Entities;
using JobFileSystem.Core.Features.Common.Exceptions;
using JobFileSystem.Shared.Interfaces;
using MediatR;

namespace Core.Features.Estimates.Commands
{
    public record AddAttachmentToLineItem(string estimateId,
                                          string lineItemId,
                                          string FileName = null,
                                          Stream File = null) : IRequest<bool>
    {

    }
    public class AddAttachmentToLineItemHandler : IRequestHandler<AddAttachmentToLineItem, bool>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Estimate> _estimateRepository;

        public AddAttachmentToLineItemHandler(IMapper mapper,
                                              IRepository<Estimate> estimateRepository)
        {
            _mapper = mapper;
            _estimateRepository = estimateRepository;
        }

        public async Task<bool> Handle(AddAttachmentToLineItem request, CancellationToken cancellationToken)
        {

            var estimate = await _estimateRepository.GetByIdAsync(request.estimateId, cancellationToken);

            if (estimate is null) throw new NotFoundException(nameof(estimate));

            var lineItem = estimate.LineItems.FirstOrDefault(x=>x.Id == request.lineItemId);

            if (lineItem is null) throw new NotFoundException(nameof(lineItem));

            if (!string.IsNullOrWhiteSpace(request.FileName) && request.File != null && request.File.Length > 0)
            {
                estimate.AddLineItemAttachment(request.lineItemId, request.FileName, request.File);

                await _estimateRepository.UpdateAsync(estimate, cancellationToken);
                await _estimateRepository.SaveChangesAsync(cancellationToken);
            }

            

            return Result<bool>.Success(true);
        }
    }
}
