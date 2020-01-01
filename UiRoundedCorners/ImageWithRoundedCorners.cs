using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImageWithRoundedCorners : MonoBehaviour {
	private static readonly int Props = Shader.PropertyToID("_WidthHeightRadius");

	public Material material;
	public float radius;

	void OnRectTransformDimensionsChange(){
		Refresh();
	}
	
	private void OnValidate(){
		Refresh();
	}

	private void Refresh(){
		var rect = ((RectTransform) transform).rect;
		material.SetVector(Props, new Vector4(rect.width, rect.height, radius, 0));
	}
}
