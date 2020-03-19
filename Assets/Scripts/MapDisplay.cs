using UnityEngine;

public class MapDisplay : MonoBehaviour
{
	public Renderer textureRender;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	public void DrawTexture(Texture2D texture)
	{
		//we dont want to have to play to view changes
		textureRender.sharedMaterial.mainTexture = texture; //.sharedMaterial is seen in editor, unlike .material
		textureRender.transform.localScale = new Vector3(texture.width, 1f, texture.height);
	}

	public void DrawMesh(MeshData meshData)
	{
		meshFilter.sharedMesh = meshData.CreateMesh();
	}
}