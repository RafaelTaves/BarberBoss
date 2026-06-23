using BarberBoss.Application.UseCases.Billing.GetAll;
using BarberBoss.Application.UseCases.Billing.Register;
using BarberBoss.Application.UseCases.Billing.Update;
using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Communication.Responses;
using BarberBoss.Communication.Responses.Billing;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BillingController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredBillingJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBilling(
        [FromServices] IRegisterBillingUseCase useCase,
        [FromBody] RequestBillingJson request)
    {
        var response = useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseRegisteredBillingJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllBillings(
        [FromServices] IGetAllBillingsUseCase useCase)
    {
        var response = await useCase.Execute();

        if(response.Billings.Count != 0 )
        return Ok(response);

        return NoContent();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseRegisteredBillingJson), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBilling(
        [FromServices] IUpdateBillingUseCase useCase,
        [FromRoute] Guid id,
        [FromBody] RequestBillingJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }
}
