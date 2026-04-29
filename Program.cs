
//using BibliotecaWebApi_JWT.Controllers;
using BibliotecaWebApi_JWT.Data;
//using BibliotecaWebApi_JWT.Dtos;
//using BibliotecaWebApi_JWT.Modelos;
using BibliotecaWebApi_JWT.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;

namespace BibliotecaWebApi_JWT
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
                options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status400BadRequest));
                options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status404NotFound));
            }).AddXmlSerializerFormatters();
            builder.Services.AddDbContext<BibliotecaDbContext>(opt =>
            {
                var cs = builder.Configuration.GetConnectionString("BibliotecaDB");
                opt.UseSqlServer(cs);
            });
            var jwt = builder.Configuration.GetSection("Jwt");
            string issuer = jwt["Issuer"] ?? "";
            string audience = jwt["Audience"] ?? "";
            string key = jwt["Key"] ?? "";
            if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience) || string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("Config JWT incompleta en appsettings.json (Jwt:Issuer/Audience/Key).");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateIssuerSigningKey = true,
                     ValidateLifetime = true,
                     ValidIssuer = issuer,
                     ValidAudience = audience,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                     ClockSkew = TimeSpan.FromSeconds(30)
                 };
             });
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Biblioteca Web API",
                    Version = "v1",
                    Description = "API REST con EF Core + JSON/XML + JWT (Bearer)"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Introduce: Bearer {tu_token}"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                        Array.Empty<string>()
                    }
                });
            
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("/api/v1/auth/login", UriKind.Relative),
                            Scopes = new Dictionary<string, string> { { "api", "Acceso a la API" } }
                        }
                    }
                });
            });
                var app = builder.Build();
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca Web API v1");
                        c.OAuthClientId("swagger");
                        c.OAuthAppName("Biblioteca API Swagger");
                    });
                }
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var problem = new ProblemDetails
                        {
                            Title = "Error no controlado",
                            Status = StatusCodes.Status500InternalServerError,
                            Detail = "Se produjo un error inesperado. Revisa logs para más detalle."
                        };
                        context.Response.StatusCode = problem.Status!.Value;
                        context.Response.ContentType = "application/problem+json";
                        await context.Response.WriteAsJsonAsync(problem);
                    });
                });


                app.UseHttpsRedirection();

                app.UseAuthentication();
            
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
        }   
    }
}
