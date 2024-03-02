using System;
using UnityEngine;
using UnityEngine.U2D;

namespace Castle.CustomUtil
{
    public class GameAssetManager : MonoBehaviour
    {
        [SerializeField] private SpriteAtlas heroAtlas;

        public Sprite GetHeroSprite(string path)
        {
            Sprite sprite = heroAtlas.GetSprite(path);
            if (sprite == null)
            {
                Debug.LogError($"Sprite with path {path} not found in heroAtlas.");
            }
            return sprite;
        }
    }
}