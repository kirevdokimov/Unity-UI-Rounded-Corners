using UnityEditor;
using UnityEngine.UI;

namespace Nobi.UiRoundedCorners.Editor {
    [CustomEditor(typeof(ImageWithRoundedCorners)), CanEditMultipleObjects]
    public class ImageWithRoundedCornersInspector : UnityEditor.Editor {
        private ImageWithRoundedCorners script;

        private void OnEnable() {
            script = (ImageWithRoundedCorners)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (!script.TryGetComponent<MaskableGraphic>(out var _)) {
                EditorGUILayout.HelpBox("This script requires an MaskableGraphic (Image or RawImage) component on the same gameobject", MessageType.Warning);
            }
        }
    }
}
