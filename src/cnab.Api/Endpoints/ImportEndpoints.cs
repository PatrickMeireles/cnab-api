using Ardalis.Result.AspNetCore;
using Cnab.Domain.Dto.Import;
using Cnab.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cnab.Api.Endpoints;

public static class ImportEndpoints
{
    public static IEndpointRouteBuilder AddImportEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/import-cnab", async ([FromServices] IImportServices services, HttpContext context, CancellationToken cancellationToken) =>
        {
            var id = Guid.NewGuid();
            var formFile = context.Request.Form.Files["file"];

            if (formFile is not null && formFile.Length > 0)
            {
                if (!formFile.FileName.Contains(".txt"))
                    return Results.StatusCode(415);

                var lines = new List<string>();

                using (var reader = new StreamReader(formFile.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        var line = await reader.ReadLineAsync();

                        if (!string.IsNullOrWhiteSpace(line))
                            lines.Add(line);
                    }
                }

                var param = new ImportRequestDto(id, formFile.FileName, lines);

                var response = await services.Import(param, cancellationToken);

                if (!response.IsSuccess)
                    return Results.Conflict();

                var result = new
                {
                    Id = id,
                    Message = "File Accepted"
                };

                return Results.Accepted("", result);
            }
            return Results.BadRequest("Format File is not valid.");

        })
            .WithTags("import");

        app.MapGet("api/v1/import/{id}", async ([FromServices] IImportServices services, Guid id) =>
        {
            var result = await services.Get(id);

            return result.ToMinimalApiResult();
        })
            .WithTags("import");
        return app;
    }

}
