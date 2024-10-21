namespace Sea.Application.Responses;

public interface IResult
{

    bool Success { get; }

    int? ErrorCode { get; }

    string Message { get; }
}

public interface IPagingResult<TModel> : IResult
{
    Paging Paging { get; }

    IEnumerable<TModel> Data { get; }

    object Criteria { get; }
}

public record Result(bool Success, int? ErrorCode, string Message, object Data) : IResult;

public record Result<TModel>(bool Success, int? ErrorCode, string Message, TModel Data) : IResult;

public record Paging(int? TotalItemsCount, int PageIndex = 1, int PageSize = 10);

public record PagingResult<TModel>(IEnumerable<TModel> Data, Paging Paging = null, bool Success = true, int? ErrorCode = null, string Message = null, object Criteria = null) : IPagingResult<TModel>;