using UnityEngine;
using UnityEngine.UI;

public static class ImageWithRoundedCornersUtils
{
    public static void UpdateSpriteBounds(this MonoBehaviour mono, Material material, int property)
    {
        var image = mono.GetComponent<Image>();
        // TODO: It would be nice to avoid this if the sprite didn't change
        if (image && image.sprite)
        {
            Sprite sprite = image.sprite;
            Rect spriteTextureRect = sprite.textureRect;
            Texture spriteTexture = sprite.texture;
            Vector4 result = new Vector4(
                spriteTextureRect.min.x / spriteTexture.width,
                spriteTextureRect.min.y / spriteTexture.height,
                spriteTextureRect.max.x / spriteTexture.width,
                spriteTextureRect.max.y / spriteTexture.height);
            material.SetVector(property, result);
        }
        else
        {
            material.SetVector(property, new Vector4(0, 0, 1, 1));
        }
    }
}