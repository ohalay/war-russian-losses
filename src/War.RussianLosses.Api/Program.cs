using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<WarContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgre")));

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<WarContext>(DbContextKind.Pooled)
    .AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

app.MapGraphQL();

app.Run();
