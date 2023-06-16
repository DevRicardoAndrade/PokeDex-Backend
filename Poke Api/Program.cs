using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Poke_Api.Context;
using Poke_Api.Repositories.User;
using Poke_Api.Services;
using Poke_Api.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Add connection string to EF
string? sqlServerConnection = builder.Configuration.GetConnectionString("stringConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(sqlServerConnection));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Poke Api", Version = "v1" });

}
    );

//Depency Injection
builder.Services.AddScoped<IUser, User>();
builder.Services.AddSingleton<AuthenticationService>();

var app = builder.Build();

ENV.Initialize(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(option => option.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapControllers();

app.Run();
