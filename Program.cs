using Carter;
using FluentValidation;
using ImprovedPicpay.Abstractions;
using ImprovedPicpay.Data;
using ImprovedPicpay.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string databaseName = "simplified";
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseInMemoryDatabase(databaseName)
           .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

var assembly = Assembly.GetExecutingAssembly();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapCarter();

app.Run();
