using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : UpdatableData
{
	public float noiseScale;

	public int octaves;
	[Range(0f, 1f)]
	public float persistence;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	protected override void OnValidate()   //called automatically when a value is changed in the editor
	{
		if (lacunarity < 1f)
		{
			lacunarity = 1f;
		}

		if (octaves < 0)
		{
			octaves = 0;
		}

		base.OnValidate();
	}
}
