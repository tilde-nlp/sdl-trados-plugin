using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetsMT.MTProvider
{
    public class ApiCredential
    {
        public string Token { get; set; }
        public string AppId { get; set; }

        public static ApiCredential ParseCredential(string strCredential)
        {
            string token = null;
            string appId = null;

            string[] credParams = strCredential.Split('\t');

            if (credParams.Length == 2) // If length > 2 then the credentials are stored in the old format (username\tpassword\tappId). Don't use them.
            {
                token = credParams[0];
                appId = credParams[1];
            }
            return new ApiCredential { Token = token, AppId = appId };
        }
    }
}
