using UnityEngine;
using System.Collections;

public class Limb : MonoBehaviour {
	
	Transform _t;
	MeshRenderer mr;
	
	void Start () {
		_t = transform;
		name = "limb";
		_t.localScale = new Vector3(5,1,1);
		
		//mr = this.gameObject.GetComponent<MeshRenderer>();
		//mr.material.color = Color.green;	
	}
}
