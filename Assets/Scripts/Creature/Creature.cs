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
	private static double MAX_ENERGY = 100.0D;
	
	Transform 		_t;
	
	Settings settings;
	Ether eth;
	Logger lg;
	
	public GameObject root;
	public Root root_script;
	
	Vector3 max_root_scale;
	Vector3 min_root_scale;
	Vector3 rootsize;
		
	public GameObject eye;
	public GameObject mouth;	
	public GameObject genital;
	
	public CreatureCount crt_count;

	double age;
	public static double init_energy;
	public double energy;
	
	public float[] chromosome;
	
	public double 	line_of_sight;
	double 			hunger_threshold;
	double 			metabolic_rate;
	int 			age_sexual_maturity;
	
	public enum State { hungry,
						persuing_mate,
						mating,
						eating,
						neutral
					  };
	public State state;
	
	int branch_limit;
	int recursion_limit;
	
	GameObject limb;
	Vector3 tmp;
#pragma warning restore 0414

	void Start () {
		_t = transform;		
		name = "Creature";
		
		eth = Ether.getInstance();
		settings = Settings.getInstance();
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		
		max_root_scale = new Vector3();
		max_root_scale.x = (float) ((double) settings.contents["creature"]["root"]["max_root_scale"]["x"]);
		max_root_scale.y = (float) ((double) settings.contents["creature"]["root"]["max_root_scale"]["y"]);
		max_root_scale.z = (float) ((double) settings.contents["creature"]["root"]["max_root_scale"]["z"]);
		
		min_root_scale = new Vector3();
		min_root_scale.x = (float) ((double) settings.contents["creature"]["root"]["min_root_scale"]["x"]);
		min_root_scale.y = (float) ((double) settings.contents["creature"]["root"]["min_root_scale"]["y"]);
		min_root_scale.z = (float) ((double) settings.contents["creature"]["root"]["min_root_scale"]["z"]);

		
		root = GameObject.CreatePrimitive(PrimitiveType.Cube);
		root.name = "root";
		root.transform.parent 			= _t;
		root.transform.position 		= _t.position;
		root.transform.localScale 		= rootsize;
		root.transform.eulerAngles 		= _t.eulerAngles;
		root.AddComponent<Rigidbody>();
		root_script = root.AddComponent<Root>();
		root_script.setColour(chromosome);
		root.transform.localScale = new Vector3( chromosome[3],
							   					 chromosome[4],
							   					 chromosome[5]
						   					   );
		root.rigidbody.mass = 10;
		
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
		
		init_energy 		= (double) 	settings.contents ["creature"]["init_energy"];
		hunger_threshold 	= (double) 	settings.contents ["creature"]["hunger_threshold"];
		line_of_sight 		= (double) 	settings.contents ["creature"]["line_of_sight"];
		metabolic_rate 		= (double) 	settings.contents ["creature"]["metabolic_rate"];
		age_sexual_maturity = (int)		settings.contents ["creature"]["age_sexual_maturity"];
		branch_limit 		= (int)		settings.contents ["creature"]["branch_limit"];
		recursion_limit		= (int)		settings.contents ["creature"]["recursion_limit"];
		

		
		for (int i=0; i<branch_limit; i++) {
			limb = GameObject.CreatePrimitive(PrimitiveType.Cube);
			MeshRenderer mr = limb.GetComponent<MeshRenderer>();
			mr.material.color = new Color(chromosome[0], chromosome[1], chromosome[2]);
			limb.transform.parent = _t;
			limb.AddComponent<Rigidbody>();
			
			limb.transform.localPosition = new Vector3(chromosome[6], chromosome[7], chromosome[8]);
			
			tmp = new Vector3(chromosome[9], chromosome[10], chromosome[11]);
			limb.transform.localEulerAngles = tmp;
			
			limb.transform.localScale = new Vector3(5f,2f,2f);
			
			HingeJoint j = limb.AddComponent<HingeJoint>();
			tmp = new Vector3(chromosome[12], chromosome[13], chromosome[14]);
			j.axis = tmp;
			
			j.anchor = new Vector3(0.5F,0,0);
			j.connectedBody = root.rigidbody;
			JointMotor m = new JointMotor();
			m.force = 1000;
			m.targetVelocity = 500;
			j.motor = m;
			
			Physics.IgnoreCollision(root.collider, limb.collider, true);
			limb.rigidbody.mass = 3;
			limb.collider.material = (PhysicMaterial) Resources.Load("Physics Materials/Rubber");
		}
		
		age = 0.0D;
		state = State.neutral;
		
		InvokeRepeating("updateAge",0,1.0f);
		InvokeRepeating("updateState",0,0.1f);
		InvokeRepeating("metabolise",0,1.0f);
	}	
		
	/*
	 * Add 1 second to the creature's age when called.
	 */
	void updateAge() {
		age += 1;
	}
	
	void updateState() {
		if(state != Creature.State.mating) {
			if (energy < hunger_threshold) {
				state = State.hungry;
			}
			if (energy >= hunger_threshold && age > age_sexual_maturity) {
				state = State.persuing_mate;
			}
		}
	}
	
	public void invokechromosome (params float[] gs) {
		this.chromosome = gs;
	}
	
	/*
	 * 
	 */
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
	
	/*
	 * Remove a specified amount of energy from the creature,
	 * kill it if the creature's energy reaches zero.
	 */
	public void subtractEnergy (double n) {
		energy -= n;
		if(energy <= 0) kill();
	}
	
	/*
	 * Remove energy from the creature for merely existing,
	 * return it to the ether.
	 */
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
