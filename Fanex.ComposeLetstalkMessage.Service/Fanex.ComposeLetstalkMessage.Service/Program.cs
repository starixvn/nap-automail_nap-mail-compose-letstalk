using Fanex.ComposeLetstalkMessage.Service;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using ONEbook.UM.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddRazorPages();
services.LoadConfig();
services.AddFanexLogSentry(configuration);
services.AddFanexSql(configuration);
services.AddOneBookUmForService(configuration);
services.AddInternalServices();
services.StartScheduler();

var app = builder.Build();
app.Map("/", () => "OK");

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.MapControllers();

await app.RunAsync();