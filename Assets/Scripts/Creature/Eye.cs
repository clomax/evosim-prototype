using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eye : MonoBehaviour {
	Creature crt;
	Foodbit fbit;
	public Creature targetCrt 		= null;
	public GameObject targetFbit	= null;
	CollisionMediator co;
	public float curr_dist 			= 0f;
	double crt_mate_range;
	double fb_eat_range;
	float eye_refresh_rate;
	double los;
	
	public Collider[] cs;
	
	Transform _t;
	
	Settings settings;
	Creature other_crt;

	public GameObject goal = null;
	public float distance_to_goal = 0F;
	Transform root;
	
	void Start () {
		_t = transform;
		
		crt = _t.parent.parent.gameObject.GetComponent<Creature>();
		co = CollisionMediator.getInstance();
		settings = Settings.getInstance();
		
		crt_mate_range =	(double) settings.contents["creature"]["mate_range"];
		fb_eat_range = 		(double) settings.contents["creature"]["eat_range"];
		eye_refresh_rate =	float.Parse( settings.contents["creature"]["eye_refresh_rate"].ToString() );
		los = crt.line_of_sight;

		root = _t.parent;

		InvokeRepeating("refreshVision",0,eye_refresh_rate);
	}

	void refreshVision () {
		switch (crt.state) {
		case Creature.State.persuing_mate:
			most_similar_creature();
			break;
		case Creature.State.searching_for_mate:
			most_similar_creature();
			break;
		case Creature.State.persuing_food:
			closestFoodbit();
			break;
		case Creature.State.searching_for_food:
			closestFoodbit();
			break;
		}
	}
	
	void most_similar_creature () {
		targetCrt 				= null;	// reference to the script of the closest creature
		GameObject target 		= null;
		GameObject c 			= null; // current collider being looked at
		float similarity		= Mathf.Infinity;
		float curr_similarity;
		cs = Physics.OverlapSphere(_t.position, (float)los);


		if (cs.Length == 0) {
			target = null;
			return;
		}

		foreach (Collider col in cs)
        {
			c = (GameObject) col.transform.gameObject;
			if (c && c.gameObject.name == "root" && c != crt.root.gameObject)
            {
				other_crt = c.transform.parent.GetComponent<Creature>();
				curr_similarity = GeneticsUtils.similar_colour (crt.chromosome, other_crt.chromosome);

				if (curr_similarity < similarity)
                {
					target = c.transform.parent.gameObject;
					similarity = curr_similarity;
				}

				Vector3 diff = c.transform.position - _t.position;
				if (diff.magnitude < (float)crt_mate_range)
                {
					other_crt = c.transform.parent.GetComponent<Creature>();
					Genitalia other_genital = other_crt.genital.GetComponent<Genitalia>();
					if (crt.state == Creature.State.persuing_mate || other_crt.state == Creature.State.persuing_mate)
                    {
						co.observe(crt.genital.gameObject, other_genital.gameObject);
						other_crt.ChangeState(Creature.State.mating);
                        crt.ChangeState(Creature.State.mating);
					}
					similarity = curr_similarity;
				}
			}

			distance_to_goal = 0F;
			goal = null;
			if (target)  {
				targetCrt = target.GetComponent<Creature>();
				goal = targetCrt.root;
				distance_to_goal = distanceToGoal();
			}
		}
	}
	
	void closestCreature () {
		targetCrt 				= null;	// reference to the script of the closest creature
		GameObject target 		= null;
		GameObject c 			= null; // current collider being looked at
		float dist 				= Mathf.Infinity;
		cs = Physics.OverlapSphere(_t.position, (float)los);

		if (cs.Length == 0) {
			target = null;
			return;
		}

		foreach (Collider col in cs) {
			c = (GameObject) col.transform.gameObject;
			if (c && c.gameObject.name == "root" && c != crt.root.gameObject) {
				Vector3 diff = c.transform.position - _t.position;
				curr_dist = diff.magnitude;
				other_crt = c.transform.parent.GetComponent<Creature>();


				if (curr_dist < dist) {
					target = c.transform.parent.gameObject;
					dist = curr_dist;
				}
				if (curr_dist < (float)crt_mate_range) {
					other_crt = c.transform.parent.GetComponent<Creature>();
					Genitalia other_genital = other_crt.genital.GetComponent<Genitalia>();
					if (crt.state == Creature.State.persuing_mate || other_crt.state == Creature.State.persuing_mate) {
						co.observe(crt.genital.gameObject, other_genital.gameObject);
						other_crt.ChangeState(Creature.State.mating);
						crt.ChangeState(Creature.State.mating);
					}
					dist = curr_dist;
				}
			}

			distance_to_goal = 0F;
			goal = null;
			if (target)  {
				targetCrt = target.GetComponent<Creature>();
				goal = targetCrt.root;
				distance_to_goal = distanceToGoal();
			}
		}	
	}
	
	void closestFoodbit () {
		targetFbit 		= null;	// reference to the script of the closest foodbit
		GameObject closest 	= null;
		float dist 			= Mathf.Infinity;
		cs = Physics.OverlapSphere(_t.position, (float)los);

		foreach (Collider c in cs) {
			GameObject f = (GameObject) c.gameObject;
			if (f && f.name == "Foodbit") {
				Vector3 diff = f.transform.position - _t.position;
				float curr_dist = diff.magnitude;
				if (curr_dist < dist) {
					closest = f;
					dist = curr_dist;
				}
				if (curr_dist < (float)fb_eat_range && crt.state == Creature.State.persuing_food) {
					fbit = f.GetComponent<Foodbit>();
					crt.energy += fbit.energy;
					fbit.destroy ();
					crt.food_eaten++;
				}
			}
		}

		distance_to_goal = 0F;
		if (closest) {
			targetFbit = closest.gameObject;
		}

		goal = targetFbit;
		if (goal) {
			distance_to_goal = distanceToGoal();
		}
	}

	public float distanceToGoal () {
		if (goal)
			return Vector3.Distance(root.position, goal.transform.position);
		else
			return 0F;
	}
}
