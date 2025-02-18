using FinancialAccountManagement.API.Data;
using FinancialAccountManagement.API.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//FinancialAccount 
builder.Services.AddDbContext<FinancialAccountDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("FinancialAccountConnectionString")); //from appsetting connection string
});

// Register the repository
builder.Services.AddScoped<IRepository, Repository>();



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

app.Run();
