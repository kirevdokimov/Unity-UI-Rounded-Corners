using Nobi.UiRoundedCorners;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Nobi.UiRoundedCorners {
    [ExecuteInEditMode]								//Required to do validation with OnEnable()
    [DisallowMultipleComponent]                     //You can only have one of these in every object
    [RequireComponent(typeof(RectTransform))]
	public class ImageWithIndependentRoundedCorners : MonoBehaviour {
		private static readonly int prop_halfSize = Shader.PropertyToID("_halfSize");
		private static readonly int prop_radiuses = Shader.PropertyToID("_r");
		private static readonly int prop_rect2props = Shader.PropertyToID("_rect2props");

		// Vector2.right rotated clockwise by 45 degrees
		private static readonly Vector2 wNorm = new Vector2(.7071068f, -.7071068f);
		// Vector2.right rotated counter-clockwise by 45 degrees
		private static readonly Vector2 hNorm = new Vector2(.7071068f, .7071068f);

        public Vector4 r = new Vector4(40f, 40f, 40f, 40f);
        private Material material;

		// xy - position,
		// zw - halfSize
		[HideInInspector, SerializeField] private Vector4 rect2props;
		[HideInInspector, SerializeField] private MaskableGraphic image;

		private void OnValidate() {
			Validate();
			Refresh();
		}

		private void OnEnable() {
            //You can only add either ImageWithRoundedCorners or ImageWithIndependentRoundedCorners
			//It will replace the other component when added into the object.
            var other = GetComponent<ImageWithRoundedCorners>();
            if (other != null)
            {
                r = Vector4.one * other.radius;		//When it does, transfer the radius value to this script
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

		private void OnDestroy() {
            image.material = null;      //This makes so that when the component is removed, the UI material returns to null

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

/// <summary>
/// Display Vector4 as 4 separate fields for each corners.
/// It's way easier to use than w,x,y,z in Vector4.
/// </summary>
#if UNITY_EDITOR 
[CustomEditor(typeof(ImageWithIndependentRoundedCorners))]
public class Vector4Editor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        serializedObject.Update();

        SerializedProperty vector4Prop = serializedObject.FindProperty("r");

        EditorGUILayout.PropertyField(vector4Prop.FindPropertyRelative("x"), new GUIContent("Top Left Corner"));
        EditorGUILayout.PropertyField(vector4Prop.FindPropertyRelative("y"), new GUIContent("Top Right Corner"));
        EditorGUILayout.PropertyField(vector4Prop.FindPropertyRelative("w"), new GUIContent("Bottom Left Corner"));
        EditorGUILayout.PropertyField(vector4Prop.FindPropertyRelative("z"), new GUIContent("Bottom Right Corner"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif