using Scalar.AspNetCore;
using System;
using Minio;
using Minio.DataModel;
using Minio.Credentials;
using Minio.DataModel.Args;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint("play.min.io")
    .WithCredentials("minioadmin", "minioadmin")
    .WithSSL(false)
    .Build());
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
