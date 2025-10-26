
using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.MyLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().
    MinimumLevel.Information()
    .WriteTo.File("Log/log.txt",
    rollingInterval: RollingInterval.Minute)
    .CreateLogger();

//use this line to override the built-in loggers
//builder.Host.UseSerilog();

//Use serilog alogn with built-in loggers
builder.Logging.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<CollegeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"));
});
// Add services to the container.
builder.Services.AddControllers(
//options => options.ReturnHttpNotAcceptable = true
).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzkyMjgxNjAwIiwiaWF0IjoiMTc2MDgwNDcyNCIsImFjY291bnRfaWQiOiIwMTk5ZjgyMzk1NTk3MmJmOTlhYzlmZTZhYjQ0MzU1ZCIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazd3MjdxZXo4NTh3dGI1NjNwd25uaGE2Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.BzkVZQjblYS71y5GezmdWA9zeeRp4ahyTkeQPQiin9QrjP1ZrdRMpElwTjqvq01-PBzQGGoH94sjL4QHLK1uX0e9j2DEJ_2qGER2LC9M0AXiEv0_W2UnLQL0ZoYhMzbd63kRBb6-jJ0JMW9z4fSnv1l-yQMT6kirwtYLR2vlnJHq9odAV-GRvaNKg7W7VO8vPDCsyUfQgDECod0QoSEAxhNmIeANYLwzlz4FJW9VK9tHLStBTNXkXiMXuWjjzLFJE6Jhv8iyhp710p0zogOqYzzTS24LdtDRx_XABJP31QPM2mXpfPV7Tq7ilgYxq63nBNaS4agl60arAs5RmDQdMg", typeof(Program));
builder.Services.AddTransient<IMyLogger, LogToServerMemory>();
builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy(name: "MyTestCORS",

        policy =>
        {
            policy.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyTestCORS");
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("api/testingendpoint",
            context => context.Response.WriteAsync("Test Response"))
        .RequireCors("AllowOnlyLocalhost");

    endpoints.MapControllers()
        .RequireCors("AllowAll");

    endpoints.MapGet("api/testendpoint2",
        context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecret")));

});

app.Run();
