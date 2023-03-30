using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sport_shop_api.Data;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    {

        string connectionString;
        if (builder.Configuration["Type"] == "local")
        {
            connectionString = builder.Configuration.GetConnectionString("DbLocal");
            options.UseSqlServer(connectionString);
        }
        else
        {
            connectionString = builder.Configuration.GetConnectionString("DbContext");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Identity:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

// for demotration
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(builder.Configuration, context);
}


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseStaticFiles();
app.UseRouting();

//("https://sport.phuongtran.xyz",
//               "https://localhost:3000", "https://vtvy.tk/link"
//               , "http://localhost:3001")

app.UseCors(options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());
app.UseAuthorization();

app.MapControllers();


app.MapGet("/", () => "Hello World!");

app.Run();
