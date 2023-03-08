namespace Services;
public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IOwnerService> lazyOwnerService;
    private readonly Lazy<IAccountService> lazyAccountService;
    public ServiceManager(IRepositoryManager repositoryManager)
    {
        this.lazyOwnerService = new Lazy<IOwnerService>(() => new OwnerService(repositoryManager));
        this.lazyAccountService = new Lazy<IAccountService>(() => new AccountService(repositoryManager));
    }
    public IOwnerService OwnerService => lazyOwnerService.Value;
    public IAccountService AccountService => lazyAccountService.Value;
}