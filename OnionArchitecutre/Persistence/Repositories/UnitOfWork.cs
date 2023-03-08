namespace Persistence.Repositories;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly RepositoryDbContext dbContext;
    public UnitOfWork(RepositoryDbContext dbContext) => this.dbContext = dbContext;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        this.dbContext.SaveChangesAsync(cancellationToken);
}