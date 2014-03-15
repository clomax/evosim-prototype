using UnityEngine;
using System.Collections;

public class Limb : MonoBehaviour {
	
	MeshRenderer mr;
	Color c;
	Vector3 position;
	Vector3 scale;
	
	public GameObject parent;
	
	HingeJoint hj;
	
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
	
	
	public void setRecurrances (int r) {
		recurrances = r;
	}
	
	public int getRecurrances () {
		return recurrances;
	}
	
	public void setJoint (Vector3 axis, Vector3 anchor, Rigidbody connected_body) {
		hj = gameObject.AddComponent<HingeJoint>();
		hj.axis = axis;
		hj.anchor = anchor;
		hj.connectedBody = connected_body;

	}
	
	public void setMotor (float force, float target_vel) {
		JointMotor m = new JointMotor();
		m.force = force;
		m.targetVelocity = target_vel;
		hj.motor = m;
	}
}
