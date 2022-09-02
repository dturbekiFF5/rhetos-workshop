using Rhetos;

var builder = WebApplication.CreateBuilder(args);

// Rhetos configuration ( useful convention )
void ConfigureRhetosHostBuilder(IServiceProvider serviceProvider, IRhetosHostBuilder rhetosHostBuilder) {
    rhetosHostBuilder
        .ConfigureRhetosAppDefaults()
        .UseBuilderLogProviderFromHost(serviceProvider)
        .ConfigureConfiguration(cfg => cfg.MapNetCoreConfiguration(builder.Configuration));
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Rhetos service
builder.Services.AddRhetosHost(ConfigureRhetosHostBuilder)
    .AddAspNetCoreIdentityUser()
    .AddHostLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
