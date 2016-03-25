using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace dotnet.common.files
{
    public static class FilesExtensions
    {

        public enum SizeUnits
        {
            Byte, KB, MB, GB, TB, PB, EB, ZB, YB
        }

        public static string ToFileSize(this Int64 value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (Int64)unit)).ToString("0.00") + " " + unit.ToString();
        }

        public static string ToFileSize(this Int32 value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (Int32)unit)).ToString("0.00") + " " + unit.ToString();
        }

        public static string ToFileSize(this byte[] bytes, SizeUnits unit)
        {
            if(bytes==null || bytes.Length==0)
                return string.Format("0 {0}",unit);
            return bytes.Length.ToFileSize(unit);
        }

        public static string EnsureLastSlashInWindowsPath(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value.EndsWith("\\"))
                return value;
            else
            {
                return value + "\\";
            }
        }

        public static IList<string> GetFilesInFolderWithExtension(this string path, string fileExtension,
            bool includeSubDirectories = false, TimeSpan? onlyFilesOlderThan = null)
        {
            if (!fileExtension.StartsWith("."))
                fileExtension = "." + fileExtension;

            return path.GetFilesInFolderAsPaths(string.Format("*{0}", fileExtension), includeSubDirectories,
                onlyFilesOlderThan);
        }

        public static IList<string> GetFilesInFolderAsPaths(this string path,string pattern="*.*",bool includeSubDirectories=false,TimeSpan? onlyFilesOlderThan=null)
        {
            if(!Directory.Exists(path))
                return new List<string>();

            return
                path.GetFilesInFolder(pattern, includeSubDirectories, onlyFilesOlderThan)
                    .Select(x => x.FullName)
                    .ToList();
        }

        public static IList<FileInfo> GetFilesInFolder(this string path, string pattern = "*.*", bool includeSubDirectories = false, TimeSpan? onlyFilesOlderThan = null)
        {
            if (!Directory.Exists(path))
                return new List<FileInfo>();

            return Directory.GetFiles(path, pattern,
                includeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Select(f => new FileInfo(f))
                .Where(f => onlyFilesOlderThan == null || (DateTime.Now - f.CreationTime) > onlyFilesOlderThan.Value)
                .ToList();
        }

        public static bool MatchFileContent(this string filePath, string otherFilePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException("filePath is null or empty");

            if (string.IsNullOrWhiteSpace(otherFilePath))
                throw new ArgumentNullException("otherFilePath is null or empty");

            if (!File.Exists(filePath))
                throw new ArgumentException("filePath is not valid, file does not exist");

            if (!File.Exists(otherFilePath))
                throw new ArgumentException("otherFilePath is not valid, file does not exist");

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String(sha1.ComputeHash(File.ReadAllBytes(filePath)))
                    .Equals(Convert.ToBase64String(sha1.ComputeHash(File.ReadAllBytes(otherFilePath))));
            }
        }
    }
    }
