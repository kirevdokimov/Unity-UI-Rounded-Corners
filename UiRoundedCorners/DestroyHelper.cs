using UnityEngine;

internal static class DestroyHelper {
    internal static void Destroy(Object @object) {
#if UNITY_EDITOR
		if (Application.isPlaying) {
			Object.Destroy(@object);
		} else {
			Object.DestroyImmediate(@object);
		}
#else
		Object.Destroy(@object);
#endif
	}
}
