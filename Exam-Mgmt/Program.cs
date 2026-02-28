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
            builder.Services.AddScoped<Top3RankDAL>();

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

//>>>>>>> origin/chaitanya
       

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
>>>>>>>>> Temporary merge branch 2
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
//<<<<<<< HEAD
            builder.Services.AddSwaggerGen();
//<<<<<<< HEAD
            builder.Services.AddScoped<ICourseSemMappingService, CourseSemMappingService>();


//<<<<<<< HEAD
//>>>>>>> origin/Shreyash
//=======
            builder.Services.AddScoped<SemesterMasterService>(); 
            builder.Services.AddScoped<SemesterMasterService>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
            //Subject_Master
//>>>>>>> origin/chaitanya

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy",
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
<<<<<<<<< Temporary merge branch 1
            builder.Services.AddScoped<SemesterMasterService>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
            builder.Services.AddScoped<SubjectMasterService>();
=========
//>>>>>>> origin/Shreyash
>>>>>>>>> Temporary merge branch 2

            builder.Services.AddScoped<IExamMasterService, ExamMasterService>();

            //=======
            builder.Services.AddScoped<CourseMasterService, CourseMasterService>();
//>>>>>>> origin/Vishwas
//>>>>>>> origin/Shreyash
//=======
            builder.Services.AddScoped<ICourseMasterService, CourseMasterService>();
<<<<<<<<< Temporary merge branch 1
            builder.Services.AddScoped<SemesterMasterService>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();

=========
//>>>>>>> origin/Shreyash
>>>>>>>>> Temporary merge branch 2

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

            app.UseCors("ReactPolicy");

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseCors("AllowReactApp");
            app.UseCors("ReactPolicy");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
