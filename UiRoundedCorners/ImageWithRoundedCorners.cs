using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImageWithRoundedCorners : BaseImageWithRoundedCorners
{
    private static readonly int Props = Shader.PropertyToID("_WidthHeightRadius");
	private static readonly int RectProp = Shader.PropertyToID("_Rect");

	public Material material;
	public float radius;

    protected override void Refresh(){
		var rect = ((RectTransform) transform).rect;
        this.UpdateSpriteBounds(material, RectProp);
        material.SetVector(Props, new Vector4(rect.width, rect.height, radius, 0));
	}
}
