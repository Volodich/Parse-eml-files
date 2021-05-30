using System.IO;
using System.Threading.Tasks;

namespace ParseEmlFiles
{
    public class ParseEmlService
    {
        private IFileManager _fileManager;
        private IParserManager _parserManager;

        private readonly string _fileResultName = "result";
        private readonly string _fileLogName = "emailNotFound";

        public ParseEmlService(string directory)
        {
            _fileManager = new FileManager(directory);
            _parserManager = new ParserManager(_fileManager.Directory.FullName);
        }

        public async Task StartParseAsync()
        {
            var files = await _fileManager.GetFilesAsync();

            await using var resultFile = _fileManager.CreateTxtDocument(_fileResultName);
            await using var logFile = _fileManager.CreateTxtDocument(_fileLogName);

            foreach (var file in files)
            {
                if (_fileManager.IsEmlFile(file))
                {
                    try
                    {
                        string body = _parserManager.GetBodyEmail(file.FullName);
                        string email = await _parserManager.GetEmailAsync(body);

                        await resultFile.WriteLineAsync(email);
                    }
                    catch (EmailNotFoundException)
                    {

                        await logFile.WriteLineAsync(file.Name);
                    }
                }
            }
        }

        public StreamReader GetFileResult()
        {
            return  new StreamReader(_fileManager.GetTxtFile(_fileResultName));
        }

        public StreamReader GetFileLog()
        {
            return new StreamReader(_fileManager.GetTxtFile(_fileLogName));
        }

    }
}
