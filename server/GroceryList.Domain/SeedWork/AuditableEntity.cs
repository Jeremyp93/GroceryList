﻿namespace GroceryList.Domain.SeedWork;

public abstract class AuditableEntity
{
    public DateTime? CreatedAt { get; private set; }

    public string CreatedBy { get; private set; } = string.Empty;

    public DateTime? LastModifiedAt { get; private set; }

    public string LastModifiedBy { get; private set; } = string.Empty;

    public void UpdateTrackingInformation(string authenticatedUser, DateTime modificationDate)
    {
        if (!CreatedAt.HasValue)
        {
            CreatedAt = modificationDate;
            CreatedBy = authenticatedUser;
        }
        LastModifiedAt = modificationDate;
        LastModifiedBy = authenticatedUser;
    }
}


