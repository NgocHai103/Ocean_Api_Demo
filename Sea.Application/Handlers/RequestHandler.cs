using DotNetCore.CAP;
using Sea.Share.RabbitMQ.Etos;
using Share.Sea.Common.Helpers;
using static Sea.Share.RabbitMQ.RabbitMQTopic;
using static Serilog.Log;

namespace Sea.Application.Handlers;

public class RequestHandler : ICapSubscribe
{
    [CapSubscribe(SEA_REQUEST_SEND)]
    public async Task Subscibe(RequestSendEto eventData) => Information("Sea-Request-Subscibe: {ETO}", eventData.Serialize());// Information("Sea-Request-Send: {Response}", await service.CloseGradeTarget(eventData));
}
