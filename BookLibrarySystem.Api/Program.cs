using BookLibrarySystem.Api.Extensions;
using BookLibrarySystem.Application;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Infrastructure;

using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);





builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "BookLibrarySystem API");
        
    });
}

app.UseHttpsRedirection();
app.UseCustomExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();