using System;
using Domain.Repositories;

namespace Persistence.Repositories;
public sealed class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IOwnerRepository> lazyOwnerRepository;
    private readonly Lazy<IAccountRepository> lazyAccountRepository;
    private readonly Lazy<IUnitOfWork> lazyUnitOfWork;
    public RepositoryManager(RepositoryDbContext dbContext)
    {
        this.lazyOwnerRepository = new Lazy<IOwnerRepository>(() => new OwnerRepository(dbContext));
        this.lazyAccountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(dbContext));
        this.lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
    }
    public IOwnerRepository OwnerRepository => lazyOwnerRepository.Value;
    public IAccountRepository AccountRepository => lazyAccountRepository.Value;
    public IUnitOfWork UnitOfWork => lazyUnitOfWork.Value;
}