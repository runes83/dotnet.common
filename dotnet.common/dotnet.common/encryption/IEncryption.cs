namespace dotnet.common.encryption
{
    interface IEncryption
    {
        /// <summary>
        /// Encrypt the byes
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <returns>Encrypted bytes</returns>
        byte[] EncryptFile(byte[] fileBytes);

        void EncryptFile(string filePath, string filePathToEncryptedFile);

        string EncryptString(string value);

        byte[] DecryptFile(byte[] fileBytes);

        void DecryptFile(string filePath, string filePathToDecryptedFile);

        string DecryptString(string value);
    }
}
