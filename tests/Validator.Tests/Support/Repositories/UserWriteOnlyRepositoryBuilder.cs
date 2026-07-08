namespace Validator.Tests.Support.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    private bool _deleteResult;

    public UserWriteOnlyRepositoryBuilder WithDeleteResult(bool deleteResult)
    {
        _deleteResult = deleteResult;
        return this;
    }

    public UserWriteOnlyRepositoryDouble Build()
    {
        return new UserWriteOnlyRepositoryDouble(_deleteResult);
    }
}
