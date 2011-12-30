using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		25.12.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */

/*
 *	Handles events that happen between multiple
 *	Objects of the same type.
 */
public class CollisionObserver : MonoBehaviour {
	
	public static CollisionObserver instance;
	public static GameObject container;
	
	public ArrayList collision_events = new ArrayList();
	public ArrayList evt;
	public Spawner spw;
	
	void Start () {
		spw = Spawner.getInstance();
	}
	
	public static CollisionObserver getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Collision Observer";
			instance = container.AddComponent(typeof(CollisionObserver)) as CollisionObserver;
		}
		return instance;
	}
	
	public void observe (GameObject a, GameObject b) {
		// for each pair in collision_events; has `a` collided with another object?
		Debug.Log(a.name + " " + b.name);
		
	}
	
}
