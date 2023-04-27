using Core.Features.Estimates;
using Core.Features.Estimates.Commands;
using Core.Features.Estimates.Queries;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.LineItems;
using Microsoft.AspNetCore.Mvc;
using NdtTracking.Wasm.Server.Controllers;

namespace JobFileSystem.Server.Controllers.Estimates
{
    public class EstimatesController : BaseApiController
    {
        private readonly EstimateValidator _validator;
        private readonly LineItemValidator _lineItemValidator;

        public EstimatesController(EstimateValidator validator,
                                   LineItemValidator lineItemValidator)
        {
            _validator = validator;
            _lineItemValidator = lineItemValidator;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate(EstimateDto Estimate)
        {
            var validationResult = await _validator.ValidateAsync(Estimate);

            return validationResult.IsValid ?
                   Ok() :
                   ReturnValidationErrors(validationResult);
        }

        [HttpPost("validatelineitem")]
        public async Task<IActionResult> ValidateLineItem(LineItemDto lineItem)
        {
            var validationResult = await _lineItemValidator.ValidateAsync(lineItem);

            return validationResult.IsValid ?
                   Ok() :
                   ReturnValidationErrors(validationResult);
        }

        private IActionResult ReturnValidationErrors(FluentValidation.Results.ValidationResult validationResult)
        {
            foreach (var validationError in validationResult.Errors)
            {
                ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(EstimateDto Estimate,
                                                CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new CreateEstimateCommand(Estimate), token));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(EstimateDto Estimate, string? purchaseOrderNumber, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new UpdateEstimateCommand(Estimate, purchaseOrderNumber), token));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new DeleteEstimateCommand(id), token));
        }
        [HttpPost("addlineitem")]
        public async Task<IActionResult> AddLineItem(LineItemDto dto, string estimateId, CancellationToken token = default)
        {
            var result = await Mediator.Send(new CreateLineItemCommand(estimateId, dto), token);
            return Ok(result);
        }
        [HttpPut("updatelineitem")]
        public async Task<IActionResult> UpdateLineItem(LineItemDto dto, string estimateId, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new UpdateLineItemCommand(estimateId, dto), token));
        }
        [HttpDelete("deletelineitem")]
        public async Task<IActionResult> DeleteLineItem(string lineItemId, string estimateId, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new DeleteLineItemCommand(estimateId, lineItemId), token));
        }

        [HttpPost("uploadFile")]
        public async Task<IActionResult> Upload(string estimateId, CancellationToken token = default)
        {
            if (Request.Form is null || !Request.Form.Files.Any()) return BadRequest();

            var file = Request.Form.Files[0];
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, token);

            return Ok(await Mediator.Send(new AddAttachmentToEstimate(estimateId, file.FileName, stream), token));

        }
        [HttpDelete("removeFile")]
        public async Task<IActionResult> Upload(string estimateId, string fileId, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(estimateId) || string.IsNullOrEmpty(fileId)) return BadRequest();

            return Ok(await Mediator.Send(new RemoveAttachmentFromEstimate(estimateId, fileId), token));

        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(string estimateId, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(estimateId)) return BadRequest();

            return Ok(await Mediator.Send(new GetEstimateByIdQuery(estimateId), token));

        }

        [HttpPost("uploadFileLineItem")]
        public async Task<IActionResult> UploadToLineItem(string estimateId, string lineItemId, CancellationToken token = default)
        {
            if (Request.Form is null || !Request.Form.Files.Any()) return BadRequest();
            if (string.IsNullOrEmpty(estimateId) ||
               string.IsNullOrEmpty(lineItemId)) return BadRequest();
            var file = Request.Form.Files[0];
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, token);

            return Ok(await Mediator.Send(new AddAttachmentToLineItem(estimateId, lineItemId, file.FileName, stream), token));

        }
        [HttpDelete("removeFileLineItem")]
        public async Task<IActionResult> RemoveFileFromLineItem(string estimateId, string lineItemId, string fileId, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(estimateId) ||
                string.IsNullOrEmpty(fileId) ||
                string.IsNullOrEmpty(lineItemId)) return BadRequest();

            return Ok(await Mediator.Send(new RemoveAttachmentFromLineItem(estimateId, lineItemId, fileId), token));

        }

        [HttpPost("print")]
        public async Task<IActionResult> Print(EstimateDto estimate, CancellationToken token = default)
        {

            return Ok(FSharp.HtmlView.printEstimate(new HttpClient(),estimate));

        }
    }
}
