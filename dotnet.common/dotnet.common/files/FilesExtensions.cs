using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace dotnet.common.files
{
    /// <summary>
    ///     Common extensions / helper methods for files and bytes
    /// </summary>
    public static class FilesExtensions
    {
        public enum SizeUnits
        {
            Byte,
            KB,
            MB,
            GB,
            TB,
            PB,
            EB,
            ZB,
            YB
        }

        /// <summary>
        ///     Formats length of a file on to a human readable format for example 2.3 MB
        /// </summary>
        /// <param name="value">Length of file/byte array</param>
        /// <param name="unit">What unit to use MB, GB etc.</param>
        /// <returns>String with human readable format for the size</returns>
        public static string ToFileSize(this long value, SizeUnits unit)
        {
            return (value/Math.Pow(1024, (long) unit)).ToString("0.00") + " " + unit;
        }

        /// <summary>
        ///     Formats length of a file on to a human readable format for example 2.3 MB
        /// </summary>
        /// <param name="value">Length of file/byte array</param>
        /// <param name="unit">What unit to use MB, GB etc.</param>
        /// <returns>String with human readable format for the size</returns>
        public static string ToFileSize(this int value, SizeUnits unit)
        {
            return (value/Math.Pow(1024, (int) unit)).ToString("0.00") + " " + unit;
        }

        /// <summary>
        ///     Returns the size of the byte array on a human readable format for example 2.3 MB
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="unit">What unit to use MB, GB etc.</param>
        /// <returns>String with human readable format for the size</returns>
        public static string ToFileSize(this byte[] bytes, SizeUnits unit)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Format("0 {0}", unit);
            return bytes.Length.ToFileSize(unit);
        }

        /// <summary>
        ///     Add trailing slash on a windows path if the path does not end with a \
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EnsureLastSlashInWindowsPath(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return value.EndsWith("\\") ? value : value + "\\";
        }

        /// <summary>
        ///     Get a list of alle the files with full path of all the files in a folder can be filered by file extension
        /// </summary>
        /// <param name="path">The path to the directory to list</param>
        /// <param name="fileExtension">Filter files to get by this extension (optional) </param>
        /// <param name="includeSubDirectories">Include files in subdirectories (recursive) (optional)</param>
        /// <param name="onlyFilesOlderThan">Only get files older than (optional)</param>
        /// <returns>List of files with full paths as list of strings</returns>
        public static IList<string> GetFilesInFolderWithExtension(this string path, string fileExtension = null,
            bool includeSubDirectories = false, TimeSpan? onlyFilesOlderThan = null)
        {
            if (!string.IsNullOrWhiteSpace(fileExtension) && !fileExtension.StartsWith("."))
                fileExtension = "." + fileExtension;


            return path.GetFilesInFolderAsPaths(string.Format("*{0}", fileExtension), includeSubDirectories,
                onlyFilesOlderThan);
        }

        /// <summary>
        ///     Get a list of alle the files with full path of all the files in a folder can be filered by file extension
        /// </summary>
        /// <param name="path">The path to the directory to list</param>
        /// <param name="pattern">Filter files to get by this pattern (optional) </param>
        /// <param name="includeSubDirectories">Include files in subdirectories (recursive) (optional)</param>
        /// <param name="onlyFilesOlderThan">Only get files older than (optional)</param>
        /// <returns>List of files with full paths as list of strings</returns>
        public static IList<string> GetFilesInFolderAsPaths(this string path, string pattern = "*.*",
            bool includeSubDirectories = false, TimeSpan? onlyFilesOlderThan = null)
        {
            if (!Directory.Exists(path))
                return new List<string>();

            return
                path.GetFilesInFolder(pattern, includeSubDirectories, onlyFilesOlderThan)
                    .Select(x => x.FullName)
                    .ToList();
        }

        /// <summary>
        ///     Get a list of alle the files with full path of all the files in a folder can be filered by file extension
        /// </summary>
        /// <param name="path">The path to the directory to list</param>
        /// <param name="pattern">Filter files to get by this pattern (optional) </param>
        /// <param name="includeSubDirectories">Include files in subdirectories (recursive) (optional)</param>
        /// <param name="onlyFilesOlderThan">Only get files older than (optional)</param>
        /// <returns>List of fileinfo</returns>
        public static IList<FileInfo> GetFilesInFolder(this string path, string pattern = "*.*",
            bool includeSubDirectories = false, TimeSpan? onlyFilesOlderThan = null)
        {
            if (!Directory.Exists(path))
                return new List<FileInfo>();

            return Directory.GetFiles(path, pattern,
                includeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Select(f => new FileInfo(f))
                .Where(f => onlyFilesOlderThan == null || (DateTime.Now - f.CreationTime) > onlyFilesOlderThan.Value)
                .ToList();
        }

        /// <summary>
        ///     Checks via SHA1 hash that two files have the same content (are identical)
        /// </summary>
        /// <param name="filePath">Full filepath to file to check</param>
        /// <param name="otherFilePath">Full filepath to file to compare with</param>
        /// <returns>True if the files are identifical (content) and false if not</returns>
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