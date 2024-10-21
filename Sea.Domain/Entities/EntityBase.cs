using System;
using System.ComponentModel.DataAnnotations;

namespace Sea.Domain.Entities;
public interface IEntity<TKey>
{
    TKey Id { get; set; }
}

public class EntityBase<TKey> : IEntity<TKey>
{
    [Key]
    public TKey Id { get; set; }

    public TKey? CreatedBy { get; set; }

    public TKey? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }
}