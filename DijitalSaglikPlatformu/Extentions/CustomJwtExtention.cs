using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DijitalSaglikPlatformu.Extentions
{
    public static class CustomJwtExtention
    {

        public static void AddCustomJwtAuth(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
                };
            });



        }





        public static void AddSwaggerGenJwtAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "digital health platform",
                    Contact = new OpenApiContact()
                    {
                        Name = "Amin Albouzan",
                        Email = "bozanamin04@gmail.com",
                        Url = new Uri("https://amin-albouzan.netlify.app/")
                    }
                });

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the JWT key"
                });


                o.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme()
                    {
                         Reference = new OpenApiReference()
                         {
                              Type = ReferenceType.SecurityScheme,
                               Id = "Bearer"
                         },
                            Name = "Bearer",
                            In = ParameterLocation.Header

                    },
                        new List<string>()

                    }
                });


            });
        }




    }
}
