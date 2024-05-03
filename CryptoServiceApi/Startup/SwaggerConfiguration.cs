using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoServiceApi.Startup;
public static class SwaggerConfiguration
{
      public static WebApplication ConfigureSwagger(this WebApplication app)
      {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
      }
}
