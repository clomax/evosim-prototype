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
public class CollisionMediator : MonoBehaviour {
	
	public static CollisionMediator instance;
	public static GameObject container;	
	public CollEvent evt;
	public ArrayList collision_events = new ArrayList();
	public Spawner spw;
	
	void Start () {
		spw = Spawner.getInstance();
	}
	
	public static CollisionMediator getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Collision Observer";
			instance = container.AddComponent(typeof(CollisionMediator)) as CollisionMediator;
		}
		return instance;
	}
	
	public void observe (GameObject a, GameObject b) {
		this.collision_events.Add(new CollEvent(a, b));
		CollEvent dup = this.findDuplicate(a);
		// If a duplicate has been found - spawn
		if (null != dup) {
			this.spw.spawn(new Vector3(0,0,0), Vector3.zero);
			this.collision_events.Remove(dup);
		}
	}
	
	private CollEvent findDuplicate(GameObject g) {
		foreach (CollEvent e in this.collision_events) {
			// if the object signalling the collision exists in another event - there is a duplicate
			if (g == e.getColliders()[0] || g == e.getColliders()[1]) {
				return e;
			}
		}
		return null;
	}
	
}
