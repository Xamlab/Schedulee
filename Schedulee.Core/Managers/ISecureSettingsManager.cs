using Schedulee.Core.Models;

namespace Schedulee.Core.Managers
{
    public interface ISecureSettingsManager
    {
        void SetAccount(Token account);
        Token GetAccount();
        void Clear();
    }
}
