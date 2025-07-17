using GamesApi.Interfaces.Services;
using GamesApi.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddSingleton<ILogReaderService, FileLogReader>();
        builder.Services.AddSingleton<IGameParserService, GameParserService>();

        // Configuração correta das settings
        var settings = builder.Configuration.GetSection(LogFileSettings.SectionName)
                                          .Get<LogFileSettings>();

        if (settings?.IsValid() != true)
        {
            throw new ApplicationException("Invalid log file settings");
        }

        builder.Services.AddSingleton(settings);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}