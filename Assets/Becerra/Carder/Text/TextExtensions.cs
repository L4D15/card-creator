using System.Text.RegularExpressions;

namespace Becerra.Carder.Text
{
    public static class TextExtensions
    {
        public static string ToRichText(this string text)
        {
            string result = text;

            if (string.IsNullOrEmpty(result)) return string.Empty;

            result = ConvertBold(result);
            result = ConvertItalic(result);
            result = ApplyEmojis(result);

            return result;
        }

        public static string ApplyEmojis(this string text)
        {
            string result = text;

            result = ApplyFreeAction(result);
            result = ApplyOneAction(result);
            result = ApplyTwoAction(result);
            result = ApplyThreeAction(result);
            result = ApplyReactionAction(result);
            result = ApplyOneToTwoAction(result);
            result = ApplyOneToThreeAction(result);
            result = ApplyTwoToThreeAction(result);

            return result;
        }

        private static string ConvertBold(string sourceText)
        {
            string result = sourceText;

            var regex = new Regex(@"__.*?__");
            var preMarkRegex = new Regex(@"__(?=.+?)");
            var postMarkRegex = new Regex(@"(?<=.+?)__");
            var matches = regex.Matches(sourceText);

            foreach (Match match in matches)
            {
                string originalText = match.Value;
                string convertedText = originalText;

                convertedText = preMarkRegex.Replace(convertedText, "<b>");
                convertedText = postMarkRegex.Replace(convertedText, "</b>");

                result = result.Replace(originalText, convertedText);
            }

            return result;
        }

        private static string ConvertItalic(string sourceText)
        {
            string result = sourceText;

            var regex = new Regex(@"\*\*.*?\*\*");
            var preMarkRegex = new Regex(@"\*\*(?=.+?)");
            var postMarkRegex = new Regex(@"(?<=.+?)\*\*");
            var matches = regex.Matches(sourceText);

            foreach (Match match in matches)
            {
                string originalText = match.Value;
                string convertedText = originalText;

                convertedText = preMarkRegex.Replace(convertedText, "<i>");
                convertedText = postMarkRegex.Replace(convertedText, "</i>");

                result = result.Replace(originalText, convertedText);
            }

            return result;
        }

        private static string ApplyFreeAction(string text)
        {
            return text.Replace("{0}", "<sprite=5>");
        }

        private static string ApplyOneAction(string text)
        {
            return text.Replace("{1}", "<sprite=0>");
        }

        private static string ApplyTwoAction(string text)
        {
            return text.Replace("{2}", "<sprite=1>");
        }

        private static string ApplyThreeAction(string text)
        {
            return text.Replace("{3}", "<sprite=2>");
        }

        private static string ApplyReactionAction(string text)
        {
            return text.Replace("{R}", "<sprite=4>");
        }

        private static string ApplyOneToTwoAction(string text)
        {
            return text.Replace("{1-2}", "<sprite=7>");
        }

        private static string ApplyOneToThreeAction(string text)
        {
            return text.Replace("{1-3}", "<sprite=11>");
        }

        private static string ApplyTwoToThreeAction(string text)
        {
            return text.Replace("{2-3}", "<sprite=4>");
        }
    }
}
