using System;
using System.Text;

namespace PayPal.OAuth
{
    public class PayPalURLEncoder
    {
        public const string digits = "0123456789abcdef";

        public static string Encode(string message, string name)
        {
            if (message == null || name == null)
            {
                throw new NullReferenceException();
            }
            StringBuilder builder = new StringBuilder(message.Length + 16);
            int start = -1;

            for (int i = 0; i < message.Length; i++)
            {
                char ch = message[i];
                if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')
                    || (ch >= '0' && ch <= '9') || " _".IndexOf(ch) > -1) //removed "." and "-" and "*"
                {
                    if (start >= 0)
                    {
                        Convert(message.Substring(start, (i - start)), builder, name);
                        start = -1;
                    }
                    if (ch != ' ')
                    {
                        builder.Append(ch);
                    }
                    else
                    {
                        builder.Append('+');
                    }
                }
                else
                {
                    if (start < 0)
                    {
                        start = i;
                    }
                }
            }
            if (start >= 0)
            {
                Convert(message.Substring(start, (message.Length - start)), builder, name);
            }

            return builder.ToString(0, builder.Length);
        }

        private static void Convert(string message, StringBuilder builder, string name)
        {
            Encoding encoding = System.Text.Encoding.GetEncoding(name);
            byte[] bytes = encoding.GetBytes(message);

            for (int j = 0; j < bytes.Length; j++)
            {
                builder.Append('%');
                builder.Append(digits[((bytes[j] & 0xf0) >> 4)]);
                builder.Append(digits[(bytes[j] & 0xf)]);
            }
        }
    }
}