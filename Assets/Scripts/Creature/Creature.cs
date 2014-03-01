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
	int recursion_limit;
	
	GameObject limb;
	GameObject limb_child;
	int branches;
	ArrayList limbs;
	
	Renderer rd;
#pragma warning restore 0414

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
				limb_child.transform.parent = _t;				
				limb_script = limb_child.AddComponent<Limb>();
				limb_script.parent = limb;
				
				limb_script.setColour		( (Color)	l[0] );
				limb_child.transform.parent = limb.transform;
				//limb_script.setPosition		( new Vector3(0,0,limb.transform.localPosition.z) / 2 );
				limb_script.setPosition		( new Vector3(0,0,-0.9F) );
				limb_child.transform.parent = _t;
				limb_script.setScale		( new Vector3(1.6F,1.6F,4.4F) );
				limb_script.setRecurrances	( 0 );
				
				limb_child.transform.LookAt(limb.transform);


				
				limb_child.AddComponent<Rigidbody>();
				Physics.IgnoreCollision(root.collider, limb_child.collider, true);
				
				// add joint to parent
				HingeJoint jnt = limb.AddComponent<HingeJoint>();
				jnt.axis = new Vector3(1F,0F,0F);
				jnt.anchor = new Vector3(0F,0F,.5F);
				jnt.connectedBody = limb_child.rigidbody;

				JointMotor jm = new JointMotor();
				jm.force = 10000;
				jm.targetVelocity = -50;
				jnt.motor = jm;
				
				limb_child.rigidbody.mass = 1;
				//limb_child.collider.material = (PhysicMaterial) Resources.Load("Physics Materials/Rubber");
	
				foreach (GameObject lmb in limb_objects) {
					Physics.IgnoreCollision(limb_child.collider, lmb.collider, true);
				}
				limb_objects.Add(limb_child);
			}
			
			if (limb_child != null) {
				HingeJoint hj_child = limb.AddComponent<HingeJoint>();
				hj_child.axis = new Vector3(0.5F, 0F, 0F);
				hj_child.anchor = new Vector3(0F, 0F, -0.5F);
				hj_child.connectedBody = limb_child.rigidbody;
				Physics.IgnoreCollision(root.collider, limb_child.collider, true);
			}
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
