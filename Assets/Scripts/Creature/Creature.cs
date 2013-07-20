using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

public class Creature : MonoBehaviour {

#pragma warning disable 0414
	static double MAX_ENERGY = 100.0D;
	
	Settings settings;
	Ether eth;
	Logger lg;
	
	public GameObject root;
	public Root root_script;
	Vector3 rootsize;
	
	public GameObject eye;
	public GameObject mouth;	
	public GameObject genital;
	
	public CreatureCount crt_count;

	double age;
	public static double init_energy;
	public double energy;
	
	Transform _t;
	public double line_of_sight;
	int matingEnergyDeduction;
	double hunger_threshold;
	double metabolic_rate;
	public enum State { hungry, persuing_mate, mating, eating, neutral };
	public State state;

#pragma warning restore 0414

	void Start () {
		_t = transform;		
		name = "Creature";
		
		eth = Ether.getInstance();
		settings = Settings.getInstance();
		//lg = Logger.getInstance();
		
		root = GameObject.CreatePrimitive(PrimitiveType.Cube);
		root.name = "root";
		root.transform.parent 			= _t;
		root.transform.position 		= _t.position;
		root.transform.localScale 		= rootsize;
		root.transform.eulerAngles 		= _t.eulerAngles;
		root.AddComponent<Rigidbody>();
		root_script = root.AddComponent<Root>();
		
		eye = new GameObject();
		eye.name = "Eye";
		eye.transform.parent 			= root.transform;
		eye.transform.eulerAngles 		= root.transform.eulerAngles;
		eye.transform.position 			= root.transform.position;
		eye.AddComponent<Eye>();
		
		mouth = new GameObject();
		mouth.name = "Mouth";
		mouth.transform.parent 			= root.transform;
		mouth.transform.eulerAngles 	= root.transform.eulerAngles;
		mouth.transform.position 		= root.transform.position;
		mouth.AddComponent<Mouth>();
		
		genital = new GameObject();
		genital.name = "Genital";
		genital.transform.parent 		= root.transform;
		genital.transform.eulerAngles 	= root.transform.eulerAngles;
		genital.transform.position		= root.transform.position;
		genital.AddComponent<Genitalia>();
		
		init_energy 		= (double) settings.contents ["creature"]["init_energy"];
		hunger_threshold 	= (double) settings.contents ["creature"]["hunger_threshold"];
		line_of_sight 		= (double) settings.contents ["creature"]["line_of_sight"];
		metabolic_rate 		= (double) settings.contents ["creature"]["metabolic_rate"];
		
		age = 0.0D;
		
		InvokeRepeating("updateAge",0,1.0f);
		InvokeRepeating("metabolise",0,1.0f);
	}	
		
	
	void updateAge() {
		age += 1;
	}
	
	void Update () {				

		if(state != Creature.State.mating) {
			if (energy < hunger_threshold) {
				state = State.hungry;
			}
			if (energy >= hunger_threshold) {
				state = State.persuing_mate;
			}
		}
	}
	
	public void setRootSize (Vector3 scale) {
		rootsize = scale;	
	}
	
	
	/*
	 * Return the current energy value for the creature
	 */
	public double getEnergy () {
		return energy;
	}
	
	/*
	 * Add to the creature the energy of what it ate
	 */
	public void addEnergy (double n) {
		energy += n;
		if (energy > MAX_ENERGY) energy = MAX_ENERGY;
	}
	
	public void subtractEnergy (double n) {
		energy -= n;
		if(energy <= 0) kill();
	}
	
	private void metabolise () {
		subtractEnergy(metabolic_rate);
		eth.addToEnergy(metabolic_rate);
	}
	
	/*
	 * Remove the creature from existence and return
	 * the creature's energy.
	 */
	public double kill () {
		Destroy(gameObject);
		crt_count.number_of_creatures--;
		return energy;
	}

}
