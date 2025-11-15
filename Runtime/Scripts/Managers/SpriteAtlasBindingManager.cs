using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace LCHFramework.Managers
{
    internal class SpriteAtlasBindingManager
    {
        private static readonly Dictionary<string, SpriteAtlas> SpriteAtlases = new();
        
        
        
        [RuntimeInitializeOnLoadMethod]
        private static void InitializeOnLoadMethod()
        {
            SpriteAtlasManager.atlasRequested -= AtlasRequested;
            SpriteAtlasManager.atlasRequested += AtlasRequested;
        }
        
        private static void AtlasRequested(string tag, Action<SpriteAtlas> action)
        {
            if (SpriteAtlases.TryGetValue(tag, out var value)) action.Invoke(value);
        }
        
        public static void AddSpriteAtlas(SpriteAtlas spriteAtlas) => SpriteAtlases.Add(spriteAtlas.name, spriteAtlas);
        
        public static void RemoveSpriteAtlas(SpriteAtlas spriteAtlas) => SpriteAtlases.Remove(spriteAtlas.name);
    }
}