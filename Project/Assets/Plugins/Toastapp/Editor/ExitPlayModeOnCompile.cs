#if UNITY_EDITOR
// Cape Guy, 2015. Use at your own risk.

using UnityEngine;
using UnityEditor;

/// <summary>
/// This script exits play mode whenever script compilation is detected during an editor update.
/// </summary>
// Make static initialiser be called as soon as the scripts are initialised in the editor (rather than just in play mode).
[InitializeOnLoad]
public class ExitPlayModeOnScriptCompile {

    // Static initialiser called by Unity Editor whenever scripts are loaded (editor or play mode)
    static ExitPlayModeOnScriptCompile() {
        Unused(_instance);
        _instance = new ExitPlayModeOnScriptCompile();
#pragma warning disable CS0618 // Type or member is obsolete
		EditorApplication.playmodeStateChanged += OnPlayModeChanged;
#pragma warning restore CS0618 // Type or member is obsolete
	}

    static void OnPlayModeChanged() {
        if(!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode) {
            Debug.Log("Start AssetDatabase.Refresh() before playing");
            AssetDatabase.Refresh();
        }
    }

    private ExitPlayModeOnScriptCompile() {
        EditorApplication.update += OnEditorUpdate;
    }

    ~ExitPlayModeOnScriptCompile() {
        EditorApplication.update -= OnEditorUpdate;
        // Silence the unused variable warning with an if.
        _instance = null;
    }

    // Called each time the editor updates.
    private static void OnEditorUpdate() {
        if(EditorApplication.isPlaying && EditorApplication.isCompiling) {
            Debug.Log("Exiting play mode due to script compilation.");
            EditorApplication.isPlaying = false;
        }
    }

    // Used to silence the 'is assigned by its value is never used' warning for _instance.
    private static void Unused<T>(T unusedVariable) {
    }

    private static ExitPlayModeOnScriptCompile _instance = null;

}
#endif