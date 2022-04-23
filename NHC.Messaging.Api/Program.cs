using Microsoft.EntityFrameworkCore;
using NHC.Messaging.Application.Services;
using NHC.Messaging.Domain;
using Hangfire;
using Hangfire.SqlServer;
using NHC.Messaging.Application.Crud;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true)
            );
});
// Add services to the container.
builder.Services
    .AddHttpClient()
    .AddScoped<ICustomerService, CustomerService>()
    .AddScoped(typeof(ICrudService<,>), typeof(CrudService<,>))
    .AddScoped<IMessageService, MessageService>()
    .AddScoped<ICustomerMessageService, CustomerMessageService>();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Application")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(configuration => configuration
      .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
      .UseSimpleAssemblyNameTypeSerializer()
      .UseRecommendedSerializerSettings()
      .UseSqlServerStorage(builder.Configuration.GetConnectionString("Hangfire"), new SqlServerStorageOptions
      {
          CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
          SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
          QueuePollInterval = TimeSpan.Zero,
          UseRecommendedIsolationLevel = true,
          DisableGlobalLocks = true
      }));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

var app = builder.Build();
app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
