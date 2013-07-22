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
	double energy;
	
	
	void Start () {
		name = "Foodbit";
		settings = Settings.getInstance();
		
		energy = (double) settings.contents["ether"]["foodbit_energy"];
		
		mr = (MeshRenderer)gameObject.AddComponent("MeshRenderer");
		mr.material = (Material)Resources.Load("Materials/Foodbit");
		eth = Ether.getInstance();
		Collider co = GetComponent<BoxCollider>();
		co.isTrigger = true;
	}
	
	public double getEnergy() {
		return energy;
	}
	
	public void destroy () {
		eth.foodbit_count--;
		Destroy(gameObject);
	}



}
