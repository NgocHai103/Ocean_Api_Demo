using Sea.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sea.Domain.Entities.Sea;

[Table("Requests")]
public sealed class Request : EntityBase<Guid>
{
    [Required] // Title is required
    [MaxLength(100)] // Limit title to 100 characters
    public string Title { get; set; }

    [Required] // CustomerName is required
    [MaxLength(50)] // Limit customer name to 50 characters
    public string CustomerName { get; set; }

    [Required] // NumberPhone is required
    [Phone] // Ensure valid phone number format
    [MaxLength(15)] // Max length for phone number
    public string NumberPhone { get; set; }

    [Required] // Code is required
    [MaxLength(10)] // Max length 10 characters
    [Column(TypeName = "varchar(10)")] // Set SQL type as varchar(10)
    public string Code { get; set; }

    [Required] // Status is required
    public RequestStatuses Status { get; set; }

    [Required] // Type is required
    public RequestTypes Type { get; set; }

    [Required] // CategoryId is required
    public Guid CategoryId { get; set; }

    [MaxLength(50)] // Limit category name to 50 characters
    public string CategoryName { get; set; }

    [Required] // SubCategoryId is required
    public Guid SubCategoryId { get; set; }

    [MaxLength(50)] // Limit subcategory name to 50 characters
    public string SubCategoryName { get; set; }

    [Required] // AssignedId is required
    public Guid AssignedId { get; set; }

    [MaxLength(50)] // Limit assigned name to 50 characters
    public string AssignedName { get; set; }

    public ICollection<RequestHistory> RequestHistories { get; set; }
}
