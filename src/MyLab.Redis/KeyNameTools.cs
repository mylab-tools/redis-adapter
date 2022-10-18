namespace MyLab.Redis
{
    static class KeyNameTools
    {
        public static string BuildName(string? prefix, string name)
        {
            return prefix != null
                ? $"{prefix}:{name}"
                : name;
        }
    }
}