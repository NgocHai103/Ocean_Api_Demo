namespace Sea.Share.ElasticSearch.ElasticDtos;

public sealed class RequestElasticEto : ElasticEtoBase<Guid>
{
    public string Title { get; set; }

    public string CustomerName { get; set; }

    public string NumberPhone { get; set; }

    public string Code { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; }

    public Guid SubCategoryId { get; set; }

    public string SubCategoryName { get; set; }

    public Guid AssignedId { get; set; }

    ICollection<RequestDetailElasticDto> RequestDetails { get; set; }
}

public sealed class RequestDetailElasticDto : ElasticEtoBase<Guid>
{
    public Guid RequestId { get; set; }

    public int Status { get; set; }

    public Guid AssignedId { get; set; }
}
