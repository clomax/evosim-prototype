using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 *		Author: 	Craig Lomax
 *		Date: 		31.08.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

 

public class Utility : MonoBehaviour {

	static System.Random rnd = new System.Random();
	
	 // generate random float in the vicinity of n
	 public static float RandomApprox(float n, float r) {
		return Random.Range(n-r, n+r);
	}
	
	//return a random 'flat' vector within a given range
	public static Vector3 RandomFlatVec(float x, float y, float z) {
		return new Vector3( Random.Range(-x,x),
							y / 2,
							Random.Range(-z,z)
			              );
	}
	
	//return a random rotation on the y axis
	public static Vector3 RandomRotVec() {
		return new Vector3(0.0F,Random.Range(0.0F,360.0F),0.0F);
	}
	
	public static Vector3 RandomVector3() {
		return new Vector3 ( Random.Range(0.0F,360.0F),
							 Random.Range(0.0F,360.0F),
							 Random.Range(0.0F,360.0F)
						   );
	}
	
	//return a random point inside a given cube's scale
	public static Vector3 RandomPointInsideCube(Vector3 bounds) {
		return new Vector3 ( Random.Range (-bounds.x, bounds.x) / 2,
							 Random.Range (-bounds.y, bounds.y) / 2,
							 Random.Range (-bounds.z, bounds.z) / 2
						   );
	}
	
	public static int UnixTimeNow () {
		System.TimeSpan t = (System.DateTime.UtcNow - new System.DateTime(1970,1,1,0,0,0));
		return (int) t.TotalSeconds;
	}
	
	public static float randomDelta(float factor) {
		return (float) rnd.NextDouble() * ( Mathf.Abs(factor-(-factor)) ) + (-factor);
	}
	
}

public class MultiDimList : List<List<GameObject>> { }
