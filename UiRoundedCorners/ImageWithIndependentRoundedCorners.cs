using UnityEngine;
using UnityEngine.UI;

namespace Nobi.UiRoundedCorners {
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class ImageWithIndependentRoundedCorners : MonoBehaviour {
		private static readonly int prop_halfSize = Shader.PropertyToID("_halfSize");
		private static readonly int prop_radiuses = Shader.PropertyToID("_r");
		private static readonly int prop_rect2props = Shader.PropertyToID("_rect2props");

		// Vector2.right rotated clockwise by 45 degrees
		private static readonly Vector2 wNorm = new Vector2(.7071068f, -.7071068f);
		// Vector2.right rotated counter-clockwise by 45 degrees
		private static readonly Vector2 hNorm = new Vector2(.7071068f, .7071068f);

		public Vector4 r;
		private Material material;

		// xy - position,
		// zw - halfSize
		[HideInInspector, SerializeField] private Vector4 rect2props;
		[HideInInspector, SerializeField] private Image image;

		private void OnValidate() {
			Validate();
			Refresh();
		}

		private void OnEnable() {
			Validate();
			Refresh();
		}

		private void OnRectTransformDimensionsChange() {
			if (enabled && material != null) {
				Refresh();
			}
		}

		private void OnDestroy() {
			DestroyHelper.Destroy(material);
			image = null;
			material = null;
		}

		public void Validate() {
			if (material == null) {
				material = new Material(Shader.Find("UI/RoundedCorners/IndependentRoundedCorners"));
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
			RecalculateProps(rect.size);
			material.SetVector(prop_rect2props, rect2props);
			material.SetVector(prop_halfSize, rect.size * .5f);
			material.SetVector(prop_radiuses, r);
		}

		private void RecalculateProps(Vector2 size) {
			// Vector that goes from left to right sides of rect2
			var aVec = new Vector2(size.x, -size.y + r.x + r.z);

			// Project vector aVec to wNorm to get magnitude of rect2 width vector
			var halfWidth = Vector2.Dot(aVec, wNorm) * .5f;
			rect2props.z = halfWidth;


			// Vector that goes from bottom to top sides of rect2
			var bVec = new Vector2(size.x, size.y - r.w - r.y);

			// Project vector bVec to hNorm to get magnitude of rect2 height vector
			var halfHeight = Vector2.Dot(bVec, hNorm) * .5f;
			rect2props.w = halfHeight;


			// Vector that goes from left to top sides of rect2
			var efVec = new Vector2(size.x - r.x - r.y, 0);

			// Vector that goes from point E to point G, which is top-left of rect2
			var egVec = hNorm * Vector2.Dot(efVec, hNorm);

			// Position of point E relative to center of coord system
			var ePoint = new Vector2(r.x - (size.x / 2), size.y / 2);

			// Origin of rect2 relative to center of coord system
			// ePoint + egVec == vector to top-left corner of rect2
			// wNorm * halfWidth + hNorm * -halfHeight == vector from top-left corner to center
			var origin = ePoint + egVec + wNorm * halfWidth + hNorm * -halfHeight;
			rect2props.x = origin.x;
			rect2props.y = origin.y;
		}
	}
}