using UnityEngine;

public static class MeshGenerator
{
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
	{
		int width = heightMap.GetLength(0); //dimensions of array
		int height = heightMap.GetLength(1);
		float topLeftX = (width - 1) / -2f; //we want central location of mesh to be 0,0 so calculating x coord of top left point
		float topLeftZ = (height - 1) / 2f; //similarly

		int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2; //how much to increment by when iterating through points, so as to change detail
																						//manually clamp to 1
		int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;
		MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);    //data of current mesh
		int vertexIndex = 0;    //index of current vertex

		for (int y = 0; y < height; y += meshSimplificationIncrement) {  //looping through vertices
			for (int x = 0; x < width; x += meshSimplificationIncrement) {
				meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMap[x, y] * heightMultiplier, topLeftZ - y);  //add vertex to list
																																									   //topLeftX + x since we move towards positive from there. topLeftZ - y since we move down
																																									   //.Evaluate returns y for inputted x value on curve
				meshData.uvs[vertexIndex] = new Vector2(x / (float) width, y / (float) height);
				/*
				 * Consider mesh with points marked as integers
				 * 0 - 1 - 2
				 * | \ | \ |
				 * 3 - 4 - 5
				 * | \ | \ |
				 * 6 - 7 - 8
				 * at vertex 0 we set triangles of square 0,1,4,3, and so on. We do not need to do this for points on right and bottom edges
				 * for some square
				 * i   -   i+1
				 * |   \   |
				 * i+w -   i+w+1
				 * our triangles are i, i+w+1, i+w and i+w+1, i, i+1
				 */

				if (x < width - 1 && y < height - 1) {  //if we arent on right or bottom edge
					meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
					meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
				}
				vertexIndex++;
			}
		}
		return meshData;
	}
}

public class MeshData   //to store mesh info
{
	public Vector3[] vertices;  //list of vertices
	public int[] triangles; //list of vertices, except every 3 define a triangle in clockwise order. Obviously, duplicates allowed
	public Vector2[] uvs;   //uv map so we can colour our mesh
	int triangleIndex;

	public MeshData(int meshWidth, int meshHeight)
	{
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
	}

	public void AddTriangle(int a, int b, int c)
	{
		triangles[triangleIndex] = a;
		triangles[triangleIndex + 1] = b;
		triangles[triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;   //assigning vertices, triangles and uvs to our mesh
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();  //recalculate normals so that lighting works
		return mesh;
	}
}