using Ardalis.Specification;

namespace Orders.Web.Interface.Repositories;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    
}