using Brewery.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

var adIssuer = builder.Configuration["AzureAd:Issuer"];
var validAudiences = builder.Configuration.GetSection("AzureAd:ValidAudiences").Get<string[]>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = adIssuer,
        ValidateAudience = true,
        ValidAudiences = validAudiences,
        ValidateLifetime = true,
        IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
        {
            var jwtToken = new JsonWebToken(token);
            var signingKeys = new List<SecurityKey>();
            // Logic to retrieve signing keys based on the token's header and configuration
            // This typically involves fetching the OpenID Connect metadata from Azure AD
            // and extracting the signing keys.
            return signingKeys;
        }
    };
});

builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<IBreweryService, BreweryService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
}
else
{
    //app.MapGet("/api/v1/breweries",async(IBreweryService s)=> await s.GetAsync());
    app.MapGet("/", () => "API Up and Running");
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
