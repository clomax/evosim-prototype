using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eye : MonoBehaviour {
	Creature crt;
	Foodbit fbit;
	public Creature closestCrt 		= null;
	public GameObject closestFbit	= null;
	CollisionMediator co;
	float curr_dist 				= 0f;
	int crt_mate_range 				= 30;
	int fb_eat_range 				= 20;
	
	public Collider[] cs;
	
	Transform _t;
	
	Creature other_crt;
	
	void Start () {
		_t = transform;
		
		crt = _t.parent.parent.gameObject.GetComponent<Creature>();
		co = CollisionMediator.getInstance();
		
		InvokeRepeating("closestCreature",0,0.1F);
		InvokeRepeating("closestFoodbit",0,0.1F);
	}
	
	void closestCreature () {
		closestCrt 				= null;
		GameObject closest 		= null;
		GameObject c 			= null; // current collider
		double dist 			= crt.line_of_sight;
		
		if (crt.state == Creature.State.persuing_mate) {
			cs = Physics.OverlapSphere(_t.position, (float)dist);
			
			foreach (Collider col in cs) {
				c = (GameObject) col.transform.gameObject;
				if (c && c.gameObject.name == "root" && c != crt.root.gameObject) {
					Vector3 diff = c.transform.position - _t.position;
					curr_dist = diff.magnitude;
					if (curr_dist < dist) {
						closest = c.transform.parent.gameObject;
						dist = curr_dist;
					}
					if (curr_dist < crt_mate_range) {
						other_crt = c.transform.parent.GetComponent<Creature>();
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
		}	
	}
	
	void closestFoodbit () {
		closestFbit 		= null;
		GameObject closest 	= null;
		double dist 		= crt.line_of_sight;
		
		if (crt.state == Creature.State.hungry) {
			cs = Physics.OverlapSphere(_t.position, (float)dist);
			
			foreach (Collider c in cs) {
				GameObject f = (GameObject) c.gameObject;
				if (f && f.name == "Foodbit") {
					Vector3 diff = f.transform.position - _t.position;
					float curr_dist = diff.magnitude;
					if (curr_dist < dist) {
						closest = f;
						dist = curr_dist;
					}
					if (curr_dist < fb_eat_range && crt.state == Creature.State.hungry) {
						fbit = f.GetComponent<Foodbit>();
						crt.addEnergy(fbit.getEnergy());
						fbit.destroy();
					}
				}
			}
			
			if (closest)
				closestFbit = closest.gameObject;
			
		}
	}
}
