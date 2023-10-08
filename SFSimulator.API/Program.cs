using FluentValidation;
using NLog.Extensions.Logging;
using SFSimulator.API.IoC;
using SFSimulator.API.Mappings;
using SFSimulator.API.Validation.Validators;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

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
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(SimulatorMappingProfiler))));

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<SimulateDungeonRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

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

