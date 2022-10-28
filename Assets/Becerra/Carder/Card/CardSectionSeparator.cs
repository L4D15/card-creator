using System.Text.RegularExpressions;

namespace Becerra.Carder
{
    public class CardSectionSeparator : CardSection
    {
        public const string RegularExpression = "^---";
        
        public CardSectionSeparator(string rawText)
            : base(rawText)
        {
            
        }

        public static bool IsOfType(string text)
        {
            var regex = new Regex(RegularExpression);
            var match = regex.Match(text);

            return match.Success;
        }
    }
}