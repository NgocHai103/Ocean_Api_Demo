using Sea.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sea.Domain.Entities.Sea;
public sealed class RequestHistory : EntityBase<Guid>
{
    [Required] // RequestId is required
    public Guid RequestId { get; set; }

    [Required] // AssigneeId is required
    public Guid AssigneeId { get; set; }

    [MaxLength(50)] // Limit AssigneeName to 50 characters
    public string AssigneeName { get; set; }

    [Required] // Status is required
    public RequestStatuses Status { get; set; }

    [MaxLength(500)] // Limit Description to 500 characters
    public string Description { get; set; }

    [ForeignKey("RequestId")]
    public Request Request { get; set; }
}
