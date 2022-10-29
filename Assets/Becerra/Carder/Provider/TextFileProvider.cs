using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Becerra.Carder.Provider
{
    public class TextFileProvider
    {
        public async Task<string> LoadText(string path)
        {
            var loader = UnityWebRequest.Get("file://" + path);

            await loader.SendWebRequest();

            var handler = loader.downloadHandler;

            if (string.IsNullOrEmpty(loader.error))
            {
                return handler.text;
            }
            else
            {
                Debug.LogErrorFormat("Error loading text file '{0}': {1}", loader.uri, loader.error);
            }

            return string.Empty;
        }
    }
}