using Fanex.ComposeLetstalkMessage.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();
builder.Services.LoadConfig();
builder.Services.AddFanexServices(builder.Configuration);
builder.Services.AddSentryService();
builder.Services.AddCoreServices();
builder.Services.StartScheduler();

var app = builder.Build();
app.Map("/", () => "OK");
app.MapControllers();
app.MapRazorPages();
app.Run($"http://0.0.0.0:5000");