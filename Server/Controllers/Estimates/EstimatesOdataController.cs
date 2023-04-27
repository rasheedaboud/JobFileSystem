using Core.Features.Estimates.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NdtTracking.Wasm.Server.Controllers
{
    public class EstimatesOdataController : ODataController
    {

        private readonly IMediator _mediator;

        public EstimatesOdataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [EnableQuery]
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetEstimatesQuery());
            return Ok(result);
        }
    }
}
