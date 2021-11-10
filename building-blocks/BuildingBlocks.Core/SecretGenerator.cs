using System.Security.Cryptography;

namespace BuildingBlocks.Core
{
    public static class SecretGenerator
    {
        public static string Generate()
        {            
            var AES = new AesCryptoServiceProvider();
            AES.GenerateKey();
            return System.Convert.ToBase64String(AES.Key);
        }
    }

}
