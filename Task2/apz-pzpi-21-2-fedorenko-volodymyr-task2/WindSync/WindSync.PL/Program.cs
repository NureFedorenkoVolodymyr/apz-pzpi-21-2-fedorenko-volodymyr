using WindSync.PL.Configuration;
using WindSync.PL.Middleware;

var builder = WebApplication.CreateBuilder(args);

Configuration.ConfigureServices(builder);
Configuration.ConfigureAuthentication(builder);
Configuration.ConfigureSwagger(builder);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WindSync API v1");
});

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<UserIdMiddleware>();

app.Run();
