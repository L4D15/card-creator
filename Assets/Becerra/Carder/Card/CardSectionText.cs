namespace Becerra.Carder
{
    public class CardSectionText : CardSection
    {
        public string BodyText { get; private set; }

        public CardSectionText(string rawText)
            : base(rawText)
        {
            BodyText = rawText;
            
            // TODO: Convert Markdown __ and ** to <b> and <i> markers
        }
    }
}