using Core.Features.MaterialTestReports.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NdtTracking.Wasm.Server.Controllers
{
    public class MaterialTestReportsOdataController : ODataController
    {

        private readonly IMediator _mediator;

        public MaterialTestReportsOdataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [EnableQuery]
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetMaterialTestReportsQuery());
            return Ok(result);
        }
    }
}
