using UnityEngine;
using System.Collections;

public class Limb : MonoBehaviour {

	MeshRenderer mr;
	HingeJoint hj;

	void Start () {
	}

	public void create (Color c, Vector3 p, Vector3 s) {
		mr = gameObject.AddComponent<MeshRenderer>();
		setColour(c);
		setPosition(p);
		setScale(s);
	}

	public Color getColour () {
		return mr.material.color;
	}

	public Vector3 getPosition () {
		return transform.localPosition;
	}

	public Vector3 getScale () {
		return transform.localScale;
	}
	
	public void setColour (Color c) {
		gameObject.GetComponent<MeshRenderer>().material.color = c;
	}
	
	public void setPosition (Vector3 p) {
		transform.localPosition = p;
	}
	
	public void setScale (Vector3 s) {
		transform.localScale = s;
	}

}
