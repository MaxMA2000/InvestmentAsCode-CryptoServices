using CryptoServiceApi.Startup;
using CryptoServiceApi.Controllers;

// Start Up Configuration
var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();

// Start Web App Config
var app = builder.Build();

app.ConfigureSwagger();

app.UseHttpsRedirection();

app.MapCryptoInfoController();
app.MapCryptoPriceController();

// Run Web App
app.Run();



