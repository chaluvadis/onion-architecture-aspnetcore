namespace Persistence.Repositories;
internal sealed class AccountRepository : IAccountRepository
{
    private readonly RepositoryDbContext dbContext;
    public AccountRepository(RepositoryDbContext dbContext) => this.dbContext = dbContext;
    public async Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
        await this.dbContext.Accounts.Where(x => x.OwnerId == ownerId).ToListAsync(cancellationToken);
    public async Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default) =>
        await this.dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);
    public void Insert(Account account) => this.dbContext.Accounts.Add(account);
    public void Remove(Account account) => this.dbContext.Accounts.Remove(account);
}
