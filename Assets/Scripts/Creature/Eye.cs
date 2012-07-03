using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour {
	static int MAX_SIZE = 50;
	GameObject[] objects;
	public Creature crt;
	public GameObject trigger;
	
	void Start () {
		objects = new GameObject[MAX_SIZE];
		crt = transform.parent.gameObject.GetComponent<Creature>();
		
		trigger = new GameObject("Trigger");
		trigger.transform.parent = transform;
		trigger.transform.localPosition = Vector3.zero;
		SphereCollider sp = trigger.AddComponent<SphereCollider>();
		sp.isTrigger = true;
		sp.radius = crt.line_of_sight;
	}
	
	void OnTriggerEnter (Collider c) {
		if (c.tag == "Foodbit" || c.tag == "Creature")
			for(int i=0; i<this.objects.Length; i++) {
			if (null == this.objects[i]) {
				this.objects[i] = c.gameObject;
				break;
			}
		}
	}
	
	void OnTriggerExit (Collider c) {
		for(int i=0; i<this.objects.Length; i++) {
			if (c.gameObject == this.objects[i]) {
				this.objects[i] = null;
				break;
			}
		}
	}
	
	void Update () {
		
	}
}
