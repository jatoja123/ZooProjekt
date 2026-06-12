using System.Collections.Generic;
using System.IO;
using Raylib_cs;

namespace Zoo.GUI
{
    public static class AssetLoader
    {
        private static readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static Texture2D? GetAnimalTexture(string animalName)
        {
            if (textures.TryGetValue(animalName, out Texture2D texture))
            {
                return texture;
            }

            string filePath = Path.Combine("assets", $"{animalName.ToLower()}.png");

            if (File.Exists(filePath))
            {
                Texture2D loadedTexture = Raylib.LoadTexture(filePath);
                textures[animalName] = loadedTexture;
                return loadedTexture;
            }

            return null;
        }

        public static void UnloadAllTextures()
        {
            foreach (var texture in textures.Values)
            {
                Raylib.UnloadTexture(texture);
            }
            textures.Clear();
        }
    }
}