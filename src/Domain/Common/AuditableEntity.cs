namespace Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public Guid? CreatedByUserId { get; protected set; }
    public Guid? UpdatedByUserId { get; protected set; }

    public void SetCreatedBy(Guid userId)
    {
        CreatedByUserId = userId;
        UpdatedByUserId = userId;
    }

    public void SetUpdatedBy(Guid userId)
    {
        UpdatedByUserId = userId;
        Touch();
    }
}