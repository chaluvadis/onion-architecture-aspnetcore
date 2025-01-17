﻿namespace Services;
internal sealed class OwnerService : IOwnerService
{
    private readonly IRepositoryManager repositoryManager;
    public OwnerService(IRepositoryManager repositoryManager)
        => this.repositoryManager = repositoryManager;
    public async Task<IEnumerable<OwnerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var owners = await repositoryManager.OwnerRepository.GetAllAsync(cancellationToken);
        return owners.Adapt<IEnumerable<OwnerDto>>();
    }

    public async Task<OwnerDto> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        var owner = await repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }
        return owner.Adapt<OwnerDto>();
    }

    public async Task<OwnerDto> CreateAsync(OwnerForCreationDto ownerForCreationDto, CancellationToken cancellationToken = default)
    {
        var owner = ownerForCreationDto.Adapt<Owner>();
        repositoryManager.OwnerRepository.Insert(owner);
        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        return owner.Adapt<OwnerDto>();
    }
    public async Task UpdateAsync(Guid ownerId, OwnerForUpdateDto ownerForUpdateDto, CancellationToken cancellationToken = default)
    {
        var owner = await repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }
        owner.Name = ownerForUpdateDto.Name;
        owner.DateOfBirth = ownerForUpdateDto.DateOfBirth;
        owner.Address = ownerForUpdateDto.Address;
        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        var owner = await repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
        if (owner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }
        repositoryManager.OwnerRepository.Remove(owner);
        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}