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
		ValidateConstraints(rect);
		material.SetVector(Props, new Vector4(rect.width, rect.height, radius, 0));
	}

	private void ValidateConstraints(Rect rect){
		if (radius > rect.width) radius = rect.width;
		if (radius > rect.height) radius = rect.height;
		if (radius < 0) radius = 0;
	}
}
