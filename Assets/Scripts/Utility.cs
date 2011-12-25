using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		31.08.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */

 

public class Utility : MonoBehaviour {

	 // generate random float in the vicinity of n
	 public static float RandomApprox(float n, float r) {
		return Random.Range(n-r, n+r);
	}
	
	//return a random vector within a given range
	public static Vector3 RandomFlatVec(float x, float y, float z) {
		Vector3 vec = new Vector3( Random.Range(-x,x),
								   y / 2,
								   Random.Range(-z,z)
			                     );
		return vec;
	}
	
	public static Vector3 RandomRotVec() {
		return new Vector3(0,Random.Range(0,360),0);
	}
	
}
