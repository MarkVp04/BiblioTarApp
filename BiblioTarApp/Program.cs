using BiblioTarApp.DataContext.Context;
using BiblioTarApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AutoMapper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


string connectionString = "Server=.\\SQLEXPRESS;Database=KonyvtarDB;User Id=sa;Password=csapat15;TrustServerCertificate=True;";

//builder.Services.AddDbContext<AppDbContext>(options =>
//   options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BiblioTarContext")));



builder.Services.AddScoped<IKonyvServices, KonyvService>();
builder.Services.AddScoped<ILakcimService, LakcimService>();
builder.Services.AddScoped<IFoglalasService, FoglalasService>();
builder.Services.AddScoped<IKolcsonzesService, KolcsonzesService>();
builder.Services.AddScoped<IBuntetesService, BuntetesService>();
builder.Services.AddScoped<IFelhasznaloService, FelhasznaloService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


//Swegger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo{ Title = "BiblioTarApp API", Version = "v1" }); //teszt
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BiblioTarApp API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
