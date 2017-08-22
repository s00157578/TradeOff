using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core.Services
{
    public class TokenManager
    {
        private byte[] secret = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };

        public string DecodeIdToken()
        {
            var token = CrossSecureStorage.Current.GetValue("idToken");
            string json = "";
            if(!string.IsNullOrEmpty(token))
                 json = JosePCL.Jwt.Decode(token, secret);
            return json;
        }
    }
}
