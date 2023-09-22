using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

var builder = WebApplication.CreateBuilder(args);
//config ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
await app.UseOcelot();
app.Run();
    