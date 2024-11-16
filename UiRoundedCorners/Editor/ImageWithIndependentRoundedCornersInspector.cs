using UnityEditor;
using UnityEngine.UI;

namespace Nobi.UiRoundedCorners.Editor {
    [CustomEditor(typeof(ImageWithIndependentRoundedCorners)), CanEditMultipleObjects]
    public class ImageWithIndependentRoundedCornersInspector : UnityEditor.Editor {
        private ImageWithIndependentRoundedCorners script;

        private void OnEnable() {
            script = (ImageWithIndependentRoundedCorners)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (!script.TryGetComponent<MaskableGraphic>(out var _)) {
                EditorGUILayout.HelpBox("This script requires an MaskableGraphic (Image or RawImage) component on the same gameobject", MessageType.Warning);
            }
        }
    }
}
