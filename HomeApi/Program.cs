using FluentValidation;
using HomeApi;
using HomeApi.Configuration;
using Scalar.AspNetCore;
using System.Reflection;
using FluentValidation.AspNetCore;
using HomeApi.Contracts.Validation;
using HomeApi.Data.Repos;
using HomeApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetAssembly(typeof(MappingProfile));

// Загрузка конфигурации из JSON файла
builder.Configuration.AddJsonFile("HomeOptions.json", optional: false, reloadOnChange: true);

// Регистрация настроек
builder.Services.Configure<HomeOptions>(builder.Configuration);
builder.Services.Configure<Address>(builder.Configuration.GetSection("Address"));

// Подключаем валидацию
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddDeviceRequestValidator>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Подключаем автомаппинг
builder.Services.AddAutoMapper(assembly);

// регистрация сервиса репозитория для взаимодействия с базой данных
builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>();
builder.Services.AddSingleton<IRoomRepository, RoomRepository>();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
            options
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient))
        .WithOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
