using Sea.Application.Requests.Requests;
using Sea.Application.Responses;

namespace Sea.Application.Services.IServices;

public interface IRequestService
{
    public ValueTask<RequestResponse> GetUserById(Guid? id);

    public ValueTask<RequestResponse> Add(RequestCreateRequest request);

    public ValueTask<PagingResult<RequestResponse>> GetRequestList(int pageIndex, int pageSize);
}
