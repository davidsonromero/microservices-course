using AutoMapper;
using GeekShopping.ProductApi.Config;
using GeekShopping.ProductApi.Model.Context;
using GeekShopping.ProductApi.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//DB Connection
string connection = builder.Configuration["MySqlConnection:MySqlConnectionString"];

builder.Services.AddDbContext<MySqlContext>(options => {
    options.UseMySql(connection, 
        new MySqlServerVersion(new Version(8, 0, 33)));
});

//Mapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddControllers();

//Identity Server
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options => 
    {
        options.Authority = "https://localhost:4435/";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options => 
{
    options.AddPolicy("ApiScope", policy => 
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("geek_shopping");
    });
});

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.ProductAPI", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() { 
        Description = @"Enter 'Bearer' [space] and your token!",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        } 
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
