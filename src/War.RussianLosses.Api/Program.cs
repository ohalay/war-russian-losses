using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<WarContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString(Const.DbType)))
    .AddHttpClient(Const.SelfHttpClientName, s => s.BaseAddress = new Uri(builder.Configuration.GetValue<string>(Const.AppBseUrlKey))).Services
    .AddScoped<ImageBuilder>()
    .AddScoped<LossesAmountService>()
    .AddTransient<LossesDataLoader>()
    .AddHostedService<AddLastLossesHostedService>()
    .AddHostedService<WarmerHostedService>()
    .AddHealthChecks()
        .AddDbContextCheck<WarContext>(Const.DbType);

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<WarContext>(DbContextKind.Pooled)
    .AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

app.MapGraphQL();
app.MapGet("/img", async (DateOnly? from, DateOnly? to, ImageBuilder builder) => 
{
    var imgStream = await builder.BuildImgStreamAsync(from, to);
    return Results.File(imgStream, contentType: MediaTypeNames.Image.Jpeg);
});

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        var response = new
        {
            Status = report.Status.ToString(),
            HealthChecks = report.Entries.Select(x => new { Components = x.Key, Status = x.Value.Status.ToString() }),
            HealthCheckDuration = report.TotalDuration
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.Run();
