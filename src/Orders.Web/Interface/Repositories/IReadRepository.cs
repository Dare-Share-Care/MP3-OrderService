using Ardalis.Specification;

namespace Orders.Web.Interface.Repositories;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
    
}