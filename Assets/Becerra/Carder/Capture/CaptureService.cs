using Becerra.Carder.Page;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Becerra.Carder.Capture
{
    public class CaptureService
    {
        public async Task<Texture2D> CaptureCard(CardView card)
        {
            Screen.SetResolution(card.Width * 2, card.Height, false);

            var rect = card.GetComponent<RectTransform>();
            int width = Mathf.FloorToInt(rect.sizeDelta.x);
            int height = Mathf.FloorToInt(rect.sizeDelta.y);
            int x = Mathf.FloorToInt(Screen.width * 0.5f + rect.rect.x);
            int y = Mathf.FloorToInt(Screen.height * 0.5f + rect.rect.y);
            
            var capturedRect = new Rect(x, y, width, height);

            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);

            await new WaitForEndOfFrame();

            texture.name = card.Model.name;
            texture.ReadPixels(capturedRect, 0, 0);
            texture.Apply();

            return texture;
        }

        public async Task<Texture2D> CaptureCardFront(CardView card)
        {
            var rect = card.GetComponent<RectTransform>();
            int width = Mathf.FloorToInt(rect.sizeDelta.x * 0.5f);
            int height = Mathf.FloorToInt(rect.sizeDelta.y);
            int x = Mathf.FloorToInt(Screen.width * 0.5f + rect.rect.x);
            int y = Mathf.FloorToInt(Screen.height * 0.5f + rect.rect.y);

            var capturedRect = new Rect(x, y, width, height);

            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);

            await new WaitForEndOfFrame();

            texture.name = card.Model.name + "(Front)";
            texture.ReadPixels(capturedRect, 0, 0);
            texture.Apply();

            return texture;
        }

        public async Task<Texture2D> CaptureCardBack(CardView card)
        {
            var rect = card.GetComponent<RectTransform>();
            int width = Mathf.FloorToInt(rect.sizeDelta.x * 0.5f);
            int height = Mathf.FloorToInt(rect.sizeDelta.y);
            int x = Mathf.FloorToInt(Screen.width * 0.5f + rect.rect.x);
            int y = Mathf.FloorToInt(Screen.height * 0.5f + rect.rect.y);

            var capturedRect = new Rect(x + width, y, width, height);

            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);

            await new WaitForEndOfFrame();

            texture.name = card.Model.name + "(Back)";
            texture.ReadPixels(capturedRect, 0, 0);
            texture.Apply();

            return texture;
        }
    }
}