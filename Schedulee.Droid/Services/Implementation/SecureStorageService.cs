using System.IO;
using System.IO.IsolatedStorage;
using Java.Lang;
using Java.Security;
using Javax.Crypto;
using Schedulee.UI.Services;
using Exception = System.Exception;

namespace Schedulee.Droid.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="ISecureStorage"/> using Android KeyStore.
    /// </summary>
    public class SecureStorageService : ISecureStorageService
    {
        private static IsolatedStorageFile File => IsolatedStorageFile.GetUserStoreForApplication();
        private static readonly object SaveLock = new object();

        private const string StorageFile = "Schedulee.KeyVaultStorage";

        private readonly KeyStore _keyStore;
        private readonly KeyStore.PasswordProtection _protection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureStorageService"/> class.
        /// </summary>
        /// <param name="password">Password to use for encryption.</param>
        public SecureStorageService(char[] password)
        {
            _keyStore = KeyStore.GetInstance(KeyStore.DefaultType);
            _protection = new KeyStore.PasswordProtection(password);

            if(File.FileExists(StorageFile))
            {
                using(var stream = new IsolatedStorageFileStream(StorageFile, FileMode.Open, FileAccess.Read, File))
                {
                    _keyStore.Load(stream, password);
                }
            }
            else
            {
                _keyStore.Load(null, password);
            }
        }

        /// <summary>
        /// Stores data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <param name="dataBytes">Data bytes to store.</param>
        public void Store(string key, byte[] dataBytes)
        {
            _keyStore.SetEntry(key, new KeyStore.SecretKeyEntry(new SecureData(dataBytes)), _protection);
            Save();
        }

        /// <summary>
        /// Retrieves stored data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <returns>Byte array of stored data.</returns>
        public byte[] Retrieve(string key)
        {
            if(!(_keyStore.GetEntry(key, _protection) is KeyStore.SecretKeyEntry entry))
            {
                throw new Exception($"No entry found for key {key}.");
            }

            return entry.SecretKey.GetEncoded();
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="key">Key for the data to be deleted.</param>
        public void Delete(string key)
        {
            _keyStore.DeleteEntry(key);
            Save();
        }

        /// <summary>
        /// Checks if the storage contains a key.
        /// </summary>
        /// <param name="key">The key to search.</param>
        /// <returns>True if the storage has the key, otherwise false.</returns>
        public bool Contains(string key)
        {
            return _keyStore.ContainsAlias(key);
        }

        private void Save()
        {
            lock(SaveLock)
            {
                using(var stream = new IsolatedStorageFileStream(StorageFile, FileMode.OpenOrCreate, FileAccess.Write, File))
                {
                    _keyStore.Store(stream, _protection.GetPassword());
                }
            }
        }

        private class SecureData : Object, ISecretKey
        {
            private const string Raw = "RAW";

            private readonly byte[] _data;

            public SecureData(byte[] dataBytes)
            {
                _data = dataBytes;
            }

            public string Algorithm => Raw;

            public string Format => Raw;

            public byte[] GetEncoded()
            {
                return _data;
            }
        }
    }
}