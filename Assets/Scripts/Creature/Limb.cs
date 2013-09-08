using UnityEngine;
using System.Collections;

public class Limb : MonoBehaviour {
	
	MeshRenderer mr;
	Color c;
	Vector3 position;
	Vector3 rotation;
	Vector3 scale;
	
	int recurrances;
	
	void Start () {
		gameObject.name = "limb";
	}
	
	public void setColour (Color c) {
		mr = gameObject.GetComponent<MeshRenderer>();
		mr.material.color = c;
	}
	
	public void setPosition (Vector3 p) {
		position = p;
		transform.localPosition = p;
	}
	
	public void setScale (Vector3 s) {
		scale = s;
		transform.localScale = s;
	}
	
	public void setRotation (Vector3 r) {
		rotation = r;
		transform.localEulerAngles = r;
	}
	
	public void setRecurrances (int r) {
		recurrances = r;
	}
}
