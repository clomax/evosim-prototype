using UnityEngine;
using System.Collections;

public class Root : MonoBehaviour {
	
	Transform _t;
	
	GameObject root;
	
	public Creature crt;	
	public GameObject eye;
	public GameObject mouth;
	public GameObject genital;
	
	public MeshRenderer mr;
	public Material mt;

	private Selectable s;
	
	void Start () {
		_t = transform;
		
		mr = gameObject.GetComponent<MeshRenderer>();
		
		crt = _t.parent.gameObject.GetComponent<Creature>();
		eye = crt.eye;
		mouth = crt.mouth;
		genital = crt.genital;

		s = crt.GetComponent<Selectable>();
	}

	void OnMouseDown () {
		s.select(_t.parent.gameObject);
	}

	public void setColour (Color c) {
		mr = gameObject.GetComponent<MeshRenderer>();
        mr.material.shader = Shader.Find("Legacy Shaders/Diffuse");
		mr.material.color = c;
	}
	
	public void setScale (Vector3 scale) {
		transform.localScale = scale;	
	}
	
}
