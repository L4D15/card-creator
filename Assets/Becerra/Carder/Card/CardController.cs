
namespace Becerra.Carder
{
    public class CardController
    {
        public readonly string rawText;
        public CardModel model;

        public CardController(string rawText)
        {
            this.rawText = rawText;
        }

        public void Parse(CardParser cardParser)
        {
            model = cardParser.Parse(rawText);
        }
    }
}