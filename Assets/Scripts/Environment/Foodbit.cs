using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		31.08.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
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
	
	private Ether eth;
	private MeshRenderer mr;
	private double energy;
	
	
	void Start () {
		mr = (MeshRenderer)this.gameObject.AddComponent("MeshRenderer");
		mr.material = (Material)Resources.Load("Materials/Foodbit");
		eth = GameObject.Find("Ether").GetComponent<Ether>();
		energy = eth.getFoodbitEnergy();
	}
	
	public double getEnergy() {
		return energy;
	}
	
	public void destroy () {
		Destroy(gameObject);
	}



}
