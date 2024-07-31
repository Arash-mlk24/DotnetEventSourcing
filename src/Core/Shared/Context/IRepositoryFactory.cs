using DotnetEventSourcing.src.Core.Shared.Models;

namespace DotnetEventSourcing.src.Core.Shared.Context;

public interface IRepositoryFactory
{
    IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : BaseEntity;
}
