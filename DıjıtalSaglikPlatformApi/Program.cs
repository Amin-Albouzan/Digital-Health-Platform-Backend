using System.Text.Json.Serialization;
using DijitalSaglikPlatformu.Data;
using DijitalSaglikPlatformu.Extentions;
using DijitalSaglikPlatformu.Models;
using DijitalSaglikPlatformu.Repo.Account;
using DijitalSaglikPlatformu.Repo.DoctorProfileRepositories;
using DijitalSaglikPlatformu.Repo.DoctorWeeklyScheduleRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using DijitalSaglikPlatformu.Repo.BookedAppointmentRepositories;
using DijitalSaglikPlatformu.Repo.AvailabilityRepositories;



var builder = WebApplication.CreateBuilder(args);

// database connection
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer
(builder.Configuration.GetConnectionString("mycon")));

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

// Repo
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IDoctorProfileRepo, DoctorProfileRepo>();
builder.Services.AddScoped<IDoctorWeeklyScheduleRepo, DoctorWeeklyScheduleRepo>();
builder.Services.AddScoped<IBookedAppointmentRepo, BookedAppointmentRepo>();
builder.Services.AddScoped<IAvailabilityRepo, AvailabilityRepo>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });



// Extentions 
builder.Services.AddSwaggerGenJwtAuth();
builder.Services.AddCustomJwtAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (!await roleManager.RoleExistsAsync("Doctor"))
        await roleManager.CreateAsync(new IdentityRole("Doctor"));

    if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));
}



app.Run();
