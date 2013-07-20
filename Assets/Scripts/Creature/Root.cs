using UnityEngine;
using System.Collections;

public class Root : MonoBehaviour {
	
	Transform _t;
	
	GameObject root;
	
	public Creature crt;	
	public GameObject eye;
	public GameObject mouth;
	public GameObject genital;
	
	MeshRenderer mr;
	Material mat;

	void Start () {
		_t = transform;
		
		crt = _t.parent.gameObject.GetComponent<Creature>();
		eye = crt.eye;
		mouth = crt.mouth;
		genital = crt.genital;

		mr = this.gameObject.GetComponent<MeshRenderer>();
		mat = (Material)Resources.Load("Materials/creature");
		mr.material = mat;
	}
}
