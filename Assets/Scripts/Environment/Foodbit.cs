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

	public static float foodbitHeight = 1.0f;
	
	Ether eth;
	MeshRenderer mr;
	int energy;
	
	
	void Start () {
		mr = (MeshRenderer)gameObject.AddComponent("MeshRenderer");
		mr.material = (Material)Resources.Load("Materials/Foodbit");
		eth = GameObject.Find("Ether").GetComponent<Ether>();
		energy = eth.getFoodbitEnergy();
		Collider co = GetComponent<BoxCollider>();
		co.isTrigger = true;
	}
	
	public int getEnergy() {
		return energy;
	}
	
	public void destroy () {
		eth.foodbitCount--;
		Destroy(gameObject);
	}



}
