using AutoMapper;
using Sea.Application.Requests.Requests;
using Sea.Application.Responses;
using Sea.Domain.Entities.Sea;
using Sea.Share.ElasticSearch.ElasticDtos;

namespace Sea.Application.Mappers;
public sealed class RequestMapper : Profile
{
    public RequestMapper()
    {
        _ = CreateMap<Request, RequestResponse>();
        _ = CreateMap<RequestCreateRequest, Request>();
        _ = CreateMap<Request, RequestElasticEto>();
        _ = CreateMap<RequestElasticEto, RequestResponse>();
        _ = CreateMap(typeof(DocumentResultEto<>), typeof(PagingResult<>));
        _ = CreateMap<DocumentResultEto<RequestResponse>, PagingResult<RequestResponse>>()
           .ForMember(dest => dest.Paging, opt => opt.MapFrom(src => new Paging(src.TotalItemsCount, src.PageIndex, src.PageSize)));
    }
}
