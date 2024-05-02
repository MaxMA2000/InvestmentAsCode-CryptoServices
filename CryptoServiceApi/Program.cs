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



app.MapGet("/v1/crypto/{id}/{date}", async (int id, string date) =>
{
    using (var httpClient = new HttpClient())
    {
        var apiUrl = $"http://localhost:8080/data/v1/crypto/byAssetIdAndDateRange?asset_id={id}&from={date}&to={date}";
        var response = await httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();

            var jsonArray = JsonSerializer.Deserialize<List<CryptoPrice>>(jsonString);

            if (jsonArray.Count != 1)
            {
                return Results.BadRequest("The crypto data is not available or not unique.");
            }

            var cryptoPrice = jsonArray[0];

            return Results.Json(cryptoPrice);
        }
        else
        {
            return Results.BadRequest("Failed to retrieve crypto data.");
        }
    }
})
.WithName("GetSingleDayCryptoPrice")
.WithOpenApi();




app.MapGet("/v1/crypto/{id}/from/{from}/to/{to}", async (int id, string from, string to) =>
{
    using (var httpClient = new HttpClient())
    {
        var apiUrl = $"http://localhost:8080/data/v1/crypto/byAssetIdAndDateRange?asset_id={id}&from={from}&to={to}";
        // var apiUrl = $"http://localhost:3000/data/v1/crypto/byAssetIdAndDateRange?asset_id=4&from=2024-01-01&to=2024-01-10";
        var response = await httpClient.GetAsync(apiUrl);
        Console.WriteLine($"response: {response}");
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();

            var jsonArray = JsonSerializer.Deserialize<List<CryptoPrice>>(jsonString);

            return Results.Json(jsonArray);
        }
        else
        {
            return Results.BadRequest("Failed to retrieve crypto data.");
        }
    }
})
.WithName("GetPeriodCryptoPrice")
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


public class CryptoPrice
{
  public long id { get; set; }
  public long assetId { get; set; }
  public string symbol { get; set; }
  public DateTime date { get; set; }
  public decimal open { get; set; }
  public decimal high { get; set; }
  public decimal low { get; set; }
  public decimal close { get; set; }
  public decimal adjClose { get; set; }
  public long volume { get; set; }
  public long unadjustedVolume { get; set; }
  public decimal change { get; set; }
  public decimal changePercent { get; set; }
  public float vwap { get; set; }
  public string label { get; set; }
  public float changeOverTime { get; set; }
  public string asOfDate { get; set; }
}
