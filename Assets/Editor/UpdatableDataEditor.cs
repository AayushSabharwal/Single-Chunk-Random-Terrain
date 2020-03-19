using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpdatableData), true)]		// true just says we want this to work for classes that derive from UpdatableData
public class UpdatableDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		UpdatableData data = (UpdatableData) target;

		if (GUILayout.Button("Update"))
		{
			data.NotifyOfUpdatedValues();
			EditorUtility.SetDirty(target);
		}
	}
}
