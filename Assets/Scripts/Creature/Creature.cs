using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

public class Creature : MonoBehaviour {
	
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
	public double energy;
	
	public Chromosome chromosome;
	
	public double 	line_of_sight;
	double 			hunger_threshold;
	double 			metabolic_rate;
	int 			age_sexual_maturity;
	double			max_energy;

	public int times_mated;
	public int times_eaten;
	
	public enum State { hungry,
						pursuing_mate,
						mating,
						eating,
						neutral
					  };
	public State state;

	GameObject limb;
	GameObject limb_child;
	MultiDimList branches;

	Renderer rd;

	void Start () {
		_t = transform;		
		name = "Creature";
		
		eth = Ether.getInstance();
		settings = Settings.getInstance();
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();


		max_root_scale = new Vector3();
		max_root_scale.x = float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["x"].ToString() );
		max_root_scale.y = float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["y"].ToString() );
		max_root_scale.z = float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["z"].ToString() );
		
		min_root_scale = new Vector3();
		min_root_scale.x = float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["x"].ToString() );
		min_root_scale.y = float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["y"].ToString() );
		min_root_scale.z = float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["z"].ToString() );

		
		root = GameObject.CreatePrimitive(PrimitiveType.Cube);
		root.name = "root";
		root.transform.parent 			= _t;
		root.transform.position 		= _t.position;
		root.transform.eulerAngles 		= _t.eulerAngles;
		root.AddComponent<Rigidbody>();
		root_script = root.AddComponent<Root>();
		root_script.setColour(chromosome.getColour());
		root_script.setScale(chromosome.getRootScale());
		root.rigidbody.mass = 3;
		
		rd = root.GetComponent<Renderer>();
		
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
		mouth.transform.localPosition 	= new Vector3(.5F,0,0);
		mouth.AddComponent<Mouth>();
		
		genital = new GameObject();
		genital.name = "Genital";
		genital.transform.parent 		= root.transform;
		genital.transform.eulerAngles 	= root.transform.eulerAngles;
		genital.transform.localPosition	= new Vector3(-.5F,0,0);
		genital.AddComponent<Genitalia>();
		
		hunger_threshold 	= (double) 	settings.contents ["creature"]["hunger_threshold"];
		line_of_sight 		= (double) 	settings.contents ["creature"]["line_of_sight"];
		metabolic_rate 		= (double) 	settings.contents ["creature"]["metabolic_rate"];
		age_sexual_maturity = (int)		settings.contents ["creature"]["age_sexual_maturity"];
		max_energy			= (double)	settings.contents ["creature"]["MAX_ENERGY"];

		setupLimbs();

		age = 0.0D;
		state = State.neutral;
		times_eaten = 0;
		times_mated = 0;
		
		InvokeRepeating("updateAge",1.0f,1.0f);
		InvokeRepeating("updateState",0,0.1f);
		InvokeRepeating("metabolise",1.0f,1.0f);
	}

	void Update () {
		//Sine(10f);
	}
		
	/*
	 * Add 1 second to the creature's age when called.
	 */
	void updateAge() {
		age += 1;
	}

	public void setEnergy(double n) {
		energy = n;
	}
	
	void updateState() {
		if(state != Creature.State.mating) {
			if (energy < hunger_threshold) {
				state = State.hungry;
			}
			if (energy >= hunger_threshold && age > age_sexual_maturity) {
				state = State.pursuing_mate;
			}
		}
	}
	
	public void invokechromosome (Chromosome gs) {
		this.chromosome = gs;
	}
	
	/*
	 * 
	 */
	public void setRootSize (Vector3 scale) {
		rootsize = scale;	
	}

	void Sine (float x) {
		float y = Mathf.Sin(Time.time * x);
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
		if (energy > max_energy) {
			double remainder = energy - max_energy;
			energy = max_energy;
			eth.addToEnergy(remainder);
		}
	}
	
	/*
	 * Remove a specified amount of energy from the creature,
	 * kill it if the creature's energy reaches zero.
	 */
	public void subtractEnergy (double n) {
		if (energy <= n) {
			eth.addToEnergy(energy);
			energy = 0;
			kill ();
		} else
			energy -= n;
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

	private void setupLimbs () {
		branches = chromosome.getBranches();

		for (int i=0; i<branches.Count; i++) {
			List<GameObject> limbs = branches[i];
			List<GameObject> actual_limbs = new List<GameObject>();

			for (int j=0; j<limbs.Count; j++) {
				Limb chromosome_limb_script = limbs[j].GetComponent<Limb>();

				GameObject limb = GameObject.CreatePrimitive(PrimitiveType.Cube);
				actual_limbs.Add(limb);
				limb.layer = LayerMask.NameToLayer("Creature");
				limb.name = "limb_"+i+"_"+j;
				limb.transform.parent = _t;

				Rigidbody rigidbody = limb.AddComponent<Rigidbody>();
				limb.AddComponent<BoxCollider>();
				rigidbody.mass = 1;

				Limb limb_script = limb.AddComponent<Limb>();


				limb_script.setScale( chromosome_limb_script.getScale() );
				limb_script.setColour( chromosome_limb_script.getColour() );
				if(j == 0) {
					limb_script.setPosition( chromosome_limb_script.getPosition() );
					limb.transform.LookAt(root.transform);
				} else {
					limb_script.setPosition( actual_limbs[j-1].transform.localPosition );
					limb.transform.LookAt(root.transform);
					limb.transform.Translate(0,0,-limb.transform.localScale.z);
				}

				HingeJoint hj = limb.AddComponent<HingeJoint>();
				hj.axis = new Vector3(0.5F, 0F, 0F);
				hj.anchor = new Vector3(0F, 0F, 0.5F);
				if(j == 0)	hj.connectedBody = root.rigidbody;
				else      	hj.connectedBody = actual_limbs[j-1].rigidbody;

				JointLimits jl = new JointLimits();
				jl.min = -90;
				jl.max = 90;
				hj.limits = jl;

				JointSpring js = new JointSpring();
				js.damper = 10;
				js.spring = 1000;
				js.targetPosition = 0;
				hj.spring = js;

				hj.useLimits = true;
				hj.useSpring = true;

			}
		}

		root.layer = LayerMask.NameToLayer("Creature");
		Physics.IgnoreLayerCollision(8,8);
	}

}









