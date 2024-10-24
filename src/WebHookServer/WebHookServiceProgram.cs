using Serilog;
using WebHookServer.Models;
using WebHookServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<WebHookService>();
builder.Services.AddSerilog(sp =>
    sp.WriteTo.Console());
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/Subscribe", (WebHookService webHookService, Subscription subscription) =>
    {
        webHookService.Subscribe(subscription);
    })
    .WithName("Subscribe")
    .WithOpenApi();

app.MapPost("/Publish", (WebHookService webHookService, PublishRequest publishRequest) =>
    webHookService.PublishMessageAsync(publishRequest.Topic, publishRequest.Message));

app.Run();

namespace WebHookServer
{
    public partial class WebHookServiceProgram;
}