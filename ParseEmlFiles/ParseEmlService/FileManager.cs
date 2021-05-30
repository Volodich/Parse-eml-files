using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParseEmlFiles
{
    public interface IFileManager
    {
        int CountFiles { get; }
        DirectoryInfo Directory { get; }
        public bool IsEmlFile(FileInfo file);
        public StreamWriter CreateTxtDocument(string name);
        public FileStream GetTxtFile(string name);
        public IEnumerable<FileInfo> GetFiles();
        public Task<IEnumerable<FileInfo>> GetFilesAsync();

    }
    public class FileManager : IFileManager
    {
        public DirectoryInfo Directory { get; }
        public FileManager(string puthDirecory)
        {
            Directory = new DirectoryInfo(puthDirecory);
            if (!Directory.Exists)
            {
                throw new DirectoryNotFoundException($"directory: {puthDirecory} not found");
            }
        }

        public int CountFiles => Directory.GetFiles().Length;

        public bool IsEmlFile(FileInfo file)
        {
            if (file.Extension == ".eml")
                return true;
            return false;
        }

        public StreamWriter CreateTxtDocument(string name)
        {
            return new StreamWriter(
                new FileStream(
                    Path.Combine(Environment.CurrentDirectory, $"{name}.txt"),
                    FileMode.OpenOrCreate,
                    FileAccess.Write));
        }

        public FileStream GetTxtFile(string name)
        {
            return new FileStream(
                Path.Combine(Environment.CurrentDirectory, $"{name}.txt"), 
                FileMode.OpenOrCreate,
                FileAccess.Read);
        }


        public IEnumerable<FileInfo> GetFiles()
        {
            return Directory.GetFiles().AsEnumerable();
        }

        public async Task<IEnumerable<FileInfo>> GetFilesAsync()
        {
            return await Task.Run(GetFiles);
        }
    }
}
