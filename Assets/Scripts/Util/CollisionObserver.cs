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
	
	public void add_collision_event (GameObject a, GameObject b) {
		// for each pair in collision_events; has `a` collided with another object?
		foreach(ArrayList x in collision_events) {
			if (x[1] == a) {
				collision_events.Remove(x);
				return;
			} else {
				evt = new ArrayList(2);
				evt.Add(a);
				evt.Add(b);
				collision_events.Add(evt);
			}
		}
		
	}
	
}
