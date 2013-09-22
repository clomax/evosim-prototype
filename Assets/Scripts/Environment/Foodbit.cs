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
	
	double energy;
	float decay_amount;
	float decay_time;
	float decay_rate;
	
	void Start () {
		name = "Foodbit";
		settings = Settings.getInstance();
		energy = 				(double) settings.contents["ether"]["foodbit_energy"];
		decay_amount = 			float.Parse(settings.contents["foodbit"]["decay_amount"].ToString() );
		decay_time = 			float.Parse(settings.contents["foodbit"]["decay_time"].ToString() );
		decay_rate = 			float.Parse(settings.contents["foodbit"]["decay_rate"].ToString() );
		
		mr = (MeshRenderer)gameObject.AddComponent("MeshRenderer");
		mr.material = (Material)Resources.Load("Materials/Foodbit");
		eth = Ether.getInstance();
		Collider co = GetComponent<BoxCollider>();
		co.isTrigger = true;
		
		InvokeRepeating("decay",decay_time,decay_time);
	}
	
	void decay () {
		if (rnd.NextDouble() < (double)decay_rate) {
			energy -= decay_amount;
			eth.addToEnergy(decay_amount);
			if (energy <= 0)
				destroy();
		}
	}
	
	public double getEnergy() {
		return energy;
	}
	
	public void destroy () {
		eth.removeFoodbit(this.gameObject);
		Destroy(gameObject);
	}



}
