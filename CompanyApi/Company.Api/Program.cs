using Company.Api.ApplicationServices;
using Company.Api.Commands;
using Company.Api.Data;
using Company.Api.DomainServices;
using Company.Api.Projections;
using Company.Api.Queries;
using Company.Api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Company API",
        Version = "v1",
        Description = "An API to manage companies with JWT authentication.",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token. Example: Bearer abc123"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// build DI registrations
builder.Services.AddScoped<IRetrieveCompanyApplicationService, RetrieveCompanyApplicationService>();
builder.Services.AddScoped<IRetrieveCompanyDomainService, RetrieveCompanyDomainService>();
builder.Services.AddScoped<ICompanyQuery, CompanyQuery>();
builder.Services.AddScoped<ICompanyProjection, CompanyProjection>();

builder.Services.AddScoped<ISaveCompanyApplicationService, SaveCompanyApplicationService>();
builder.Services.AddScoped<ICompanyDomainFactory, CompanyDomainFactory>();
builder.Services.AddScoped<ISaveCompanyCommand, SaveCompanyCommand>();

builder.Services.AddScoped<IUserAuthenticationApplicationService, UserAuthenticationApplicationService>();
builder.Services.AddScoped<IUserRegistrationDomainFactory, UserRegistrationDomainFactory>();
builder.Services.AddScoped<IRetrieveUserDomainService, RetrieveUserDomainService>();
builder.Services.AddScoped<ISaveUserCommand, SaveUserCommand>();

builder.Services.AddScoped<IUserLoginDomainFactory, UserLoginDomainFactory>();
builder.Services.AddScoped<IAuthenticationTokenDomainService, AuthenticationTokenDomainService>();

builder.Services.AddDbContext<CompanyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CompaniesDbConnection"));
});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Company API")
            .WithTheme(ScalarTheme.Saturn)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithHttpBearerAuthentication(new HttpBearerOptions());
    });
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();
