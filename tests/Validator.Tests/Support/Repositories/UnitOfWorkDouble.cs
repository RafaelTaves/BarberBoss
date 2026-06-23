using BarberBoss.Domain.Repositories;

namespace Validator.Tests.Support.Repositories;

public class UnitOfWorkDouble : IUnitOfWork
{
    public int CommitsCount { get; private set; }

    public Task Commit()
    {
        CommitsCount++;
        return Task.CompletedTask;
    }
}
