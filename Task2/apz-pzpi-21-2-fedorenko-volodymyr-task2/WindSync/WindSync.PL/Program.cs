using WindSync.PL.Configuration;

var builder = WebApplication.CreateBuilder(args);

Configuration.ConfigureServices(builder);
Configuration.ConfigureAuthentication(builder);
Configuration.ConfigureSwagger(builder);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
