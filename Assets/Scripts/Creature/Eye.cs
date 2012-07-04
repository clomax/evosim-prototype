using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour {
	static int MAX_SIZE = 50;
	GameObject[] objects;
	public Creature crt;
	public Foodbit fbit;
	public GameObject trigger;
	public Creature closestCrt = null;
	public GameObject closestFbit = null;
	private CollisionMediator co;
	private float curr_dist = 0f;
	private int crt_mate_range = 30;
	private int fb_eat_range = 20;
	
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
		closestFbit = closestFoodbit();
	}
	
	private Creature closestCreature () {
		GameObject closest = null;
		float dist = crt.line_of_sight;
		foreach(GameObject c in objects) {
			if (c && c.tag == "Creature" && c != crt.gameObject) {
				Vector3 diff = c.transform.position - transform.position;
				curr_dist = diff.magnitude;
				if (curr_dist < dist) {
					closest = c;
					dist = curr_dist;
				}
				if (curr_dist < crt_mate_range) {
					other_crt = c.gameObject.GetComponent<Creature>();
					Genitalia other_genital = other_crt.genital.GetComponent<Genitalia>();
					if (crt.state == Creature.State.persuing_mate || other_crt.state == Creature.State.persuing_mate) {
						co.observe(crt.genital.gameObject, other_genital.gameObject);
						other_crt.state = Creature.State.mating;
						crt.state = Creature.State.mating;
					}
					dist = curr_dist;
				}
			}
		}
		if (closest) return closest.GetComponent<Creature>();
		return null;
	}
	
	private GameObject closestFoodbit () {
		GameObject closest = null;
		float dist = crt.line_of_sight;
		foreach(GameObject f in objects) {
			if (f && f.tag == "Foodbit") {
				Vector3 diff = f.transform.position - transform.position;
				float curr_dist = diff.magnitude;
				if (curr_dist < dist) {
					closest = f;
					dist = curr_dist;
				}
				if (curr_dist < fb_eat_range && crt.state == Creature.State.hungry) {
					fbit = f.GetComponent<Foodbit>();
					crt.eat(fbit.getEnergy());
					fbit.destroy();
				}
			}
		}
		if (closest) return closest;
		return null;
	}
}
