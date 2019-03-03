using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImageWithRoundedCorners : Image {
	private static readonly int Width = Shader.PropertyToID("_Width");
	private static readonly int Height = Shader.PropertyToID("_Height");
	private static readonly int Radius = Shader.PropertyToID("_Radius");

	// TODO think up a better name
	public void SetRadius(float radius){
		this.material.SetFloat(Radius, radius);
	}

	private void OnRenderObject(){
			this.material.SetFloat(Width,((RectTransform)transform).rect.width);
			this.material.SetFloat(Height,((RectTransform)transform).rect.height);
	}
}
