using System;
using System.Collections.Generic;

namespace ProjectOrangeSunshine.Shared
{
    public class Utilities
    {
        public static string ArrayToString<T>(T[] array)
        {
            string str = "";
            foreach (T element in array)
            {
                if (element != null)
                    str += element.ToString() + ",";
            }
            str = str[0..^1];
            return str;
        }

        public static byte[] StringToByteArray(string str)
        {
            string[] bytes = str.Split(',');
            List<byte> arrayAsList = new();
            foreach (string s in bytes)
            {
                try
                {
                    _ = byte.TryParse(s, out byte b);
                    arrayAsList.Add(b);
                }
                catch(Exception e)
                {
                    Logger.LogServerError(100, e.ToString());
                }
            }
            return arrayAsList.ToArray();
        }
    }
}
