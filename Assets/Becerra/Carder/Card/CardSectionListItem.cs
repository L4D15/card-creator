using System.Text.RegularExpressions;

namespace Becerra.Carder
{
    public class CardSectionListItem : CardSection
    {
        public const string RegularExpression = @"(?<=(\-+\s)).*";
        
        public string Text { get; private set; }
        
        public CardSectionListItem(string rawText)
            : base(rawText)
        {
            var regex = new Regex(RegularExpression);
            var match = regex.Match(rawText);

            Text = match.Value;
        }

        public static bool IsOfType(string text)
        {
            var regex = new Regex(RegularExpression);
            var match = regex.Match(text);

            return match.Success;
        }
    }
}