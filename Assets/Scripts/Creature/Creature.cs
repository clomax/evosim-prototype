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
	public double energy;
	
	public Chromosome chromosome;
	
	public double 	line_of_sight;
	double 			hunger_threshold;
	double 			metabolic_rate;
	int 			age_sexual_maturity;
	
	public enum State { hungry,
						pursuing_mate,
						mating,
						eating,
						neutral
					  };
	public State state;
	
	int branch_limit;
	int recurrence_limit;
	
	GameObject limb;
	GameObject limb_child;
	int branches;
	ArrayList limbs;
	
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
		root.rigidbody.mass = 10;
		
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
		branch_limit 		= (int)		settings.contents ["creature"]["branch_limit"];
		recurrence_limit	= (int)		settings.contents ["creature"]["recurrence_limit"];

		setupLimbs();

		age = 0.0D;
		state = State.neutral;
		
		InvokeRepeating("updateAge",1.0f,1.0f);
		InvokeRepeating("updateState",0,0.1f);
		InvokeRepeating("metabolise",1.0f,1.0f);
	}

	void Update () {
		Sine(10f);
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
		//Debug.Log(y);
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
		if (energy > MAX_ENERGY) {
			double remainder = energy - MAX_ENERGY;
			energy = MAX_ENERGY;
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
		limbs = chromosome.getLimbs();
		branches = chromosome.getBranches();
		ArrayList limb_objects = new ArrayList();

		for (int i=0; i<branches; i++) {
			limb = GameObject.CreatePrimitive(PrimitiveType.Cube);
			limb.transform.parent = _t;
			
			Limb limb_script = limb.AddComponent<Limb>();
			ArrayList l = (ArrayList) limbs[i];
			
			limb_script.setColour		( (Color)	l[0] );
			limb_script.setPosition		( (Vector3) l[1] );
			limb_script.setScale		( (Vector3) l[2] );
			limb_script.setPosition		( Utility.RandomPointInsideCube(root.transform.localScale) );
			limb_script.setRecurrances	( (int) 	l[3] );
			limb.transform.LookAt(root.transform);
			
			limb.AddComponent<Rigidbody>();
			
			/* Hingejoint connecting limb to root */
			HingeJoint hj_root = limb.AddComponent<HingeJoint>();
			hj_root.axis = new Vector3(0.5F, 0F, 0F);
			hj_root.anchor = new Vector3(0F, 0F, 0.5F);
			hj_root.connectedBody = root.rigidbody;
			Physics.IgnoreCollision(root.collider, limb.collider, true);
			
			JointMotor m = new JointMotor();
			m.force = 10000;
			m.targetVelocity = 200;
			hj_root.motor = m;
			
			limb.rigidbody.mass = 3;
			
			foreach (GameObject lmb in limb_objects) {
				Physics.IgnoreCollision(limb.collider, lmb.collider, true);
			}
			limb_objects.Add(limb);

			GameObject limb_child = null;
			for (int j=0; j<limb_script.getRecurrances(); j++) {
				limb_child = GameObject.CreatePrimitive(PrimitiveType.Cube);
				limb_script = limb_child.AddComponent<Limb>();
				limb_script.setColour((Color) l[0]);

				limb_child.transform.parent = _t;
				limb_script.setScale (limb.transform.localScale * 0.8F);

				limb_child.transform.localPosition = limb.transform.localPosition;
				limb_child.transform.Translate (new Vector3(0,0,-limb_child.transform.localScale.z), limb.transform);

				limb_child.transform.LookAt(limb.transform);

				limb_child.transform.localRotation = limb.transform.localRotation;

				limb_child.AddComponent<Rigidbody>();
				limb_child.rigidbody.mass = 1;

				HingeJoint hj = limb_child.AddComponent<HingeJoint>();
				hj.axis = new Vector3(0.5F,0F,0F);
				hj.anchor = new Vector3(0F,0F,0.5F);
				hj.connectedBody = limb.rigidbody;
				Physics.IgnoreCollision(root.collider,limb_child.collider,true);
				Physics.IgnoreCollision(limb.collider,limb_child.collider,true);
			}
		}
	}

}
