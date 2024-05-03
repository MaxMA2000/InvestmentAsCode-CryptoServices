namespace CryptoServiceApi.Startup;

using System.Text.Json;

public static class MapEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {

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


      return app;
    }
}
