using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Helper class for instantiating ScriptableObjects.
/// </summary>
public class ScriptableObjectFactory
{
	[MenuItem("Project/Create/ScriptableObject")]
	[MenuItem("Assets/Create/ScriptableObject")]
	public static void Create()
	{
        var firstPass_assembly = GetFirstPassAssembly();
		var secondPass_assembly = GetSecondPassAssembly ();


        IEnumerable<Type> secondPassAssemblyTypes = (from t in secondPass_assembly.GetTypes() where t.IsSubclassOf(typeof(ScriptableObject)) select t);
        IEnumerable<Type> firstPassAssemblyTypes = (from t in firstPass_assembly.GetTypes() where t.IsSubclassOf(typeof(ScriptableObject)) select t);


        // Get all classes derived from ScriptableObject
        var allScriptableObjects = secondPassAssemblyTypes.Concat(firstPassAssemblyTypes).ToArray();

        // Show the selection window.
        var window = EditorWindow.GetWindow<ScriptableObjectWindow>(true, "Create a new ScriptableObject", true);
		window.ShowPopup();

		window.Types = allScriptableObjects;
	}

	/// <summary>
	/// Returns the assembly that contains the script code for this project (currently hard coded)
	/// </summary>
	private static Assembly GetSecondPassAssembly ()
	{
		return Assembly.Load (new AssemblyName ("Assembly-CSharp"));
	}

    /// <summary>
    /// Returns the assembly that contains the script code for this project (currently hard coded)
    /// </summary>
    private static Assembly GetFirstPassAssembly() {
        return Assembly.Load(new AssemblyName("Assembly-CSharp-firstpass"));
    }
}
