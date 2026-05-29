using CvForgeAI.API.Middleware;
using CvForgeAI.Application.Abstractions.Repositories;
using CvForgeAI.Application.Services.AI;
using CvForgeAI.Application.Services.Auth;
using CvForgeAI.Application.Services.Certificates;
using CvForgeAI.Application.Services.Education;
using CvForgeAI.Application.Services.Experience;
using CvForgeAI.Application.Services.Pdf;
using CvForgeAI.Application.Services.Projects;
using CvForgeAI.Application.Services.Resume;
using CvForgeAI.Application.Services.Skills;
using CvForgeAI.Infrastructure.Persistence;
using CvForgeAI.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IResumeService, ResumeService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IEducationService, EducationService>();

builder.Services.AddScoped<ISkillRepository, SkillRepository>();

builder.Services.AddScoped<ISkillService, SkillService>();

builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();

builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddHttpClient<IAIService, AIService>(
    client =>
    {
        client.BaseAddress =
            new Uri("https://openrouter.ai/api/v1/");

        client.DefaultRequestHeaders.Add(
            "Authorization",
            $"Bearer {builder.Configuration["OpenRouter:ApiKey"]}");
    });




builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();