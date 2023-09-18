using ManhNhungShop_Order_Service.Models;
using ManhNhungShop_Order_Service.Repository;
using ManhNhungShop_Order_Service.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<ICachingServices, CachingService>();

//add service to connectioner
builder.Services.Configure<OrderDatabaseSetting>(builder.Configuration.GetSection("OrderServiceDatabase"));

builder.Services.AddSingleton<OrderRep>();



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
