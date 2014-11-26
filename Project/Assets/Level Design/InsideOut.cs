using UnityEngine;
using System.Collections;

public class InsideOut : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		int[] triangles = mesh.triangles;
		for (int i = 0; i < triangles.Length; i += 3) 
		{
			int temp = triangles[i];
			triangles[i] = triangles[i+2];
			triangles[i+2] = temp;
		}
		mesh.triangles = triangles;

		Vector3[] normals = mesh.normals;
		for (int i = 0; i < normals.Length; i++)
						normals [i] = -normals [i];
		mesh.normals = normals;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
