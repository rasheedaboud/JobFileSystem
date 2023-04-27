using Core.Features.JobFiles;
using Core.Features.JobFiles.Commands;
using JobFileSystem.Shared.JobFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NdtTracking.Wasm.Server.Controllers;

namespace JobFileSystem.Server.Controllers.JobFiles
{
    public class JobFilesController : BaseApiController
    {
        private readonly JobFileValidator _validator;

        public JobFilesController(JobFileValidator validator)
        {
            _validator = validator;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate(JobFileDto jobfile)
        {
            var validationResult = await _validator.ValidateAsync(jobfile);

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
        public async Task<IActionResult> Create(JobFileDto jobfile,
                                                CancellationToken token = default)
        {           
            return Ok(await Mediator.Send(new CreateJobFile(jobfile), token));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(JobFileDto jobFile, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new UpdateJobFile(jobFile), token));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new DeleteJobFile(id), token));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(string id, string fileName, CancellationToken token = default)
        {
            if (Request.Form is null || !Request.Form.Files.Any()) return BadRequest();

            var file = Request.Form.Files[0];
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, token);

            return Ok(await Mediator.Send(new AddAttachmentToJobFile(id, fileName, stream), token));

        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFile(string id, string fileId, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new RemoveAttachmentToJobFile(id, fileId), token));
        }
    }
}
