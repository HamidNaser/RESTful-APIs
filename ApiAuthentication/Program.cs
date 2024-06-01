using Authenication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();


builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddResponseCaching();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var secretKey = configuration["AppSettings:JwtSecretKey"];
var issuer = configuration["AppSettings:Issuer"];
var audience = configuration["AppSettings:Audience"];

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services
    .AddHttpContextAccessor()
    .AddRouting()
    .AddConnectServicesAndRepositories();

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .SetIsOriginAllowed((host) => true)
           .AllowCredentials(); ;
}));

var app = builder.Build();

//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenAI V1");
    });    
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();