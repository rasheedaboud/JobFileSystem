using Core.Features.JobFiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NdtTracking.Wasm.Server.Controllers
{

    public class JobFilesOdataController : ODataController
    {

        private readonly IMediator _mediator;

        public JobFilesOdataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [EnableQuery]
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetJobFilesQuery()));
        }
    }
}
