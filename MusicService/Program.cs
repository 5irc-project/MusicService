using Microsoft.EntityFrameworkCore;
using MusicService.Models;
using MusicService.Helpers;
using MusicService.Services.Interfaces;
using MusicService.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddDbContext<MusicServiceDBContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MusicServiceDBContext")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IKindService, KindService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

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

// Test test = new Test();
// test.TestStreamReader();

app.Run();
