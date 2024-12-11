using VTS.Core;
using VTSParamStepper.Application;
using VTSParamStepper.Web;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args); // Create a host builder so the program doesn't exit immediately

ConsoleVTSLoggerImpl logger = new(); // Create a logger to log messages to the console (you can use your own logger implementation here like in the Advanced example)
CoreVTSPlugin plugin = new(logger, 20, "VTSParamStepper", "Zopo", "");

builder.Services.AddSingleton<IVTSPlugin>(plugin);
builder.Services.AddSingleton<IVTSParamStore, VTSParamStore>();
builder.Services.AddSingleton<IVTSTriggerStore, VTSTriggerStore>();
builder.Services.AddHostedService<TriggerExecutionService>();
builder.Services.AddHostedService<VTSValueTransmitter>();
builder.Services.AddHostedService<TestSetup>();

try 
{
    await plugin.InitializeAsync(
        new WebSocketImpl(logger), 
        new NewtonsoftJsonUtilityImpl(), 
        new TokenStorageImpl(""), 
        () => logger.LogWarning("Disconnected!"));
    
    logger.Log("Connected!");
    var apiState = await plugin.GetAPIState();
    
    logger.Log("Using VTubeStudio " + apiState.data.vTubeStudioVersion);
    var currentModel = await plugin.GetCurrentModel();
    
    logger.Log("The current model is: " + currentModel.data.modelName);
    
    // To unsubscribe, use the plugin.UnsubscribeFrom* methods
} 
catch (VTSException error) {
    logger.LogError(error); // Log any errors that occur during initialization
}

var host = builder.Build(); // Build the host
await host.RunAsync(); // This will keep the program running until the user presses Ctrl+C, or kills the process