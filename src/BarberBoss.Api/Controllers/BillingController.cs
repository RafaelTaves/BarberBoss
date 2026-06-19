using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Communication.Responses.Billing;
using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using BarberBoss.Application.UseCases.Billing.Register;

namespace BarberBoss.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BillingController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredBillingJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public IActionResult CreateBilling(
        [FromServices] IRegisterBillingUseCase usecase,
        [FromBody] RequestBillingJson request
        )
    {
        
        var response = usecase.Execute(request);

        return Created(string.Empty, response);
    }
}
