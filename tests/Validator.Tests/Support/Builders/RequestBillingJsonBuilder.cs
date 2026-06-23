using BarberBoss.Communication.Requests.Billing;

namespace Validator.Tests.Support.Builders;

public class RequestBillingJsonBuilder
{
    private readonly RequestBillingJson _request = new()
    {
        Date = new DateOnly(2026, 6, 23),
        BarberName = "Rafael",
        ClientName = "Joao",
        ServiceName = "Corte",
        Amount = 50,
        PaymentMethod = "Pix",
        Status = "Pago",
        Notes = "Cliente recorrente",
        CreatedAt = DateTime.UtcNow
    };

    public RequestBillingJsonBuilder WithInvalidBarberName()
    {
        _request.BarberName = string.Empty;
        return this;
    }

    public RequestBillingJsonBuilder WithServiceName(string serviceName)
    {
        _request.ServiceName = serviceName;
        return this;
    }

    public RequestBillingJsonBuilder WithAmount(decimal amount)
    {
        _request.Amount = amount;
        return this;
    }

    public RequestBillingJson Build()
    {
        return _request;
    }
}
