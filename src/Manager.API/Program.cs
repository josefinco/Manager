using AutoMapper;
using EscNet.IoC.Cryptography;
using Manager.API.Token;
using Manager.API.ViewModels;
using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Swagger
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Manager User API�",
    Version = "v1",
    Description = "Api para ger�ncia de usu�rios",
    TermsOfService = new Uri("https://example.com/terms"),
    Contact = new OpenApiContact
    {
      Name = "José Finco",
      Email = "josefinco_@hotmail.com",
      Url = new Uri("https://josefinco.github.io/index.html"),
    }
  });
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    In = ParameterLocation.Header,
    Description = "Por favor informe o token JWT",
    Name = "Authorization",
    BearerFormat = "JWT",
    Scheme = "Bearer",
    Type = SecuritySchemeType.ApiKey,
  });
  options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});
#endregion

#region AutoMapper

var autoMapperConfig = new MapperConfiguration(cfg =>
{
  cfg.CreateMap<User, UserDTO>().ReverseMap();
  cfg.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
  cfg.CreateMap<UpdateUserViewModel, UserDTO>().ReverseMap();
});
#endregion

#region JWT
var secretKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(x =>
{
  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
  x.RequireHttpsMetadata = false;
  x.SaveToken = true;
  x.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
    ValidateIssuer = false,
    ValidateAudience = false
  };
});
#endregion

#region DI

builder.Services.AddSingleton(autoMapperConfig.CreateMapper());
builder.Services.AddDbContext<ManagerContext>(options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddRijndaelCryptography(builder.Configuration["Cryptography:Key"]);

#endregion


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
