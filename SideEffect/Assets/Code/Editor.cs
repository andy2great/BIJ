using UnityEngine;
using UnityEditor;

public delegate void StopHandler();

// [InitializeOnLoadAttribute]
public static class Editor
{
    public static event StopHandler stop;

    // register an event handler when the class is initialized
    // static Editor()
    // {
        // EditorApplication.playModeStateChanged += PlayModeStateChanged;
    // }

    // private static void PlayModeStateChanged(PlayModeStateChange state)
    // {
        // if (state == PlayModeStateChange.ExitingPlayMode)
        // {
            // Editor.stop?.Invoke();
        // }
    // }
}
