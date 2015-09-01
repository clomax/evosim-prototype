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

	double init_energy;
	public double energy;
	float decay_amount;
	float destroy_at;
	float decay_time;
	float decay_rate;

	void Start () {
		name = "Foodbit";
		settings = Settings.getInstance();
		init_energy = (double) settings.contents["foodbit"]["init_energy"];

		energy = init_energy;

		eth = Ether.getInstance();
		Collider co = GetComponent<SphereCollider>();
		co.isTrigger = true;
		
	}

	public void destroy () {
		eth.removeFoodbit(this.gameObject);
		Destroy(gameObject);
	}



}
