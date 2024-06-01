
using Microsoft.OpenApi.Models;
using System.Reflection;
using RESTful_APIs;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers().AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiDataFormat", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiDataFormat V1");
    });
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");


app.UseAuthorization();

app.MapControllers();

app.Run();