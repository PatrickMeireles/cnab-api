using Ardalis.Result.AspNetCore;
using Cnab.Domain.Dto.Store;
using Cnab.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cnab.Api.Endpoints;

public static class StoreEndpoints
{
    public static IEndpointRouteBuilder AddStoreEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/stores", async ([FromServices] IStoreServices services, [AsParameters] StoreRequestParameterDto param) =>
        {
            var result = await services.Get(param);

            return result.ToMinimalApiResult();
        });

        app.MapGet("api/v1/stores/{id}/transactions", async ([FromServices] IStoreServices services, Guid id) =>
        {
            var result = await services.GetTransactions(id);

            return result.ToMinimalApiResult();
        });

        return app;
    }
}
