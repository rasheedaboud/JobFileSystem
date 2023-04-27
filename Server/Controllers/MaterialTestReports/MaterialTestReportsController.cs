using Core.Features.MaterialTestReports;
using Core.Features.MaterialTestReports.Commands;
using Core.Features.MaterialTestReports.Queries;
using JobFileSystem.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using NdtTracking.Wasm.Server.Controllers;

namespace JobFileSystem.Server.Controllers.MaterialTestReports
{
    public class MaterialTestReportsController : BaseApiController
    {
        private readonly MaterialTestReportValidator _validator;

        public MaterialTestReportsController(MaterialTestReportValidator validator)
        {
            _validator = validator;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate(MaterialTestReportDto MaterialTestReport)
        {
            var validationResult = await _validator.ValidateAsync(MaterialTestReport);

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
        public async Task<IActionResult> Create(MaterialTestReportDto MaterialTestReport,
                                                CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new CreateMaterialTestReportCommand(MaterialTestReport), token));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(MaterialTestReportDto MaterialTestReport, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new UpdateMaterialTestReportCommand(MaterialTestReport), token));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new DeleteMaterialTestReportCommand(id), token));
        }



        [HttpPost("uploadFile")]
        public async Task<IActionResult> Upload(string MaterialTestReportId, CancellationToken token = default)
        {
            if (Request.Form is null || !Request.Form.Files.Any()) return BadRequest();

            var file = Request.Form.Files[0];
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, token);

            return Ok(await Mediator.Send(new AddAttachmentToMaterialTestReport(MaterialTestReportId, file.FileName, stream), token));

        }
        [HttpDelete("removeFile")]
        public async Task<IActionResult> Upload(string materialTestReportId, string fileId, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(materialTestReportId) || string.IsNullOrEmpty(fileId)) return BadRequest();

            return Ok(await Mediator.Send(new RemoveAttachmentFromMaterialTestReport(materialTestReportId, fileId), token));

        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(string materialTestReportId, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(materialTestReportId)) return BadRequest();

            return Ok(await Mediator.Send(new GetMaterialTestReportByIdQuery(materialTestReportId), token));

        }
    }
}
