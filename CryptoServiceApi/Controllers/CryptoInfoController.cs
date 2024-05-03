namespace CryptoServiceApi.Controllers;


using System.Text.Json;

public static class CryptoInfoController
{

  private const string AssetServiceUrl = "http://localhost:3000";

  public static WebApplication MapCryptoInfoController(this WebApplication app)
  {

      app.MapGet("/v1/cryptos", async () =>
      {
          using (var httpClient = new HttpClient())
          {
              var response = await httpClient.GetAsync($"{AssetServiceUrl}/v1/asset/byType?type=crypto");

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
              var apiUrl = $"{AssetServiceUrl}/v1/asset/byId?id={id}";
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


      return app;
  }

}
