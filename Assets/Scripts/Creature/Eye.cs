using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour {
	static int MAX_SIZE = 50;
	GameObject[] objects;
	public Creature crt;
	public GameObject trigger;
	public Creature closestCrt = null;
	private CollisionMediator co;
	private float curr_dist = 0f;
	private int crt_mate_range = 20;
	
	public Creature other_crt;
	
	void Start () {
		objects = new GameObject[MAX_SIZE];
		crt = transform.parent.gameObject.GetComponent<Creature>();
		co = CollisionMediator.getInstance();
		
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
		closestCrt = closestCreature();
	}
	
	private Creature closestCreature () {
		GameObject closest = null;
		float dist = crt.line_of_sight;
		Vector3 pos = transform.position;
		foreach(GameObject c in objects) {
			if (c && c.tag == "Creature" && c != crt.gameObject) {
				Vector3 diff = c.transform.position - pos;
				curr_dist = diff.magnitude;
				if (curr_dist < dist) {
					closest = c;
					dist = curr_dist;
				}
				if (curr_dist < crt_mate_range) {
					other_crt = c.gameObject.GetComponent<Creature>();
					Genitalia other_genital = other_crt.genital.GetComponent<Genitalia>();
					if (this.crt.state == Creature.State.persuing_mate || other_crt.state == Creature.State.persuing_mate) {
						co.observe(crt.genital.gameObject, other_genital.gameObject);
						other_crt.state = Creature.State.mating;
						this.crt.state = Creature.State.mating;
					}
					dist = curr_dist;
				}
			}
		}
		if (closest)
			return closest.GetComponent<Creature>();
		return null;
	}	
}
