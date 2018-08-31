namespace Framework.Helper.Extension
{
    public static class StringExtension
    {
        public static string RemoveFromEnd(this string str, params string[] valores)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            foreach (var item in valores)
            {
                if (str.EndsWith(item))
                {
                    str = str.Substring(0, str.Length - item.Length);
                }
                else if (str.StartsWith(item))
                {
                    str = str.Substring(item.Length, str.Length - item.Length);
                }
            }

            return str;
        }
    }
}