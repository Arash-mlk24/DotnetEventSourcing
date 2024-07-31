using DotnetEventSourcing.src.Core.Utilities;
using DotnetEventSourcing.src.Web.Utilities.Extensions;

var ApplicationCorsPolicyName = "_applicationCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationCors(ApplicationCorsPolicyName);
builder.Services.AddApplicationApis();
builder.Services.AddApplicationContexts(builder.Configuration);
builder.Services.AddApplicationSwagger();
builder.Services.AddApplicationMediatR();
builder.Services.RegisterMapster();

var app = builder.Build();

app.UseCors(ApplicationCorsPolicyName);
app.UseHttpsRedirection();
app.MapControllers();
app.UseApplicationSwagger();
app.ApplyDatabaseMigrate();

app.Run();