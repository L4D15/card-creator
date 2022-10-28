using UnityEngine;

namespace Becerra.Save
{
    public class SaveService
    {
        public readonly string savePath;

        public SaveService(string savePath)
        {
            this.savePath = savePath;

            if (System.IO.Directory.Exists(savePath) == false)
            {
                System.IO.Directory.CreateDirectory(savePath);
            }
        }

        public void PrepareFolder(string folderName)
        {
            string fullPath = System.IO.Path.Combine(savePath, folderName);

            if (System.IO.Directory.Exists(fullPath) == false)
            {
                System.IO.Directory.CreateDirectory(fullPath);
            }
        }

        public void SaveTexture(Texture2D texture, string folder)
        {
            var bytes = texture.EncodeToPNG();
            string folderPath = System.IO.Path.Combine(savePath, folder);
            string path = folderPath + "/" + texture.name + ".png";

            Debug.Log("Saving texture " + texture + " at " + path);
            
            System.IO.File.WriteAllBytes(path, bytes);
        }
    }
}
