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
	double crt_mate_range;
	double fb_eat_range;
	float eye_refresh_rate;
	double dist;
	
	public Collider[] cs;
	
	Transform _t;
	
	Settings settings;
	Creature other_crt;
	
	void Start () {
		_t = transform;
		
		crt = _t.parent.parent.gameObject.GetComponent<Creature>();
		co = CollisionMediator.getInstance();
		settings = Settings.getInstance();
		
		crt_mate_range =	(double) settings.contents["creature"]["mate_range"];
		fb_eat_range = 		(double) settings.contents["creature"]["eat_range"];
		eye_refresh_rate =	float.Parse( settings.contents["creature"]["eye_refresh_rate"].ToString() );
		dist = crt.line_of_sight;

		InvokeRepeating("refreshVision",0,eye_refresh_rate);
	}

	void refreshVision () {
		cs = Physics.OverlapSphere(_t.position, (float)dist);
		if (crt.state == Creature.State.pursuing_mate)
			closestCreature(cs);
		if (crt.state == Creature.State.hungry)
			closestFoodbit(cs);
	}
	
	void closestCreature (Collider[] cs) {
		closestCrt 				= null;	// reference to the script of the closest creature
		GameObject closest 		= null;
		GameObject c 			= null; // current collider being looked at

		foreach (Collider col in cs) {
			c = (GameObject) col.transform.gameObject;
			if (c && c.gameObject.name == "root" && c != crt.root.gameObject) {
				Vector3 diff = c.transform.position - _t.position;
				curr_dist = diff.magnitude;
				if (curr_dist < dist) {
					closest = c.transform.parent.gameObject;
				}
				if (curr_dist < (float)crt_mate_range) {
					other_crt = c.transform.parent.GetComponent<Creature>();
					Genitalia other_genital = other_crt.genital.GetComponent<Genitalia>();
					if (crt.state == Creature.State.pursuing_mate || other_crt.state == Creature.State.pursuing_mate) {
						co.observe(crt.genital.gameObject, other_genital.gameObject);
						other_crt.state = Creature.State.mating;
						crt.state = Creature.State.mating;
					}
				}
				dist = curr_dist;
			}

			if (closest)
				closestCrt = closest.GetComponent<Creature>();
		}	
	}
	
	void closestFoodbit (Collider[] cs) {
		closestFbit 		= null;	// reference to the script of the closest foodbit
		GameObject closest 	= null;

		foreach (Collider c in cs) {
			GameObject f = (GameObject) c.gameObject;
			if (f && f.name == "Foodbit") {
				Vector3 diff = f.transform.position - _t.position;
				float curr_dist = diff.magnitude;
				if (curr_dist < dist) {
					closest = f;
				}
				if (curr_dist < (float)fb_eat_range && crt.state == Creature.State.hungry) {
					fbit = f.GetComponent<Foodbit>();
					crt.addEnergy(fbit.energy);
					fbit.destroy ();
				}
				dist = curr_dist;
			}
		}
		
		if (closest)
			closestFbit = closest.gameObject;
		
	}
}
