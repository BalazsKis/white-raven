namespace WhiteRaven.Shared.Basics
{
    public static class StringExtentions
    {
        /// <summary>
        /// Returns true if the string is null, empty, or only consists of whitespace character(s)
        /// </summary>
        /// <param name="s">The string to inspect</param>
        /// <returns>True if the string is null, empty, or only consists of whitespace character(s)</returns>
        public static bool IsBlank(this string s) =>
            string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
    }
}