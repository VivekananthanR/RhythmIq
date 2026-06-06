using elr_API.IService.ICommon_manager;
using elr_API.IService.IDapper;
using elr_API.IService.IEntrolement;
using elr_API.IService.IToken;
using elr_API.IService.IUser_manager;
using elr_API.Service.Common_manager;
using elr_API.Service.Dapper;
using elr_API.Service.Enrolment;
using elr_API.Service.Exception_handler;
using elr_API.Service.Token;
using elr_API.Service.User_manager;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

#region CORS configuration
builder.Services.AddCors(op =>
{
    op.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
#endregion

#region Service configuration
builder.Services.AddScoped<IDapperSqlService, DapperSqlService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHelperService, HelperService>();
builder.Services.AddScoped<IEnrollment, Enrollment>();
builder.Services.AddScoped<IQikberrychatService, QikberrychatService>();
#endregion

#region Logging configuration

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
//builder.Host.UseSerilog();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//app.UseSerilogRequestLogging();
app.UseCors("AllowAll");
app.UseMiddleware<CustomErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
