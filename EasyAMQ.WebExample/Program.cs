using EasyAMQ.Extensions;
using EasyAMQ.WebExample.Models;
using EasyAMQ.WebExample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ExampleProducer>();

builder.AddMessaging((services, setup) =>
{
    setup.AddConsumer<ExampleConsumer, ExampleMessage>();
    setup.AddProducer<ExampleProducer, ExampleMessage>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("produce", async (string message, ExampleProducer exampleProducer) =>
{
    await exampleProducer.CreateMessage(new ExampleMessage
    {
        Value = message
    });
});

app.Run();