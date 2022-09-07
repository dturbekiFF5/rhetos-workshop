using Autofac.Core;
using Rhetos;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Rhetos configuration ( useful convention )
void ConfigureRhetosHostBuilder(IServiceProvider serviceProvider, IRhetosHostBuilder rhetosHostBuilder) {
    rhetosHostBuilder
        .ConfigureRhetosAppDefaults()
        .UseBuilderLogProviderFromHost(serviceProvider)
        .ConfigureConfiguration(cfg => cfg.MapNetCoreConfiguration(builder.Configuration));
}

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(o => {
        // Using NewtonsoftJson for backward-compatibility with older versions of RestGenerator:
        // 1. Properties starting with uppercase in JSON objects.
        o.UseMemberCasing();
        // 2. Legacy Microsoft DateTime serialization.
        o.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
        // 3. byte[] serialization as JSON array of integers instead of Base64 string.
        o.SerializerSettings.Converters.Add(new Rhetos.Host.AspNet.RestApi.Utilities.ByteArrayConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.ToString()); // Allows multiple entities with the same name in different modules.
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestApp", Version = "v1" });
    // Adding Rhetos REST API to Swagger with document name "rhetos".
    c.SwaggerDoc("rhetos", new OpenApiInfo { Title = "Rhetos REST API", Version = "v1" });
});
// Add Rhetos service
builder.Services.AddRhetosHost(ConfigureRhetosHostBuilder)
    .AddAspNetCoreIdentityUser()
    .AddHostLogging()
    .AddDashboard()
    .AddRestApi(o => {
        o.BaseRoute = "rest";
        o.GroupNameMapper = (conceptInfo, controller, oldName) => "rhetos"; // OpenAPI document name.
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/rhetos/swagger.json", "Rhetos REST API");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestApp v1");
    });
    app.MapRhetosDashboard();
}

app.UseRhetosRestApi();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
