using UnityEngine;

public class UpdatableData : ScriptableObject
{
	public event System.Action OnValuesUpdated;	//function to call when values change
	public bool autoUpdate;

	protected virtual void OnValidate()	
	{
		if(autoUpdate)
		{
			NotifyOfUpdatedValues();
		}
	}

	public void NotifyOfUpdatedValues()
	{
		if(OnValuesUpdated != null)
		{
			OnValuesUpdated();
		}
	}
}
