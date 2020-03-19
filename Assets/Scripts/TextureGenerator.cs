using UnityEngine;

public static class TextureGenerator
{
	public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)	//given colour map, creates texture
	{
		Texture2D texture = new Texture2D(width, height);
		texture.filterMode = FilterMode.Point;  //to reduce blurriness around pixels
		texture.wrapMode = TextureWrapMode.Clamp;	//prevents thin line of opposite edge appearing on end, since by default textures wrap around
		texture.SetPixels(colourMap);	//sets pixel colours
		texture.Apply();	//applies to texture
		return texture;
	}

	public static Texture2D TextureFromHeightMap(float[,] heightMap)
	{
		int width = heightMap.GetLength(0);  //.GetLength returns length of specified dimension
		int height = heightMap.GetLength(1);

		Texture2D texture = new Texture2D(width, height);   //2D texture that is displayed on plane

		Color[] colorMap = new Color[width * height];   //array of colours to set pixels to

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]); //shades of grey distinguish noise values
			}
		}
		return TextureFromColourMap(colorMap, width, height);
	}
}
