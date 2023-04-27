using Core.Features.Contacts.Commands;
using Core.Features.Contacts.Queries;
using JobFileSystem.Shared.Contacts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NdtTracking.Wasm.Server.Controllers
{
    public class ContactsOdataController : ODataController
    {

        private readonly IMediator _mediator;

        public ContactsOdataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [EnableQuery]
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetContactsQuery());
            return Ok(result);
        }

        [EnableQuery]
        public async Task<SingleResult<ContactDto>> Get([FromODataUri] string key)
        {
            var result = await _mediator.Send(new GetContactsQuery());
            var filteredResult = result.Where(x=>x.Id==key).AsQueryable();
            return SingleResult.Create(filteredResult);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] ContactDto dto, CancellationToken token)
        {

            return Created(await _mediator.Send(new CreateContactCommand(dto), token));
        }

        [EnableQuery]
        public async Task<IActionResult> Patch([FromODataUri] string key, [FromBody] ContactDto dto)
        {           
            return Updated(await _mediator.Send(new UpdateContactCommand(dto)));
        }

        [EnableQuery]
        public async Task<IResult> Delete([FromODataUri] string key)
        {            
            return Results.Ok(_mediator.Send(new DeleteContactCommand(key)));
        }


    }
}
