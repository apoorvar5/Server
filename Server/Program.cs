using CountryModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using FirebaseAdmin.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new()
    {
        Contact = new()
        {
            Email = "apoorvarumale@csun.edu",
            Name = "Apoorva Rumale",
            Url = new("https://canvas.csun.edu/courses/128137")
        },
        Description = "APIs for Player Club",
        Title = "Club Player APIs",
        Version = "V1"
    });
    OpenApiSecurityScheme jwtSecurityScheme = new()
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Please enter only JWT token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, [] }
    });
});

builder.Services.AddDbContext<PlayerSourceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ClubPlayerUser, IdentityRole>().AddEntityFrameworkStores<PlayerSourceContext>();

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("C:\\Users\\ACER\\source\\repos\\Server\\Server\\my-angular-app-f7e34-firebase-adminsdk-gq83z-d47a16db07.json"),
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false,
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            if (context.SecurityToken is not JwtSecurityToken jwtToken)
            {
                context.Fail("Unauthorized");
                return;
            }

            try
            {
                var firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(jwtToken.RawData);
                // Token is valid, you can access firebaseToken.Uid to get the user's ID
                // You can also access other user claims like email, etc. from firebaseToken
                // Perform any additional validation or processing here if needed
            }
            catch (FirebaseAuthException)
            {
                context.Fail("Unauthorized");
                return;
            }

            // If the token is successfully validated, no need to fail the authentication
            context.Success();
        },
        OnAuthenticationFailed = context =>
        {
            // Handle authentication failure
            Console.WriteLine(context.Exception);
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseMiddleware<FirebaseAuthenticationMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();