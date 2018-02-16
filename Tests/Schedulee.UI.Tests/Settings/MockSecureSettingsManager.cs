using Schedulee.Core.Managers;
using Schedulee.Core.Models;

namespace Schedulee.UI.Tests.Settings
{
    public class MockSecureSettingsManager : ISecureSettingsManager
    {
        private Token _account;

        public void SetAccount(Token account)
        {
            _account = account;
        }

        public Token GetAccount()
        {
            return _account;
        }

        public void Clear()
        {
            _account = null;
        }
    }
}