using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Serverless.Api.Common.Settings;
using Serverless.Api.Middleware.HttpLogger;

var builder = WebApplication.CreateBuilder(args);

/// Settings
var configuration = builder.Configuration;
var serviceSettings = configuration.GetSection(nameof(ServiceSettings));
builder.Services.Configure<ServiceSettings>(serviceSettings);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Serverless - V1",
            Version = "v1"
        }
     );

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "Serverless.Api.Controllers.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
               JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomHttpLogging();

app.MapControllers();

app.Run();
