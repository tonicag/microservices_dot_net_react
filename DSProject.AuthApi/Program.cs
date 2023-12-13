using UsersAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UsersAPI.Models;
using UsersAPI.Service.IService;
using UsersAPI.Service;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DSProject.AuthAPI.Extensions;
using DSProject.AuthAPI.Utility;
using DSProject.AuthAPI.Service.IService;
using DSProject.AuthAPI.Service;
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
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("UsersDb"));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();


builder.Services.AddScoped<IDeviceService, DeviceService>();

builder.Services.AddHttpClient("Devices", u => u.BaseAddress =
new Uri(builder.Configuration["ServiceUrls:DevicesAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers();

builder.AddAppAuthetication();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("allowed_origins");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
AddDefaultUser();
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

async void AddDefaultUser()
{
    using (var scope = app.Services.CreateScope())
    {
        var manager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var user = new ApplicationUser { Email = "admin@admin.com", UserName = "admin@admin.com", Name = "admin@admin.com" };
        var result = await manager.CreateAsync(user, "Admin!123");
        if (!_roleManager.RoleExistsAsync("ADMIN").GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole("ADMIN")).GetAwaiter().GetResult();
        }
        await manager.AddToRoleAsync(user, "ADMIN");
    }
}