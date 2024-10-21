using AutoMapper;
using DotNetCore.CAP;
using Sea.Application.Requests.Requests;
using Sea.Application.Responses;
using Sea.Application.Services.IServices;
using Sea.Domain.Entities.Sea;
using Sea.Domain.Enums;
using Sea.Domain.UnitOfWorks;
using Sea.Share.ElasticSearch.ElasticDtos;
using Sea.Share.ElasticSearch.Services;
using static Sea.Share.RabbitMQ.RabbitMQTopic;
using Sea.Share.RabbitMQ.Etos;
using Serilog;
using static Sea.Share.ElasticSearch.ElasticConstants;

namespace Sea.Application.Services;

public class RequestService(IUnitOfWork<Request, Guid> _unitOfWork, IMapper _mapper, IElasticSearchService<RequestElasticEto, Guid> _elasticSearchService, ICapPublisher _capPublisher) : IRequestService
{
    public async ValueTask<RequestResponse> Add(RequestCreateRequest request)
    {
        try
        {
            //create 1 milions records
            var size = 10_000;  // Số lượng records cần tạo
            var requests = new Request[size];  // Mảng chứa 1 triệu đối tượng Request

            // Sử dụng Parallel.For để tạo các đối tượng Request song song
            _ = Parallel.For(0, size, i => requests[i] = new Request
            {
                Id = Guid.NewGuid(),
                Title = $"Title_{i}",
                CustomerName = $"Customer_{i}",
                NumberPhone = $"012345678{i % 10}",  // Tạo số điện thoại ngẫu nhiên
                Code = $"Code_{i % 1000}",  // Tạo code với giá trị từ 0 đến 999
                Status = (RequestStatuses)(i % Enum.GetValues(typeof(RequestStatuses)).Length), // Gán status ngẫu nhiên
                Type = (RequestTypes)(i % Enum.GetValues(typeof(RequestTypes)).Length),  // Gán type ngẫu nhiên
                CategoryId = Guid.NewGuid(),  // Tạo Guid ngẫu nhiên cho CategoryId
                CategoryName = $"Category_{i % 100}",  // Tạo tên category
                SubCategoryId = Guid.NewGuid(),  // Tạo Guid ngẫu nhiên cho SubCategoryId
                SubCategoryName = $"SubCategory_{i % 100}",  // Tạo tên subcategory
                AssignedId = Guid.NewGuid(),  // Tạo Guid ngẫu nhiên cho AssignedId
                AssignedName = $"Assigned_{i % 100}",  // Tạo tên người được giao
            });

            var uerId = Guid.NewGuid();
            var newRequest = _mapper.Map<Request>(request);
            newRequest.Code = "RQ579";

            await _unitOfWork.Repository.BulkInsertAsync(uerId, requests);
            // await _unitOfWork.Repository.InsertAsync(uerId, requests);
            // await _unitOfWork.Repository.InsertAsync(uerId, newRequest);
            _ = await _unitOfWork.SaveChangesAsync();

            _ = await _elasticSearchService.BulkIndexAsync(Sea_Demo_Request_Index, _mapper.Map<IList<RequestElasticEto>>(requests));

            //Console.WriteLine($"Thời gian xử lý insert db: {stopwatch.ElapsedMilliseconds} ms");
            //Log.Information("[RequestService] [Add] Thời gian xử lý insert db {message}", stopwatch.ElapsedMilliseconds);

            //_ = await _elasticSearchService.IndexAsync(Sea_Demo_Request_Index, _mapper.Map<RequestElasticEto>(newRequest));

            return _mapper.Map<RequestResponse>(newRequest);
        }
        catch (Exception ex)
        {
            Log.Error("[RequestService] [Add] Error {message}", ex.Message);
            throw;
        }
    }

    public async ValueTask<PagingResult<RequestResponse>> GetRequestList(int pageIndex, int pageSize)
    {
        try
        {
             await _capPublisher.PublishAsync(SEA_REQUEST_SEND, new RequestSendEto { Message = "Test RQMQ"});

            var requests = await _elasticSearchService.SearchAsync(Sea_Demo_Request_Index, s => s.Query(q => q.MatchAll()), pageIndex, pageSize);

            return _mapper.Map<PagingResult<RequestResponse>>(requests);
        }
        catch (Exception ex)
        {
            Log.Error("[RequestService] [GetRequestList] Error {message}", ex.Message);
            throw;
        }
    }

    public async ValueTask<RequestResponse> GetUserById(Guid? id)
    {
        try
        {
            id = id != Guid.Empty ? id : new Guid("1B837476-FC71-4A57-8A4A-08DCE8344C83");

            var requests = id is not null ? await _elasticSearchService.GetByIdAsync(Sea_Demo_Request_Index, id.Value) : default;

            return _mapper.Map<RequestResponse>(requests);
        }
        catch (Exception ex)
        {
            Log.Error("[RequestService] [GetUserById] Error {message}", ex.Message);
            throw;
        }
    }
}
