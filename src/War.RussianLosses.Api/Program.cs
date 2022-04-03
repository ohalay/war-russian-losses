using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<WarContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgre")))
    .AddScoped<ImageBuilder>()
    .AddScoped<LossesAmountService>()
    .AddTransient<LossesDataLoader>()
    .AddHostedService<AddLastLossesHostedService>();

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
    return Results.File(imgStream, contentType: "image/jpeg");
});

app.Run();
