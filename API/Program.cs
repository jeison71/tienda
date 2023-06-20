using API.Extensions;
using API.Helpers.Errors;

using AspNetCoreRateLimit;

using Infraestructure.Data;

using Microsoft.EntityFrameworkCore;

using Serilog;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
			.ReadFrom.Configuration(builder.Configuration)
			.Enrich.FromLogContext()
			.CreateLogger();

//builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//servicio automapper
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

builder.Services.ConfigureRateLimitiong();
// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();
builder.Services.ConfigureApiVersioning();
builder.Services.AddJwt(builder.Configuration);

builder.Services.AddControllers(options =>
{
	options.RespectBrowserAcceptHeader = true;
	options.ReturnHttpNotAcceptable = true; //Da respuesta al cliente en caso de que el servidor no  acepte el formato que este pide
}).AddXmlSerializerFormatters();


builder.Services.AddValidationErrors();

builder.Services.AddDbContext<TiendaContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); //permite manejar la excepciones de forma global
app.UseStatusCodePagesWithReExecute("/errors/{0}"); //componente middleware que viene integrado .netCore
													//errors es un controlador personalizado que se encarga de enviar el mensaje de error

app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using ( var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
	try
	{
		var context = services.GetRequiredService<TiendaContext>();
		await context.Database.MigrateAsync();
		await TiendaContextSeed.SeedAsync(context, loggerFactory);
		await TiendaContextSeed.SeedRolesAsync(context, loggerFactory);	
	}
	catch (Exception ex)
	{
		var _logger = loggerFactory.CreateLogger<Program>();
		_logger.LogError(ex, "Ocurrió un error durante la migración");
		throw;
	}
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
