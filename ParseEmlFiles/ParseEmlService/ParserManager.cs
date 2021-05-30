using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using glib.Email;

namespace ParseEmlFiles
{
    public interface IParserManager
    {
        string GetBodyEmail(string pathToFile);
        Task<string> GetBodyTextEmailAsync(string pathToFile);
        string GetEmail(string bodyText);
        Task<string> GetEmailAsync(string pathToFile);

    }
    public class ParserManager : IParserManager
    {
        private readonly string _emailPattern = @"[A-Za-z0-9_\-\+]+@[A-Za-z0-9\-]+\.([A-Za-z]{2,3})(?:\.[a-z]{2})?";
        private string _directorePath;
        private MimeReader _mimeReader;
        private RxMailMessage _rxMailMessage;
        private Regex _regex;
        public ParserManager(string directoryPath)
        {
            _directorePath = directoryPath;

            _mimeReader = new MimeReader();
            _rxMailMessage = new RxMailMessage();

            _regex = new Regex(pattern: _emailPattern);
        }

        public string GetBodyEmail(string pathToFile)
        {
            var mailMessage = _mimeReader.GetEmail(pathToFile);
            string result = string.Empty;

            if (mailMessage.Subject.Contains("spam")) 
                // if subject in email contains email
            {
                result += mailMessage.Subject;
            }

            if (!mailMessage.Entities.Any()) 
                // catch exception System.InvalidOperationException: 'Sequence contains no elements'
            {
                return result + mailMessage.Body;
            }

            if (!string.IsNullOrEmpty(mailMessage.Entities.First().Body))
            {
                return result + mailMessage.Entities.First().Body;
            }

            if (mailMessage.TopParent is not null)
            {
                var parentMailMessageList = mailMessage.TopParent.Entities;
                foreach (var rxMailMessage in parentMailMessageList)
                {
                    if (!string.IsNullOrEmpty(rxMailMessage.Body))
                    {
                        return result + rxMailMessage.Body;
                    }
                }
            }

            throw new EmailNotFoundException($"email not fount from file!");
        }

        public async Task<string> GetBodyTextEmailAsync(string puthToFile)
        {
            return await Task.Run(() => GetBodyEmail(puthToFile));
        }

        public string GetEmail(string bodyText)
        {
            var result = _regex.Match(bodyText).Value;

            if(!string.IsNullOrEmpty(result))
            {
                return result;
            }

            throw new EmailNotFoundException("email not fount from file!");
        }

        public async Task<string> GetEmailAsync(string pathToFile)
        {
            return await Task.Run(() => GetEmail(pathToFile));
        }
    }
}
