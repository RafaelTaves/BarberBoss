namespace Validator.Tests.Support.Repositories;

public class BillingWriteOnlyRepositoryBuilder
{
    private bool _deleteResult;

    public BillingWriteOnlyRepositoryBuilder WithDeleteResult(bool deleteResult)
    {
        _deleteResult = deleteResult;
        return this;
    }

    public BillingWriteOnlyRepositoryDouble Build()
    {
        return new BillingWriteOnlyRepositoryDouble(_deleteResult);
    }
}
