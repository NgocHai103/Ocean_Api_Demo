namespace Sea.Share.ElasticSearch.ElasticDtos;

public record DocumentResultEto<T>(IEnumerable<T> Data, int TotalItemsCount, int PageIndex, int PageSize);