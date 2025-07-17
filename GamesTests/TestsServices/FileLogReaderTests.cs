namespace GamesTests.TestsServices
{
    public class FileLogReaderTests
    {
        private readonly string _tempFilePath;

        public FileLogReaderTests()
        {
            _tempFilePath = Path.GetTempFileName();
        }

        [Fact]
        public void ReadLogFile_ReturnsContent_WhenFileExists()
        {
            var content = "log content";
            File.WriteAllText(_tempFilePath, content);

            var settings = new LogFileSettings { FilePath = _tempFilePath, MaxReadRetries = 3 };
            var reader = new FileLogReader(settings);

            var result = reader.ReadLogFile();

            Assert.Equal(content, result);
        }

        [Fact]
        public void ReadLogFile_Throws_WhenFileNotFound()
        {
            var settings = new LogFileSettings { FilePath = "invalid.txt", MaxReadRetries = 1 };
            var reader = new FileLogReader(settings);

            Assert.Throws<FileNotFoundException>(() => reader.ReadLogFile());
        }

        [Fact]
        public void ReadLogFile_Throws_WhenUnauthorized()
        {
            var mockSettings = new LogFileSettings { FilePath = _tempFilePath, MaxReadRetries = 1 };
            var reader = new FileLogReader(mockSettings);

            File.SetAttributes(_tempFilePath, FileAttributes.ReadOnly);
            File.Open(_tempFilePath, FileMode.Open, FileAccess.Read, FileShare.None).Close();

            Assert.Throws<UnauthorizedAccessException>(() =>
            {
                File.SetAttributes(_tempFilePath, FileAttributes.ReadOnly);
                File.WriteAllText(_tempFilePath, "");
            });
        }
    }
}
