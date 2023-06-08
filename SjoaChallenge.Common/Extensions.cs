namespace SjoaChallenge.Utilities
{
    public static class Extensions
    {
        public static bool EqualsIgnoreCase(this string value, string input) => 
            value.Equals(input, StringComparison.InvariantCultureIgnoreCase);
    }
}
