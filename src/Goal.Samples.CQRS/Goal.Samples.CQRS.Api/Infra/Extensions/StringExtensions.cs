using System.Text;

namespace Goal.Samples.CQRS.Api.Infra.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string name)
        {
            string _separator = "_";

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            ReadOnlySpan<char> spanName = name.Trim();

            var stringBuilder = new StringBuilder();
            bool addCharacter = true;

            bool isPreviousSpace = false;
            bool isPreviousSeparator = false;
            bool isCurrentSpace = false;
            bool isNextLower = false;
            bool isNextUpper = false;
            bool isNextSpace = false;

            for (int position = 0; position < spanName.Length; position++)
            {
                if (position != 0)
                {
                    isCurrentSpace = spanName[position] == 32;
                    isPreviousSpace = spanName[position - 1] == 32;
                    isPreviousSeparator = spanName[position - 1] == 95;

                    if (position + 1 != spanName.Length)
                    {
                        isNextLower = spanName[position + 1] is > (char)96 and < (char)123;
                        isNextUpper = spanName[position + 1] is > (char)64 and < (char)91;
                        isNextSpace = spanName[position + 1] == 32;
                    }

                    if (isCurrentSpace &&
                        (isPreviousSpace ||
                        isPreviousSeparator ||
                        isNextUpper ||
                        isNextSpace))
                    {
                        addCharacter = false;
                    }
                    else
                    {
                        bool isCurrentUpper = spanName[position] is > (char)64 and < (char)91;
                        bool isPreviousLower = spanName[position - 1] is > (char)96 and < (char)123;
                        bool isPreviousNumber = spanName[position - 1] is > (char)47 and < (char)58;

                        if (isCurrentUpper &&
                        (isPreviousLower ||
                        isPreviousNumber ||
                        isNextLower ||
                        isNextSpace ||
                        isNextLower && !isPreviousSpace))
                        {
                            stringBuilder.Append(_separator);
                        }
                        else
                        {
                            if (isCurrentSpace &&
                                !isPreviousSpace &&
                                !isNextSpace)
                            {
                                stringBuilder.Append(_separator);
                                addCharacter = false;
                            }
                        }
                    }
                }

                if (addCharacter)
                {
                    stringBuilder.Append(spanName[position]);
                }
                else
                {
                    addCharacter = true;
                }
            }

            return stringBuilder.ToString().ToLower();
        }

        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
            {
                return str;
            }

            char[] chars = str.ToCharArray();
            FixCasing(chars);

            return new string(chars);
        }

        private static void FixCasing(Span<char> chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = i + 1 < chars.Length;

                // Stop when next char is already lowercase.
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    // If the next char is a space, lowercase current char before exiting.
                    if (chars[i + 1] == ' ')
                    {
                        chars[i] = char.ToLowerInvariant(chars[i]);
                    }

                    break;
                }

                chars[i] = char.ToLowerInvariant(chars[i]);
            }
        }
    }
}
