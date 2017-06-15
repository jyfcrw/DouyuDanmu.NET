using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouyuDanmu
{
    public class DouyuUtility
    {
        public static string Serialize(Dictionary<string, object> data)
        {
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, object> item in data)
            {
                string key = Escape(item.Key.ToString());
                string value = Escape(item.Value.ToString());
                str.Append(key + "@=" + value + "/");
            }

            return str.ToString();
        }

        public static Dictionary<string, object> Deserialize(string data)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            string[] dataPair = data.Split('/');
            foreach (string pair in dataPair)
            {
                int pos = pair.IndexOf("@=");
                if (pos > 0)
                {
                    string key = pair.Substring(0, pos);
                    string value = pair.Substring(pos + 2);

                    dict[Descape(key)] = Descape(value);
                }
            }

            return dict;
        }

        public static string Escape(string s)
        {
            return s.Replace("@", "@A").Replace("/", "@S");
        }

        public static string Descape(string s)
        {
            return s.Replace("@S", "/").Replace("@A", "@");
        }

        public static long UnixTimestamp()
        {
            DateTime time = DateTime.Now;
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, time.Kind);
            return Convert.ToInt64((time - start).TotalSeconds);
        }
    }
}
