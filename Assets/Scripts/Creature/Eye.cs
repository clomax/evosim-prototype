using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eye : MonoBehaviour {
	public IList objects;
	Creature crt;
	Foodbit fbit;
	GameObject trigger;
	public Creature closestCrt = null;
	public GameObject closestFbit = null;
	CollisionMediator co;
	float curr_dist = 0f;
	int crt_mate_range = 30;
	int fb_eat_range = 20;
	
	Creature other_crt;
	
	void Start () {
		crt = transform.parent.gameObject.GetComponent<Creature>();
		co = CollisionMediator.getInstance();
		objects = new List<GameObject>();
		
		trigger = new GameObject("Trigger");
		trigger.transform.parent = transform;
		trigger.transform.localPosition = Vector3.zero;
		SphereCollider sp = trigger.AddComponent<SphereCollider>();
		sp.isTrigger = true;
		sp.radius = crt.line_of_sight;
		
		StartCoroutine("closestCreature");
		StartCoroutine("closestFoodbit");
	}
	
	void OnTriggerEnter (Collider c) {
		if (c.tag == "Foodbit" || c.tag == "Creature")
			objects.Add(c.gameObject);
	}
	
	void OnTriggerExit (Collider c) {
		objects.Remove(c.gameObject);
	}
	
	IEnumerator closestCreature () {
		while(true) {
			closestCrt = null;
			GameObject closest = null;
			float dist = crt.line_of_sight;
			IEnumerator e = objects.GetEnumerator();
			while(e.MoveNext()) {
				GameObject c = (GameObject) e.Current;;
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
			if (closest)
				closestCrt = closest.GetComponent<Creature>();
			
			yield return new WaitForSeconds(0.01f);
		}
	}
	
	IEnumerator closestFoodbit () {
		while(true) {
			closestFbit = null;
			GameObject closest = null;
			float dist = crt.line_of_sight;
			IEnumerator e = objects.GetEnumerator();
			while(e.MoveNext()) {
				GameObject f = (GameObject) e.Current;
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
			if (closest)
				closestFbit = closest.gameObject;
			
			yield return new WaitForSeconds(0.01f);

		}
	}
}
