using ApI.Data;
using ApI.Managers;
using ApI.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.AddSecurityDefinition(ApiKeyAuthenticationOptions.DefaultScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = ApiKeyAuthenticationOptions.HeaderName,
        Type = SecuritySchemeType.ApiKey
    });

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = ApiKeyAuthenticationOptions.DefaultScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddSingleton<ISessionManager,SessionManager>();

builder.Services.AddScoped<ApiKeyAuthenticationHandler>();

builder.Services.AddDbContext<ParkingDbContext>(options => options.UseInMemoryDatabase("parkbee"),contextLifetime: ServiceLifetime.Singleton, optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme,
        null);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

AddSeedData(app);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static void AddSeedData(WebApplication app)
{
    var parkingDbContext = app.Services.GetService<ParkingDbContext>();
    var guidList = Enumerable.Range(0, 40).Select(x => Guid.NewGuid()).ToList();
    int i = 0;
    parkingDbContext.Garages.AddRange(Enumerable.Range(0, 10).Select(x => new Garage
    {
        Id = guidList[i],
        Name = $"Garage {guidList[i++]}",
        Capacity = 10,
        Doors = new List<Door>()
        {
            new Door
            {
                Id = guidList[i],
                Description = $"Door {guidList[i++]}",
                DoorType = DoorType.Entry,
                IP = "127.0.0.1"
            },
            new Door
            {
                Id = guidList[i],
                Description = $"Door {guidList[i++]}",
                DoorType = DoorType.Exit,
                IP = "127.0.0.1"
            },
            new Door
            {
                Id = guidList[i],
                Description = $"Door {guidList[i++]}",
                DoorType = DoorType.Pedestrian,
                IP = "127.0.0.1"
            }
        },
        ActiveSessions = new List<Session>()
    }));
    parkingDbContext.Garages.Add(new Garage
    {
        Id = Guid.NewGuid(),
        Name = "Bad Garage",
        Capacity = 1,
        Doors = new List<Door>()
        {
            new Door
            {
                Id = Guid.NewGuid(),
                Description = $"Bad Entry Door",
                DoorType = DoorType.Entry,
                IP = "192.0.2.1"
            },
            new Door
            {
                Id = Guid.NewGuid(),
                Description = "Bad Exit Door",
                DoorType = DoorType.Exit,
                IP = "unreachablehost"
            },
            new Door
            {
                Id = Guid.NewGuid(),
                Description = "Bad Pedestrian Door",
                DoorType = DoorType.Pedestrian,
                IP = "unreachablehost"
            }
        },
        ActiveSessions = new List<Session>()
    });
    parkingDbContext.Users.AddRange(Enumerable.Range(0, 20).Select(x => new User()
    {
        Id = Guid.NewGuid(),
        PartnerId = "partner-1"
    }));

    parkingDbContext.SaveChanges();
}