using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		31.08.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */



/*
 * Foodbit - representing each foodbit in the environment
 * 	params: 
 *				> int energy - given by the `ether` gameObject
 *	
 *		notes:
 *				> timeToGerminate, treeRadius and energy should be options read from a settings file
 */

public class Foodbit : MonoBehaviour {

	public static float foodbitHeight = 1.0F;
	
	Settings settings;
	Ether eth;
	MeshRenderer mr;
	
	System.Random rnd = new System.Random();
	
	double init_energy;
	public double energy;
	float decay_amount;
	float decay_time;
	float decay_rate;
	float spore_time;
	float spore_rate;
	
	void Start () {
		name = "Foodbit";
		settings = Settings.getInstance();
		init_energy = 			(double) settings.contents["foodbit"]["init_energy"];
		decay_amount = 			float.Parse(settings.contents["foodbit"]["decay_amount"].ToString() );
		decay_time = 			float.Parse(settings.contents["foodbit"]["decay_time"].ToString() );
		decay_rate = 			float.Parse(settings.contents["foodbit"]["decay_rate"].ToString() );
		spore_rate = 			float.Parse(settings.contents["foodbit"]["spore_rate"].ToString() );
		spore_time = 			float.Parse(settings.contents["foodbit"]["spore_time"].ToString() );

		energy = init_energy;
		
		mr = (MeshRenderer)gameObject.AddComponent("MeshRenderer");
		mr.material = (Material)Resources.Load("Materials/Foodbit");
		eth = Ether.getInstance();
		Collider co = GetComponent<BoxCollider>();
		co.isTrigger = true;
		
		InvokeRepeating("decay", decay_time, decay_time);
		InvokeRepeating("spore", spore_time, spore_time);
	}

	void spore () {
		if (rnd.NextDouble() < (double)spore_rate) {
			eth.newFoodbit(this.transform.localPosition);
		}
	}
	
	void decay () {
		if (rnd.NextDouble() < (double)decay_rate) {
			if (energy <= decay_amount) {
				eth.addToEnergy(energy);
				energy = 0;
				destroy();
			} else {
				energy -= decay_amount;
				eth.addToEnergy(decay_amount);
			}
		}
	}
	
	public void destroy () {
		eth.removeFoodbit(this.gameObject);
		Destroy(gameObject);
	}



}
