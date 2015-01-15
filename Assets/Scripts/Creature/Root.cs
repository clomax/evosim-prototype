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

	private Eye eye_script;



	public Vector3 direction;




	void Start () {
		_t = transform;
		
		mr = gameObject.GetComponent<MeshRenderer>();
		
		crt = _t.parent.gameObject.GetComponent<Creature>();
		eye = crt.eye;
		mouth = crt.mouth;
		genital = crt.genital;

		s = crt.GetComponent<Selectable>();

		eye_script = eye.GetComponent<Eye>();

		InvokeRepeating("RandomDirection", 1F, 20F);
	}

	void FixedUpdate () {
		if(eye_script.goal) {
			direction = (eye_script.goal.transform.position - transform.position).normalized;
		}
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
		rigidbody.AddForce(direction * 15F);
	}

	void OnMouseDown () {
		s.select(_t.parent.gameObject);
	}

	public void setColour (Color c) {
		mr = gameObject.GetComponent<MeshRenderer>();
		mr.material.color = c;
	}
	
	public void setScale (Vector3 scale) {
		transform.localScale = scale;	
	}

	private void RandomDirection () {
		direction = new Vector3 (Random.Range (-1F,1F), Random.Range(-1F,1F), Random.Range(-1F,1F));
	}
}
