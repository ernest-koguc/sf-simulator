using SFSimulator.Api;
using SFSimulator.Core;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

foreach (var type in TypesMapper.Types)
{
    builder.Services.AddScoped(type.Interface, type.Implementation);
}

builder.Services.AddScoped<SimulateDungeonHandler>();
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors(b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.MapGroup("api/dungeon")
    .MapPost("/simulate", (SimulateDungeonRequest request, SimulateDungeonHandler handler) =>
    {
        return handler.HandleSimulateDungeonAsync(request);
    });

app.Run();

[JsonSerializable(typeof(SimulateDungeonResponse))]
[JsonSerializable(typeof(List<SimulateDungeonResponse>))]
[JsonSerializable(typeof(SimulateDungeonRequest))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
