using System.Text.RegularExpressions;

namespace Becerra.Carder
{
    public class CardSectionActionTitle : CardSection
    {
        private const string TitleRegularExpression = @"(?<=## ).*";    // Matches between ##

        public string Title { get; private set; }
        
        public CardSectionActionTitle(string rawText)
            : base(rawText)
        {
            var regex = new Regex(TitleRegularExpression);
            var result = regex.Match(rawText);

            Title = result.Value;
        }

        public static bool IsOfType(string text)
        {
            var regex = new Regex(TitleRegularExpression);
            var result = regex.Match(text);

            return result.Success;
        }
    }
}