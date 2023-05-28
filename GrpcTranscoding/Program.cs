using System.Net;
using System.Security.Authentication;
using GrpcTranscoding.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Loopback, 25000, o =>
    {
        o.Protocols = HttpProtocols.Http1;
        o.UseHttps();
    });

    options.Listen(IPAddress.Loopback, 25001, o =>
    {
        o.Protocols = HttpProtocols.Http2;
    });
});

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcReflection();

builder.Services.AddSwaggerGen();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.MapGrpcReflectionService();
app.MapGrpcService<GreeterService>();
app.MapGet("greeter/{name}", (string name) => Results.Ok(new
{
    Message = "Hello " + name
}));

app.Run();
