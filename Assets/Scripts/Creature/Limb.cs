using UnityEngine;
using System.Collections;

public class Limb : MonoBehaviour {

	MeshRenderer mr;
	HingeJoint hj;

	void Start () {
	}

	public void create (Color c, Vector3 p, Vector3 s, Vector3 axis) {
		mr = gameObject.AddComponent<MeshRenderer>();
		setColour(c);
		setPosition(p);
		setScale(s);
		setJoint (axis, new Vector3(0f,0f,0.5f), null);
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
