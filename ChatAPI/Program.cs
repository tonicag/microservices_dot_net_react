using DSProject.ChatAPI.Service;
using DSProject.ChatAPI.Data;
using Microsoft.EntityFrameworkCore;
using DSProject.ChatAPI.Extensions;
using DSProject.ChatAPI.Services.IService;
using DSProject.ChatAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
    option.UseNpgsql(builder.Configuration.GetConnectionString("ChatDb"));
    option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});



builder.AddAppAuthetication();
builder.Services.AddSingleton<SocketsManager>();
builder.Services.AddScoped<IChatService, ChatService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseWebSockets();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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