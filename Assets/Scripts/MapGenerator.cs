using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public enum DrawMode { NoiseMap, ColourMap, Mesh }; //do we want to see the colour or noisemap or mesh when drawing
	public DrawMode drawMode;

	// we will divide mesh into chunks so that we can have larger terrains, since unity imposes restrictions on max vertices per mesh (255^2)
	const int mapChunkSize = 241;   //241 is chosen since it allows for us to choose a larger number of resolutions for a chunk
									//we will skip over points for a lower resolution, to render farther planes efficiently. If we consider every xth point, x must be a factor of
									//mapChunkSize - 1 and will result in (width - 1)/x + 1 points	
									//this replaces mapWidth and Height since each chunk will be a square with fixed number of points

	[Range(0, 6)]
	public int levelOfDetail;   //we clamp it to 0, 6 and multiply it by 2 for x. Hence, we get 0, 2, 4, 6, 8, 10, 12 which all are factors of 240
								//note that 0 will be manually clamped to 1 since we have to increment point by something

	public float noiseScale;

	public int octaves;
	[Range(0f, 1f)]
	public float persistence;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public float meshHeightMultiplier;  //multiply height of each node to create terrain
	public AnimationCurve meshHeightCurve;  //curve that indicates how much heightMap values should be affected by multiplier. We want water to be less affected than hills
	public bool autoUpdate;

	public TerrainType[] regions;

	public void GenerateMap()
	{
		float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistence, lacunarity, offset);    //generate noise map

		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];    //colourmap for pixels
		for (int y = 0; y < mapChunkSize; y++) { //looping through all points
			for (int x = 0; x < mapChunkSize; x++) {
				float currentHeight = noiseMap[x, y];
				for (int i = 0; i < regions.Length; i++) {    //looping through all regions
					if (currentHeight <= regions[i].height) { //if our pixel falls under current region's category
						colourMap[y * mapChunkSize + x] = regions[i].colour;    //give it the proper colour
						break;
					}
				}
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay>();    //self explanatory
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap)); //draw the visualisaton of the noise map
		}
		else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize)); //draw colours
		}
		else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));//draw mesh
		}



	}

	private void OnValidate()   //called automatically when a value is changed in the editor
	{
		if (lacunarity < 1f) {
			lacunarity = 1f;
		}

		if (octaves < 0) {
			octaves = 0;
		}
	}
}

[System.Serializable]   //so it comes up in inspector
public struct TerrainType   //used to colour our map. Specify what terrain has what colour at what height
{
	public string name;
	public float height;
	public Color colour;
}
