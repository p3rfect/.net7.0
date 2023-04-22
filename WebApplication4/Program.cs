using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApplication4.Models.Interfaces;
using WebApplication4.Models.Services;
using Microsoft.IdentityModel.Tokens;
using WebApplication4.jwt;
using Microsoft.AspNetCore.Builder;
using WebApplication4.Models;
using WebApplication4.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Org.BouncyCastle.Utilities.Net;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.RequireHttpsMetadata= false;
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
        
    });

builder.Services.AddCors();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseFileServer();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseAuthorization();
app.UseCors(builder => builder.AllowAnyOrigin());
app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Map("/verifyip", [Authorize] async (string email, HttpContext context) =>
{
    User user = await DatabaseWRK.GetUserByEmailAsync(email);
    if (user.Role != "user")
    {
        string safeList = app.Configuration["AdminSafeList"];
        var remoteIp = context.Connection.RemoteIpAddress;
        bool result = false;
        var ipList = safeList.Split(';');
        foreach (var adress in ipList)
        {
            var ip = remoteIp.ToString();
            if (ip == adress)
            {
                result = true;
                break;
            }
        }
        context.Response.StatusCode = result ? 200 : 500;
    }
    else context.Response.StatusCode = 200;
});

app.Run();
