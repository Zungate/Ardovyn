using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var config = ConfigurationLoader.LoadInfrastructureConfiguration();
builder.Services.AddOptions(config);

var auth0Options = config.GetSection("Auth0Config").Get<Auth0Config>();

var auth0Authority = auth0Options.Authority;
var auth0Audience = auth0Options.Audience;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = auth0Authority;
    options.Audience = auth0Audience;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "sub"
    };
});

//ConnectionString
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAuthorization();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
