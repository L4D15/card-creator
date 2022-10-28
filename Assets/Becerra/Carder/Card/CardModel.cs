using System.Collections.Generic;

namespace Becerra.Carder
{
    public class CardModel
    {
        public const int NO_LEVEL = -99;

        public List<CardSection> sections;
        public List<string> categories;
        public List<string> tags;

        // Metadata
        public string name;
        public string frontImage;
        public string backImage;
        public string source;
        public string level;
        
        public CardModel()
        {
            this.sections = new List<CardSection>();
            this.categories = new List<string>();
            this.tags = new List<string>();
            this.level = string.Empty;
        }
    }
}