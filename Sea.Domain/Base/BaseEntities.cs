using System;
using System.ComponentModel.DataAnnotations;

namespace Sea.Domain.Base;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}

public abstract class TrackedEntity
{
    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? ModifiedBy { get; set; }

    public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;

    public bool IsActived { get; set; } = false;

}

public abstract class BaseEntity<TKey> : TrackedEntity, IEntity<TKey>
{
    public TKey Id { get; set; }
}