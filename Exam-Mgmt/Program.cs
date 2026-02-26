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
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });


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
            builder.Services.AddScoped<ILookupRepository, LookupRepository>();
            //<<<<<<< HEAD


            //=======
            builder.Services.AddScoped<CourseMasterService, CourseMasterService>();
//>>>>>>> origin/Vishwas
//>>>>>>> origin/Shreyash
//=======
            builder.Services.AddScoped<ICourseMasterService, CourseMasterService>();
            builder.Services.AddScoped<SemesterMasterService>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
            builder.Services.AddScoped<IResultRepository, ResultRepository>();


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
            app.UseCors("AllowAll");
            app.UseCors("AllowReactApp");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
