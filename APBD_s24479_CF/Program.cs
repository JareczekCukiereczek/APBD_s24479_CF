
using APBD_s24479_CF.Context;
using APBD_s24479_CF.Service;
using Microsoft.EntityFrameworkCore;

namespace ApbdEfCodeFirst
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ContextEF>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IPrescriptionsService, PrescriptionsService>();//dodawanko
            builder.Services.AddScoped<IPatientsService, PatientsService>();//dodawanko
            

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}