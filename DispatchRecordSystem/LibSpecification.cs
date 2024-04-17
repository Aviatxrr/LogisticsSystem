namespace DispatchRecordSystem;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
}
public abstract class CompositeSpecification<T> : ISpecification<T>
{
    public abstract bool IsSatisfiedBy(T entity);
    public ISpecification<T> And(ISpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }
}
public class AndSpecification<T> : CompositeSpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override bool IsSatisfiedBy(T entity)
    {
        return _left.IsSatisfiedBy(entity) && _right.IsSatisfiedBy(entity);
    }
}

public class ByStation<T> : CompositeSpecification<T> where T : ILocatable
{
    private int _stationId;

    public ByStation(int stationId)
    {
        _stationId = stationId;
    }
    public override bool IsSatisfiedBy(T entity)
    {
        return entity.StationId == _stationId;
    }
}

public class ByCompany<T> : CompositeSpecification<T> where T : IHaveCompany
{
    private int _companyId;

    public ByCompany(int companyId)
    {
        _companyId = companyId;
    }

    public override bool IsSatisfiedBy(T entity)
    {
        return entity.CompanyId == _companyId;
    }
}

public class ByUserName : ISpecification<User>
{
    private string _userName;

    public ByUserName(string userName)
    {
        _userName = userName;
    }

    public bool IsSatisfiedBy(User user)
    {
        return user.UserName == _userName;
    }
}
