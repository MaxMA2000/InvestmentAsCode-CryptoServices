namespace CryptoServiceApi.Controllers;


using System.Text.Json;

public static class CryptoPriceController
{
  private const string DALUrl = "http://localhost:8080";
  public static WebApplication MapCryptoPriceController(this WebApplication app)
  {


      app.MapGet("/v1/crypto/{id}/{date}", async (int id, string date) =>
      {
          using (var httpClient = new HttpClient())
          {
              var apiUrl = $"{DALUrl}/data/v1/crypto/byAssetIdAndDateRange?asset_id={id}&from={date}&to={date}";
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
              var apiUrl = $"{DALUrl}/data/v1/crypto/byAssetIdAndDateRange?asset_id={id}&from={from}&to={to}";

              var response = await httpClient.GetAsync(apiUrl);

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
