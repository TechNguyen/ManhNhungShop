using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

var builder = WebApplication.CreateBuilder(args);
//config ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyMethod();
    });
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.UseCors(options => options.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
await app.UseOcelot();

app.Run(); 