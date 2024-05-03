namespace CryptoServiceApi.Startup;

public static class DependencyInjectionSetup
{

  public static IServiceCollection RegisterServices(this IServiceCollection services)
  {

      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();

      return services;
  }


}
