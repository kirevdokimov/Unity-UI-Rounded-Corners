using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public abstract class BaseImageWithRoundedCorners : MonoBehaviour
{
    private void Start()
    {
        Refresh();
    }

    void OnRectTransformDimensionsChange()
    {
        Refresh();
    }

    private void OnValidate()
    {
        Refresh();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }
        Refresh();
    }
#endif

    protected abstract void Refresh();
}
