using UnityEngine;
using UnityEngine.UI;

namespace Nobi.UiRoundedCorners {
	[ExecuteInEditMode]
    [DisallowMultipleComponent]						//FFaUniHan: You can only have one of these in every object.
    [RequireComponent(typeof(RectTransform))]
	public class ImageWithRoundedCorners : MonoBehaviour {
		private static readonly int Props = Shader.PropertyToID("_WidthHeightRadius");

		public float radius;
		private Material material;

		[HideInInspector, SerializeField] private Image image;

		private void OnValidate() {
			Validate();
			Refresh();
		}

		private void OnDestroy() {
			image.material = null;		//FFaUniHan: This makes so that when the component is removed, the UI material returns to null

			DestroyHelper.Destroy(material);
			image = null;
			material = null;
		}

		private void OnEnable() {
            //FFaUniHan: You can only add either regular UI rounded corner or independent.
            var other = GetComponent<ImageWithIndependentRoundedCorners>();
            if (other != null)
            {
                DestroyHelper.Destroy(other);
            }

            Validate();
			Refresh();
        }

		private void OnRectTransformDimensionsChange() {
			if (enabled && material != null) {
				Refresh();
			}
		}

		public void Validate() {
			if (material == null) {
				material = new Material(Shader.Find("UI/RoundedCorners/RoundedCorners"));
			}

			if (image == null) {
				TryGetComponent(out image);
			}

			if (image != null) {
				image.material = material;
			}
		}

		public void Refresh() {
			var rect = ((RectTransform)transform).rect;
			material.SetVector(Props, new Vector4(rect.width, rect.height, radius, 0));
		}
	}
}