using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using WhereTo.Middleware;
using WhereTo;
using WhereToDataAccess;
using WhereToDataAccess.Interfaces;
using WhereToServices;
using WhereToServices.Interfaces;
using AutoMapper;
using Azure.Storage.Queues;
using System.Configuration;
using Microsoft.Extensions.Azure;
using WhereToServices.DTOs;
using Azure.Storage.Blobs;
using Azure.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IUserTourService, UserTourService>();
builder.Services.AddScoped<IQueueMessagePublisher<PayForTourDto>, WhereTo_BookingQueueMessagePublisherService>();
builder.Services.AddScoped<IQueueMessageSubscriber<BookingFinishedEvent>, WhereTo_BookingFinishedQueueSubscriberService>();
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddAutoMapper(typeof(Program), typeof(EmptyBlobForSasGenerationModel));

builder.Services.AddHostedService<TourBookingExpirationChecker>();
builder.Services.AddHostedService<WhereTo_BookingFinishedQueueSubscriber>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DataBase configuration
builder.Services.AddDbContext<WhereToDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnectionString"), b => b.MigrationsAssembly("WhereTo")));

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddAzureClients(b =>
{
    b.AddQueueServiceClient((builder.Configuration.GetConnectionString("AzureStorage")));
    b.AddBlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage"));
});

builder.Services.AddSingleton(sp =>
{
    return new StorageSharedKeyCredential(builder.Configuration["AzureStorageAccountKeys:StorageName"], builder.Configuration["AzureStorageAccountKeys:StorageKey"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }