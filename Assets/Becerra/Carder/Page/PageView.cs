using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Becerra.Carder.Page
{
    public class PageView
    {
        public const int MAX_CARDS = 9;
        public const int WIDTH = 2480;
        public const int HEIGHT = 3508;
        public const int PADDING = 24;

        public CardView CardView { get; }
        public int VerticalMargin => (HEIGHT - CardView.Height * 3) / 2;
        public int HorizontalMargin => (WIDTH - CardView.Width * 3) / 2;

        public struct CardTextures
        {
            public Texture2D front;
            public Texture2D back;

            public CardTextures(Texture2D front, Texture2D back)
            {
                this.front = front;
                this.back = back;
            }
        }

        public IEnumerable<CardTextures> Cards => _cards;
        public bool IsFull => _cards.Count >= MAX_CARDS;
        public bool IsEmpty => _cards.Count < 1;
        public int Index { get; }

        private readonly List<CardTextures> _cards;

        public PageView(int index, CardView cardView)
        {
            this.Index = index;
            this.CardView = cardView;
            this._cards = new List<CardTextures>();
        }

        public void AddCard(Texture2D cardFront, Texture2D cardBack)
        {
            CardTextures card = new(cardFront, cardBack);

            _cards.Add(card);
        }

        public void Clear()
        {
            _cards.Clear();
        }

        public async Task<Texture2D> GenerateFrontTexture()
        {
            if (_cards.Count == 0) return new Texture2D(0, 0);

            var texture = await CreateBlankPageTexture();

            for (int i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];
                var position = CalculateFrontPosition(i);
                var pixels = card.front.GetPixels32();
                int x = (int)position.x;
                int y = (int)position.y;
                int width = card.front.width;
                int height = card.front.height;

                Debug.Log($"Applying card {i + 1} to x:{x} y:{y} w:{width} h:{height}");

                texture.SetPixels32(x, y, width, height, pixels);
            }

            texture.name = $"_Page {Index + 1} (Front)";
            texture.Apply();

            return texture;
        }

        public async Task<Texture2D> GenerateBackTexture()
        {
            if (_cards.Count == 0) return new Texture2D(0, 0);

            var texture = await CreateBlankPageTexture();

            for (int i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];
                var position = CalculateBackPosition(i);
                var pixels = card.back.GetPixels32();
                texture.SetPixels32((int)position.x, (int)position.y, card.front.width, card.front.height, pixels);
            }

            texture.name = $"_Page {Index + 1} (Back)";
            texture.Apply();

            return texture;
        }

        private Vector2 CalculateFrontPosition(int cardIndex)
        {
            int width = CardView.Width;
            int height = CardView.Height;
            int x;
            int y;

            x = (cardIndex % 3) * width;

            if (cardIndex <= 2) y = height * 2;
            else if (cardIndex <= 5) y = height;
            else y = 0;

            var position = new Vector2(x, y);
            var margin = new Vector2(HorizontalMargin, VerticalMargin);
            var padding = CalculatePadding(cardIndex);

            return position + margin + padding;
        }

        private Vector2 CalculateBackPosition(int cardIndex)
        {
            int width = CardView.Width;
            int height = CardView.Height;
            int x;
            int y;

            x = (cardIndex % 3) * width;
            if (cardIndex == 0 || cardIndex == 3 || cardIndex == 6) x = width * 2;
            else if (cardIndex == 1 || cardIndex == 4 || cardIndex == 7) x = width;
            else x = 0;

            if (cardIndex <= 2) y = height * 2;
            else if (cardIndex <= 5) y = height;
            else y = 0;

            var position = new Vector2(x, y);
            var margin = new Vector2(HorizontalMargin, VerticalMargin);
            var padding = CalculatePadding(cardIndex) * new Vector2(-1, 1);

            return position + margin + padding;
        }

        private Vector2 CalculatePadding(int cardIndex)
        {
            int x = 0;
            int y = 0;

            switch (cardIndex)
            {
                case 0:
                case 3:
                case 6:
                    {
                        x = -PADDING;
                        break;
                    }
                case 2:
                case 5:
                case 8:
                    {
                        x = PADDING;
                        break;
                    }
            }

            switch (cardIndex)
            {
                case 0:
                case 1:
                case 2:
                    {
                        y = PADDING;
                        break;
                    }
                case 6:
                case 7:
                case 8:
                    {
                        y = -PADDING;
                        break;
                    }
            }

            return new Vector2(x, y);
        }

        private async Task<Texture2D> CreateBlankPageTexture()
        {
            var texture = new Texture2D(WIDTH, HEIGHT, TextureFormat.RGB24, false);
            var originalPixels = texture.GetPixels32();
            Color32 white = new Color32(255, 255, 255, 255);

            for (int i = 0; i < originalPixels.Length; i++)
            {
                originalPixels[i] = white;
            }

            texture.SetPixels32(originalPixels);

            return texture;
        }
    }
}