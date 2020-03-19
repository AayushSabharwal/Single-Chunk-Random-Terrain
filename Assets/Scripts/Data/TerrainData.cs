using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : UpdatableData
{
	public float meshHeightMultiplier;  //multiply height of each node to create terrain
	public AnimationCurve meshHeightCurve;  //curve that indicates how much heightMap values should be affected by multiplier. We want water to be less affected than hills

}
