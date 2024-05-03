using CryptoServiceApi.Startup;

// Start Up Configuration
var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();

// Start Web App Config
var app = builder.Build();

app.ConfigureSwagger();

app.UseHttpsRedirection();

app.MapUserEndpoints();

// Run Web App
app.Run();



