using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Becerra.Carder.Provider
{
    public class SpriteProvider
    {
        public async Task<Sprite> LoadSprite(string path)
        {
            Texture2D texture;

            var loader = UnityWebRequestTexture.GetTexture("file://" + path);

            await loader.SendWebRequest();

            if (string.IsNullOrEmpty(loader.error))
            {
                texture = DownloadHandlerTexture.GetContent(loader);
            }
            else
            {
                texture = null;
                Debug.LogErrorFormat("Error loading Texture '{0}': {1}", loader.uri, loader.error);
            }

            if (texture == null) return null;

            var sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f);

            return sprite;
        }
    }
}