using DSProject.MonitoringAPI;
using DSProject.MonitoringAPI.Extensions;
using DSProject.MonitoringAPI.Model;
using DSProject.MonitoringAPI.Service;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "allowed_origins",
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:3000",
                                              "http://localhost:3000",
                                              "http://127.0.0.1:3000/")
                                                    .AllowAnyHeader()
                                                    .AllowAnyMethod();
                      });
});
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("MonitoringDb"));
});
// Add services to the container.
builder.Services.AddSingleton<SocketsManager>();
builder.Services.AddHostedService<ConsumeMeasurementService>();
builder.Services.AddHostedService<ConsumeDeviceService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.AddAppAuthetication();

builder.Services
  .AddOptions<RabbitMqConfiguration>()
  .Bind(builder.Configuration.GetSection(RabbitMqConfiguration.Key));

var app = builder.Build();
app.UseWebSockets();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("allowed_origins");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}