namespace Questao2.Util
{
    public static class Helper
    {
        public class Serialize
        {
            public static T StringToObject<T>(string value)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            }
        }
    }
}
