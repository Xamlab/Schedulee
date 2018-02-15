using System.Text;
using Newtonsoft.Json;
using Schedulee.Core.Managers;
using Schedulee.Core.Models;
using Schedulee.Core.Providers;
using Schedulee.UI.Services;

namespace Schedulee.UI.Managers.Implementation
{
    public class SecureSettingsManager : ISecureSettingsManager
    {
        private readonly ISecureStorageService _secureStorage;
        private readonly string _accountKey;
        private readonly object _accountLocker = new object();

        public SecureSettingsManager(ISecureStorageService secureStorage, IConfigurationProvider configurationProvider)
        {
            _secureStorage = secureStorage;
            _accountKey = $"{configurationProvider.AppId}:Account";
        }

        public void SetAccount(Token account)
        {
            lock(_accountLocker)
            {
                SetProtectedObject(_accountKey, account);
            }
        }

        public Token GetAccount()
        {
            lock(_accountLocker)
            {
                return GetProtectedObject<Token>(_accountKey);
            }
        }

        public void Clear()
        {
            lock(_accountLocker)
            {
                Remove(_accountKey);
            }
        }

        private void SetProtectedObject<T>(string key, T value)
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            _secureStorage.Store(key, Encoding.UTF8.GetBytes(jsonValue));
        }

        private T GetProtectedObject<T>(string key)
        {
            if(_secureStorage.Contains(key))
            {
                var raw = _secureStorage.Retrieve(key);
                var jsonResult = Encoding.UTF8.GetString(raw, 0, raw.Length);
                var result = JsonConvert.DeserializeObject<T>(jsonResult);
                return result;
            }

            return default(T);
        }

        private void Remove(string key)
        {
            if(_secureStorage.Contains(key))
            {
                _secureStorage.Delete(key);
            }
        }
    }
}