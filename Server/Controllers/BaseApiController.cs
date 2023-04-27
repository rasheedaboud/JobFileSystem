using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NdtTracking.Wasm.Server.Controllers
{
    [TranslateResultToActionResult]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : Controller
    {

        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    }
}