using UnityEngine;

public class UpdatableData : ScriptableObject
{
	public event System.Action OnValuesUpdated;	//function to call when values change
	public bool autoUpdate;

	protected virtual void OnValidate()	
	{
		if(autoUpdate)
		{
			UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;	//this allows the function to be called after the shader recompiles, so that
																			//the shader values don't reset the mesh to white
		}
	}

	public void NotifyOfUpdatedValues()
	{
		UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;	//remove it so that it isn't called every frame
		if(OnValuesUpdated != null)
		{
			OnValuesUpdated();
		}
	}
}
