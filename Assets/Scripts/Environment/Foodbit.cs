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
	//MeshRenderer mr;
	
	System.Random rnd = new System.Random();
	
	double init_energy;
	public double energy;
	float decay_amount;
	float destroy_at;
	float decay_time;
	float decay_rate;

	void Start () {
		name = "Foodbit";
		settings = Settings.getInstance();
		init_energy = 			(double) settings.contents["foodbit"]["init_energy"];
		decay_amount = 			float.Parse(settings.contents["foodbit"]["decay_amount"].ToString() );
		decay_time = 			float.Parse(settings.contents["foodbit"]["decay_time"].ToString() );
		destroy_at = 			float.Parse(settings.contents["foodbit"]["destroy_at"].ToString() );
		decay_rate = 			float.Parse(settings.contents["foodbit"]["decay_rate"].ToString() );

		energy = init_energy;
		
		//mr = (MeshRenderer)gameObject.AddComponent("MeshRenderer");
		//mr.sharedMaterial = (Material)Resources.Load("Materials/Foodbit");
		eth = Ether.getInstance();
		Collider co = GetComponent<SphereCollider>();
		co.isTrigger = true;
		
		//InvokeRepeating("decay", decay_time, decay_time);
	}

	void decay () {
		if (rnd.NextDouble() < (double)decay_rate) {
			if (energy <= destroy_at) {
				eth.addToEnergy(energy);
				eth.removeFoodbit(gameObject);
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
