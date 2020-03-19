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

	public TerrainData terrainData;
	public NoiseData noiseData;
	public TextureData textureData;

	public Material terrainMaterial;

	public bool autoUpdate;
	void OnValuesUpdated()
	{
		if (!Application.isPlaying)
		{
			GenerateMap();
		}
	}

	void OnTextureValuesUpdated()
	{
		textureData.ApplyToMaterial(terrainMaterial);
	}

	public void GenerateMap()
	{
		float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistence,
													noiseData.lacunarity, noiseData.offset);    //generate noise map

		textureData.UpdateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
		
		MapDisplay display = FindObjectOfType<MapDisplay>();    //self explanatory
		if (drawMode == DrawMode.NoiseMap)
		{
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap)); //draw the visualisaton of the noise map
		}
		else if (drawMode == DrawMode.Mesh)
		{
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, 
																terrainData.uniformScale, levelOfDetail));//draw mesh
		}

	}

	private void OnValidate()
	{
		if (terrainData != null)
		{
			terrainData.OnValuesUpdated -= OnValuesUpdated; //this ensures each change we dont call this function many times
			terrainData.OnValuesUpdated += OnValuesUpdated;
		}

		if (noiseData != null)
		{
			noiseData.OnValuesUpdated -= OnValuesUpdated;
			noiseData.OnValuesUpdated += OnValuesUpdated;
		}

		if (textureData != null)
		{
			textureData.OnValuesUpdated -= OnTextureValuesUpdated;
			textureData.OnValuesUpdated += OnTextureValuesUpdated;
		}
	}


}