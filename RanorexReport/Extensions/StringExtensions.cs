using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace RanorexReport.Extensions
{
    public static class StringExtensions
    {
        public static T FromXmlTo<T>(this string xmlFilenamePath)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(xmlFilenamePath)) return default(T);

            try
            {
                StreamReader xmlStream = new StreamReader(xmlFilenamePath);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                returnObject = (T)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return returnObject;
        }

        public static string GetMD5HashCode(this string input)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
