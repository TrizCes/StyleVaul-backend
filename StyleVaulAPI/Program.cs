using Microsoft.OpenApi.Models;
using StyleVaulAPI.Database;
using StyleVaulAPI.Database.Repositories;
using StyleVaulAPI.Interfaces.Repositories;
using StyleVaulAPI.Interfaces.Services;
using StyleVaulAPI.Mapper;
using StyleVaulAPI.Middlewares;
using StyleVaulAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        jsonOptions.JsonSerializerOptions.IncludeFields = false;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddSwaggerGen(s =>
{
    s.DescribeAllParametersInCamelCase();

    s.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "1.0.1",
            Title = "Style Vaul API",
            Description = string.Empty,
        }
    );

    s.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        }
    );

    s.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});
builder.Services.AddDbContext<StyleVaulDbContext>();

builder.Services.AddScoped<ICollectionsService, CollectionsService>();
builder.Services.AddScoped<ICompaniesService, CompaniesService>();
builder.Services.AddScoped<ICompaniesSetupService, CompaniesSetupService>();
builder.Services.AddScoped<IModelsService, ModelsService>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<ICollectionsRepository, CollectionsRepository>();
builder.Services.AddTransient<ICompaniesRepository, CompaniesRepository>();
builder.Services.AddTransient<ICompaniesSetupRepository, CompaniesSetupRepository>();
builder.Services.AddTransient<IModelsRepository, ModelsRepository>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();

builder.Services.AddAutoMapperConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("swagger/v1/swagger.json", "StyleVaulAPI");
    options.RoutePrefix = string.Empty;
});
app.UseCors("AllowAll");

app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ErrorsMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();