using ChatMessageServer;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            // .WithOrigins("http://localhost:5173")
            .SetIsOriginAllowed(origin =>
                new Uri(origin).Host.Equals(
                    "localhost", StringComparison.OrdinalIgnoreCase))
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

app.UseCors("CorsPolicy");

app.MapHub<ChatHub>("/chathub");
app.MapGet("/emit", async (IHubContext<ChatHub> hubContext) => 
{
    await hubContext.Clients.All.SendAsync("ReceiveMessage", "John Doe", "Hello, World!");
    return "ChatHub is running. Connect to /chathub to send messages.";
});

app.Run();
