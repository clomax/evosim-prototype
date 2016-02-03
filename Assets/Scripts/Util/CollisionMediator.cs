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
	public Ether ether;

	Settings settings;

	decimal energy_scale;
	double crossover_rate;
	double mutation_rate;
	float mutation_factor;

	void Start () {
		collision_events = new ArrayList();
		ether = Ether.getInstance();
		settings = Settings.getInstance();
		energy_scale 		= decimal.Parse(	settings.contents["creature"]["energy_to_offspring"].ToString());
		crossover_rate 		= (double) 			settings.contents["genetics"]["crossover_rate"];
		mutation_rate		= (double)			settings.contents["genetics"]["mutation_rate"];
		mutation_factor		= float.Parse(	    settings.contents["genetics"]["mutation_factor"].ToString() );
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
		// If a duplicate event has been found spawn a child
		if (null != dup) {
			collision_events.Clear();
			Vector3 pos = (a.transform.position - b.transform.position) * 0.5F + b.transform.position;

			// Get references to the scripts of each creature
			Creature a_script = a.transform.parent.parent.GetComponent<Creature>();
			Creature b_script = b.transform.parent.parent.GetComponent<Creature>();

			decimal a_energy = a_script.getEnergy();
			decimal b_energy = b_script.getEnergy();

			Chromosome newChromosome;
			newChromosome = GeneticsUtils.crossover(a_script.chromosome, b_script.chromosome, crossover_rate);
			newChromosome = GeneticsUtils.mutate(newChromosome, mutation_rate, mutation_factor);

            decimal a_energy_to_child = (a_energy * energy_scale);
            decimal b_energy_to_child = (b_energy * energy_scale);
            decimal new_crt_energy = (a_energy_to_child + b_energy_to_child);

			ether.spawner.spawn(
                      pos,Vector3.zero,
					  new_crt_energy,
					  newChromosome
					 );

			a_script.setEnergy(a_energy - a_energy_to_child);
			b_script.setEnergy(b_energy - b_energy_to_child);

			a_script.offspring++;
			b_script.offspring++;
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
