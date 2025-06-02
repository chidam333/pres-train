using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using FaqAiApi.Interfaces;
using FaqAiApi.Repositories;
using FaqAiApi.Services;
using FaqAiApi.Mappings;
using FaqAiApi.Contexts;

var builder = WebApplication.CreateBuilder(args);

var geminiKey = builder.Configuration["GEMINI"];

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddDbContext<FaqContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IBotRequestRepository, BotRequestRepository>();
builder.Services.AddScoped<BotResponseRepository>();

builder.Services.AddScoped<IBotRequestService, BotRequestService>();
builder.Services.AddScoped<BotResponseService>();

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