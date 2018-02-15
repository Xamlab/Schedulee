using System;

namespace Schedulee.Core.Models
{
    public class Token
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }

        public DateTime Created { get; set; }

        public User User { get; set; }

        public bool IsExpired => DateTime.Now > Created.AddSeconds(ExpiresIn - 10);
    }
}