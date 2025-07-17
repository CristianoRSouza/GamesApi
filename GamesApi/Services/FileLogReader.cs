public class FileLogReader : ILogReaderService
{
    private readonly LogFileSettings _settings;

    public FileLogReader(LogFileSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public string ReadLogFile()
    {
        if (_settings == null)
            throw new InvalidOperationException("Settings não foram configuradas");

        if (string.IsNullOrWhiteSpace(_settings.FilePath))
            throw new ArgumentException("Caminho do arquivo de log não configurado");

        int attempt = 0;
        Exception lastException = null;

        while (attempt < _settings.MaxReadRetries)
        {
            try
            {
                attempt++;
                return File.ReadAllText(_settings.FilePath);
            }
            catch (FileNotFoundException ex)
            {
                lastException = ex;
                throw; 
            }
            catch (UnauthorizedAccessException ex)
            {
                lastException = ex;
                throw; 
            }
            catch (IOException ex) when (attempt < _settings.MaxReadRetries)
            {
                lastException = ex;

                var delay = CalculateRetryDelay(attempt);
                Thread.Sleep(delay);

                continue;
            }
            catch (Exception ex)
            {
                lastException = ex;
                throw; 
            }
        }

        throw new IOException($"Falha ao ler o arquivo após {_settings.MaxReadRetries} tentativas", lastException);
    }
    private TimeSpan CalculateRetryDelay(int attempt)
    {
        var random = new Random();
        double factor = (Math.Pow(2, attempt) + random.NextDouble());
        return TimeSpan.FromMilliseconds(Math.Min(1000 * factor, 5000));
    }

}