using UnityEngine;
using System.Collections;

public class Root : MonoBehaviour
{
	public MeshRenderer mesh_renderer;
	public Material material;

    public Color original_colour;
	
	void Start () {
		mesh_renderer = gameObject.GetComponent<MeshRenderer>();
        tag = "Creature";
	}

	public void setColour (Color c)
    {
        original_colour = c;
		mesh_renderer = gameObject.GetComponent<MeshRenderer>();
        mesh_renderer.material.shader = Shader.Find("Legacy Shaders/Diffuse");
		mesh_renderer.material.color = c;
	}
	
	public void setScale (Vector3 scale)
    {
		transform.localScale = scale;	
	}
	
}
