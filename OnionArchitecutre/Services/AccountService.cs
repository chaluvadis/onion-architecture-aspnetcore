namespace Services;
internal sealed class AccountService : IAccountService
{
    private readonly IRepositoryManager repositoryManager;
    public AccountService(IRepositoryManager repositoryManager) => this.repositoryManager = repositoryManager;
    public async Task<IEnumerable<AccountDto>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        var accounts = await repositoryManager.AccountRepository.GetAllByOwnerIdAsync(ownerId, cancellationToken);
        return accounts.Adapt<IEnumerable<AccountDto>>();
    }
    public async Task<AccountDto> GetByIdAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken)
    {
        var owner = await repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }
        var account = await repositoryManager.AccountRepository.GetByIdAsync(accountId, cancellationToken);
        if (account is null)
        {
            throw new AccountNotFoundException(accountId);
        }
        if (account.OwnerId != owner.Id)
        {
            throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
        }
        return account.Adapt<AccountDto>();
    }

    public async Task<AccountDto> CreateAsync(Guid ownerId, AccountForCreationDto accountForCreationDto, CancellationToken cancellationToken = default)
    {
        var owner = await repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }
        var account = accountForCreationDto.Adapt<Account>();
        account.OwnerId = owner.Id;
        repositoryManager.AccountRepository.Insert(account);
        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        return account.Adapt<AccountDto>();
    }

    public async Task DeleteAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default)
    {
        var owner = await repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }
        var account = await repositoryManager.AccountRepository.GetByIdAsync(accountId, cancellationToken);
        if (account is null)
        {
            throw new AccountNotFoundException(accountId);
        }
        if (account.OwnerId != owner.Id)
        {
            throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
        }
        repositoryManager.AccountRepository.Remove(account);
        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
