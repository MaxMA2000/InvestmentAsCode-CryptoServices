using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
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


app.MapGet("/v1/cryptos", async () =>
{
    using (var httpClient = new HttpClient())
    {
        var response = await httpClient.GetAsync("http://localhost:3000/v1/asset/byType?type=crypto");

        if (response.IsSuccessStatusCode)
        {

            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonArray = JsonSerializer.Deserialize<List<CryptoAsset>>(jsonString);

            return Results.Json(jsonArray);

        }
        else
        {
            return Results.BadRequest("Failed to retrieve the crypto list.");
        }
    }
})
.WithName("GetAllCryptosInfo")
.WithOpenApi();

app.MapGet("/v1/crypto/{id}", async (int id) =>
{
    using (var httpClient = new HttpClient())
    {
        var apiUrl = $"http://localhost:3000/v1/asset/byId?id={id}";
        var response = await httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var crypto = JsonSerializer.Deserialize<CryptoAsset>(jsonString);

            if (crypto.type != "crypto")
              {
                  return Results.BadRequest("The input id is not a crypto.");
              }

            return Results.Json(crypto);
        }
        else
        {
            return Results.BadRequest("Failed to retrieve the crypto.");
        }
    }
})
.WithName("GetCryptoInfoById")
.WithOpenApi();


app.Run();



public class CryptoAsset
{
    public int assetId { get; set; }
    public string symbol { get; set; }
    public string name { get; set; }
    public string exchange { get; set; }
    public string exchangeShortName { get; set; }
    public string type { get; set; }
    public DateTime asOfDate { get; set; }
}

