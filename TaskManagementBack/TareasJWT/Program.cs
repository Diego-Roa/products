using TaskManagement.DataAccess;
using TaskManagement.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TaskManagement.Services.Services;
using TaskManagement;
using TaskManagement.Services.Mapping;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configurar Identity
builder.Services.AddIdentity<AplicationUserEntity, AplicationRoleEntity>(options =>
{
    // Configura las opciones de contrase�a seg�n tus necesidades
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configurar IdentityServer
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential() // Usa certificados de desarrollo. En producci�n, usa certificados v�lidos.
    .AddAspNetIdentity<AplicationUserEntity>()
    .AddInMemoryIdentityResources(TaskManagement.Config.IdentityResources)
    .AddInMemoryApiScopes(TaskManagement.Config.ApiScopes)
    .AddInMemoryClients(TaskManagement.Config.Clients);

//Configuracion Token
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // Opcional: elimina la tolerancia de tiempo
    };
});

// Agregar servicios
builder.Services.AddTransient<LoginService>();
builder.Services.AddTransient<PermissionService>();
builder.Services.AddTransient<TaskService>();
builder.Services.AddTransient<ProductsService>();
builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddScoped(typeof(UnitOfWork));

// Agregar controladores
builder.Services.AddControllers();

//Agregar Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Agregar swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS si es necesario
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurar el pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors("DefaultCorsPolicy");

app.UseRouting();

// Usar IdentityServer
app.UseIdentityServer();

// Usar autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await MyIdentityDataInitializer.SeedData(services);
    }
    catch (Exception ex)
    {
        // Manejo de excepciones si es necesario
        Console.WriteLine($"Error seeding data: {ex.Message}");
    }
}

app.Run();
