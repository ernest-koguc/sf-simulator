using NLog.Extensions.Logging;
using SFSimulatorAPI.IoC;
using SFSimulatorAPI.Mappings;

var logger = NLog.LogManager.GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddNLog();



builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    policy
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .AllowAnyHeader());
});

builder.Services.RegisterQuestSimulator();
builder.Services.RegisterControllerServices();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<SimulatorMappingProfiler>());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("CorsPolicy");
}

app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex);
    throw;
}

