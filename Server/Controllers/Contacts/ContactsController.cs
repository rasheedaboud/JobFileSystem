using Core.Features.Contacts;
using Core.Features.Contacts.Commands;
using JobFileSystem.Shared.Contacts;
using Microsoft.AspNetCore.Mvc;
using NdtTracking.Wasm.Server.Controllers;

namespace JobFileSystem.Server.Controllers.Contacts
{
    public class ContactsController : BaseApiController
    {
        private readonly ContactValidator _validator;

        public ContactsController(ContactValidator validator)
        {
            _validator = validator;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate(ContactDto Contact)
        {
            var validationResult = await _validator.ValidateAsync(Contact);

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
        public async Task<IActionResult> Create(ContactDto Contact,
                                                CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new CreateContactCommand(Contact), token));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(ContactDto Contact, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new UpdateContactCommand(Contact), token));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id, CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new DeleteContactCommand(id), token));
        }

    }
}
