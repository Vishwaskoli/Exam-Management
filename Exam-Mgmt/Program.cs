using Exam_Mgmt.Repositories;
using Exam_Mgmt.Services;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

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
                options.AddPolicy("ReactPolicy", policy => { policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
//<<<<<<< HEAD
            builder.Services.AddSwaggerGen();
//<<<<<<< HEAD
            builder.Services.AddScoped<ICourseSemMappingService, CourseSemMappingService>();


            builder.Services.AddScoped<ISemesterRepository,SemesterRepository>();
            //=======
            //HEAD
            //Subject_Master
            //=======

            // ? VERY IMPORTANT
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Exam Management API",
                    Version = "v1"
                });
            });

            // Register services
//>>>>>>> origin/Shreyash
            builder.Services.AddScoped<SubjectMasterService>();
            builder.Services.AddScoped<SubjectSemMappingService>();
            //<<<<<<< HEAD


            //=======
            //builder.Services.AddScoped<CourseMasterService, CourseMasterService>();
            //>>>>>>> origin/Vishwas
            //>>>>>>> origin/Shreyash
            //=======   
            builder.Services.AddScoped<StudentRepository,StudentRepository>();
            builder.Services.AddScoped<ICourseMasterService, CourseMasterService>();
            builder.Services.AddScoped<SubjectMasterService, SubjectMasterService>();
//>>>>>>> origin/Shreyash

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
            app.UseCors("ReactPolicy");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
