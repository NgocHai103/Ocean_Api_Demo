using Sea.Domain.Enums;

namespace Sea.Application.Requests.Requests;
public sealed record RequestCreateRequest
{
    public string Title { get; set; }

    public string CustomerName { get; set; }

    public string NumberPhone { get; set; }

    public RequestStatuses Status { get; set; }

    public RequestTypes Type { get; set; }

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; }

    public Guid SubCategoryId { get; set; }

    public string SubCategoryName { get; set; }

    public Guid AssignedId { get; set; }
}
