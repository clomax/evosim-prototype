using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		25.12.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
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
	
	void Update () {
		
	}
	
	public void observe (GameObject a, GameObject b) {
		this.collision_events.Add(new CollEvent(a, b));
		CollEvent dup = this.findMatch(a, b);
		// If a duplicate has been found - spawn
		if (null != dup) {
			this.collision_events.Clear();
			Vector3 pos = Utility.RandomFlatVec(-200,1,200);
			this.spw.spawn(pos,Vector3.zero);
		} else {
			this.collision_events.Add(new CollEvent(b,a));
		}
	}
	
	private CollEvent findMatch(GameObject a, GameObject b) {
		foreach (CollEvent e in this.collision_events) {
			// if the object signalling the collision exists in another event - there is a duplicate
			if (b == e.getColliders()[0] || a == e.getColliders()[1]) {
				return e;
			}
		}
		return null;
	}
	
}
