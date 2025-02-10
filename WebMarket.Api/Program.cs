using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using WebMarket.Api;
using WebMarket.Api.Middlewares;
using WebMarket.Application.DependencyInjection;
using WebMarket.Consumer.DependencyInjection;
using WebMarket.DAL.DependencyInjection;
using WebMarket.Domain.Settings;
using WebMarket.Producer.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));

builder.Services.AddControllers();

builder.Services.AddAuthenticationAndAuthorization(builder);
builder.Services.AddSwagger();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddProducer();
builder.Services.AddConsumer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebMarket Swagger v1.0");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebMarket Swagger v2.0");
    });
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.UseHttpsRedirection();
app.Run();

