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
	public ArrayList collision_events;
	public Spawner spw;
	
	Settings settings;
	
	double energy_scale;
	
	void Start () {
		collision_events = new ArrayList();
		spw = Spawner.getInstance();
		settings = Settings.getInstance();
		energy_scale = (double) settings.contents["creature"]["energy_to_offspring"];
	}
	
	public static CollisionMediator getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Collision Observer";
			instance = container.AddComponent<CollisionMediator>();
		}
		return instance;
	}
	
	public void observe (GameObject a, GameObject b) {
		collision_events.Add(new CollEvent(a, b));
		CollEvent dup = findMatch(a, b);
		// If a duplicate has been found - spawn
		if (null != dup) {
			collision_events.Clear();
			Vector3 pos = Utility.RandomFlatVec(-200,10,200);
			
			// Get references to the scripts of each creature
			Creature a_script = a.transform.parent.parent.GetComponent<Creature>();
			Creature b_script = b.transform.parent.parent.GetComponent<Creature>();
			
			double a_energy = a_script.getEnergy();
			double b_energy = b_script.getEnergy();
			
			spw.spawn(pos,Vector3.zero,
					  a_energy * energy_scale +
					  b_energy * energy_scale,
					  a_script.genes
					 );
			a_script.subtractEnergy(a_energy * energy_scale);
			b_script.subtractEnergy(b_energy * energy_scale);
		} else {
			collision_events.Add(new CollEvent(b,a));
		}
	}
	
	private CollEvent findMatch(GameObject a, GameObject b) {
		foreach (CollEvent e in collision_events) {
			GameObject e0 = e.getColliders()[0];
			GameObject e1 = e.getColliders()[1];
			// if the object signalling the collision exists in another event - there is a duplicate
			if (b.GetInstanceID() == e0.GetInstanceID() || a.GetInstanceID() == e1.GetInstanceID()) {
				return e;
			}
		}
		return null;
	}
	
}
