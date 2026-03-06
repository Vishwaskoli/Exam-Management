using Exam_Mgmt.DAL;
using Exam_Mgmt.Repositories;
using Exam_Mgmt.Services;
using Microsoft.OpenApi.Models;
//using Microsoft.OpenApi.Models;

namespace Exam_Mgmt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();
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

            builder.Services.AddScoped<SubjectMasterService>();
            builder.Services.AddScoped<SubjectSemMappingService>();
            builder.Services.AddScoped<UserRepository>();
            //<<<<<<< HEAD

            builder.Services.AddScoped<IExamMasterService, ExamMasterService>();
            builder.Services.AddScoped<Top3RankDAL>();

            //=======
            builder.Services.AddScoped<CourseMasterService, CourseMasterService>();
            //>>>>>>> origin/Vishwas
            //>>>>>>> origin/Shreyash
            //=======
            builder.Services.AddScoped<StudentRepository, StudentRepository>();
            builder.Services.AddScoped<ICourseMasterService, CourseMasterService>();
            //<<<<<<< HEAD
            //>>>>>>> origin/Shreyash
            //=======
            builder.Services.AddScoped<SemesterMasterService>();

            //>>>>>>> origin/chaitanya
            builder.Services.AddScoped<SemesterMasterService>();
            builder.Services.AddScoped<ICourseSemMappingService, CourseSemMappingService>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
            builder.Services.AddScoped<SubjectMasterService, SubjectMasterService>();
            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();      // This needs AddSwaggerGen()
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exam Management API v1");
                });
            }


            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
