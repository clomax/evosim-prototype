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
	float foodbit_spawn_time;
	int	  fb_spawn_range;
	float decay_amount;
	float decay_time;
	float decay_rate;
	float request_rate;
	
	void Start () {
		name = "Foodbit";
		settings = Settings.getInstance();
		
		energy = 				(double) settings.contents["ether"]["foodbit_energy"];
		foodbit_spawn_time = 	float.Parse(settings.contents["ether"]["foodbit_spawn_time"].ToString() );
		decay_amount = 			float.Parse(settings.contents["foodbit"]["decay_amount"].ToString() );
		decay_time = 			float.Parse(settings.contents["foodbit"]["decay_time"].ToString() );
		decay_rate = 			float.Parse(settings.contents["foodbit"]["decay_rate"].ToString() );
		request_rate = 			float.Parse(settings.contents["foodbit"]["request_rate"].ToString() );
		
		mr = (MeshRenderer)gameObject.AddComponent("MeshRenderer");
		mr.material = (Material)Resources.Load("Materials/Foodbit");
		eth = Ether.getInstance();
		Collider co = GetComponent<BoxCollider>();
		co.isTrigger = true;
		
		//Debug.Log((float)foodbit_spawn_time);
		StartCoroutine(requestFoodbit());
		InvokeRepeating("decay",decay_time,decay_time);
	}
	
	void decay () {
		if (rnd.NextDouble() < (double)decay_rate) {
			energy -= decay_amount;
			if (energy <= 0)
				destroy();
		}
	}
	
	IEnumerator requestFoodbit () {
		while (true) {
			if (rnd.NextDouble() < (double)request_rate)
				eth.newFoodbit(transform.localPosition);
			
			yield return new WaitForSeconds(foodbit_spawn_time);
		}
	}
	
	public double getEnergy() {
		return energy;
	}
	
	public void destroy () {
		eth.foodbit_count--;
		Destroy(gameObject);
	}



}
