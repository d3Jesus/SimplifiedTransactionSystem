using Carter;
using ImprovedPicpay.Data;
using ImprovedPicpay.Repositories;
using ImprovedPicpay.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string databaseName = "simplified";
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseInMemoryDatabase(databaseName)
           .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

builder.Services.AddScoped<UserRepository>().AddScoped<UserService>();
builder.Services.AddScoped<TransactionRepository>().AddScoped<TransactionService>();
builder.Services.AddScoped<NotificationService>();

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
