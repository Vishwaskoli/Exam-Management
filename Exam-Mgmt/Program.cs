using Exam_Mgmt.DAL;
using Exam_Mgmt.Repositories;
using Exam_Mgmt.Services;
using Microsoft.OpenApi.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Exam_Mgmt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddAuthorization();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"])
                        )
                };
            });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });

                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Exam Management API",
                    Version = "v1"
                });
            });

            // Services
            builder.Services.AddScoped<SubjectMasterService>();
            builder.Services.AddScoped<SubjectSemMappingService>();
            builder.Services.AddScoped<UserRepository>();
            //<<<<<<< HEAD

            builder.Services.AddScoped<IExamMasterService, ExamMasterService>();
            builder.Services.AddScoped<Top3RankDAL>();
            builder.Services.AddScoped<CourseMasterService>();
            builder.Services.AddScoped<StudentRepository>();
            builder.Services.AddScoped<ICourseMasterService, CourseMasterService>();
            builder.Services.AddScoped<SemesterMasterService>();
            builder.Services.AddScoped<ICourseSemMappingService, CourseSemMappingService>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthentication();   // IMPORTANT
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}