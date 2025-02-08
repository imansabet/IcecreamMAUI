using IcecreamMAUI.Api.Data;
using IcecreamMAUI.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var connectionString = builder.Configuration.GetConnectionString("Icecream");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));


// why transient ? suitable for ligh service (stateless) : new instance for every request (not Good for Db operations)
builder.Services.AddTransient<TokenService>()
                .AddTransient<PasswordService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    jwtOptions => jwtOptions.TokenValidationParameters = TokenService.GetTokenValidationParameters(builder.Configuration));


builder.Services.AddAuthentication();


var app = builder.Build();

#if DEBUG
MigrateDatabase(app.Services);
#endif


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();




app.Run();


static void MigrateDatabase(IServiceProvider sp)
{
    var scope = sp.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    if (dataContext.Database.GetPendingMigrations().Any())
        dataContext.Database.Migrate();
}

