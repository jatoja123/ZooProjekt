using System.Collections.Generic;
using System.IO;
using Raylib_cs;
using Zoo.Animals;

namespace Zoo.GUI
{
    public static class AssetLoader
    {
        private static readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static Texture2D? GetAnimalTexture(Animal animal)
        {
            string speciesName = animal.GetType().Name;

            if (textures.TryGetValue(speciesName, out Texture2D texture))
            {
                return texture;
            }

            string filePath = Path.Combine("assets", $"{speciesName.ToLower()}.png");

            if (File.Exists(filePath))
            {
                Texture2D loadedTexture = Raylib.LoadTexture(filePath);
                textures[speciesName] = loadedTexture;
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