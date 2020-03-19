using UnityEngine;

[CreateAssetMenu()]
public class TextureData : UpdatableData
{
	public Color[] baseColours;
	[Range(0f, 1f)]
	public float[] baseStartHeights;
	[Range(0f, 1f)]
	public float[] baseBlends;

	float savedMinHeight;
	float savedMaxHeight;

    public void ApplyToMaterial(Material material)
	{
		material.SetInt("baseColourCount", baseColours.Length);
		material.SetColorArray("baseColours", baseColours);
		material.SetFloatArray("baseStartHeights", baseStartHeights);
		material.SetFloatArray("baseBlends", baseBlends);
		UpdateMeshHeights(material, savedMinHeight, savedMaxHeight);
	}

	public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
	{
		savedMaxHeight = maxHeight;
		savedMinHeight = minHeight;
		material.SetFloat("minHeight", minHeight);
		material.SetFloat("maxHeight", maxHeight);
	}
}
