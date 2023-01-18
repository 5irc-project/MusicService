using Microsoft.EntityFrameworkCore;
using MusicService.Models;
using MusicService.Helpers;
using MusicService.Services.Interfaces;
using MusicService.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MassTransit;
using System.Reflection;
using MusicService.HttpClient;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Setup RabbitMQ
builder.Services.AddMassTransit(x => {
    var entryAssembly = Assembly.GetExecutingAssembly();
    x.AddConsumers(entryAssembly);
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:host"], "/", h => { 
            h.Username("root");
            h.Password("root");
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<MusicServiceDBContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MusicServiceDBContext")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IKindService, KindService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IMLHttpClient, MLHttpClient>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

builder.Services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = JwtTokenValidator.CreateTokenValidationParameters();
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();