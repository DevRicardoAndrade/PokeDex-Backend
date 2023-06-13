using System.Text.Json;

namespace Poke_Api.Utils
{
    public static class ENV
    {

        private static string _secretKey = "";
        private static IConfiguration? _config;

        public static void Initialize(IConfiguration config)
        {
            try
            {
                _config = config;
                _secretKey = _config.GetValue<string>("secret_key");
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public static string SecretKey(string password)
        {
            try
            {
                return password == "PokeApi" ? _secretKey : "";
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }


    }
}
