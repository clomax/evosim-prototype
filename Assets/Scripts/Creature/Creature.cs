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

	List<ConfigurableJoint> joints = new List<ConfigurableJoint>();

	public double age;
	public double energy;

	public Chromosome chromosome;
	
	public double 	line_of_sight;
	double 			hunger_threshold;
	double 			metabolic_rate;
	int 			age_sexual_maturity;

	public int times_mated;
	public int times_eaten;
	
	public enum State { 
						persuing_food,
						persuing_mate,
						searching_for_mate,
						mating,
						eating,
						searching_for_food,
						neutral
					  };
	public State state;
	public Eye eye_script;

	void Start () {
		_t = transform;		
		name = "creature" + gameObject.GetInstanceID();
		
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
		root.rigidbody.mass = 15F;
		root.rigidbody.angularDrag = 20F;
		//root.collider.material = (PhysicMaterial)Resources.Load ("Physics Materials/Creature");

		eye = new GameObject();
		eye.name = "Eye";
		eye.transform.parent 			= root.transform;
		eye.transform.eulerAngles 		= root.transform.eulerAngles;
		eye.transform.position 			= root.transform.position;
		eye_script = eye.AddComponent<Eye>();

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

		setupLimbs();

		age = 0.0D;
		state = State.neutral;
		times_eaten = 0;
		times_mated = 0;
		
		InvokeRepeating("updateAge",1.0f,1.0f);
		InvokeRepeating("updateState",0,0.1f);
		InvokeRepeating("metabolise",1.0f,1.0f);
	}

	public float phase;

	void FixedUpdate () {
		//float sine = Sine (chromosome.base_joint_frequency, chromosome.base_joint_amplitude, 0);
		for (int i=0; i<joints.Count; i++) {
			joints[i].targetAngularVelocity = new Vector3(
				Sine (chromosome.base_joint_frequency, chromosome.base_joint_amplitude, chromosome.base_joint_phase),
			    //Sine (chromosome.base_joint_frequency, chromosome.base_joint_amplitude, chromosome.base_joint_phase),
				0F,
			    //Sine (chromosome.base_joint_frequency, chromosome.base_joint_amplitude, chromosome.base_joint_phase)
				0F
			);
		}
	}

	float Sine (float freq, float amplitude, float phase_shift) {
		return Mathf.Sin(Time.time * freq + phase_shift) * amplitude;
	}

	void updateAge() {
		age += 1;
	}

	public void setEnergy(double n) {
		energy = n;
	}
	
	void updateState() {
		if(state != Creature.State.mating) {
			if (energy < hunger_threshold) {
				state = eye_script.closestFbit != null ? State.persuing_food : State.searching_for_food;
			}
			if (energy >= hunger_threshold && age > age_sexual_maturity) {
				state = eye_script.closestCrt != null ? State.persuing_mate : State.searching_for_mate;
			}
		}
	}
	
	public void invokechromosome (Chromosome gs) {
		this.chromosome = gs;
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
		int num_branches = chromosome.getBranchCount();

		for (int i=0; i<num_branches; i++) {
			ArrayList limbs = chromosome.getLimbs(i);
			List<GameObject> actual_limbs = new List<GameObject>();

			for (int j=0; j<limbs.Count; j++) {

				GameObject limb = GameObject.CreatePrimitive(PrimitiveType.Cube);
				limb.layer = LayerMask.NameToLayer("Creature");
				limb.name = "limb_"+i+"_"+j;
				limb.transform.parent = _t;
				actual_limbs.Add(limb);
				Limb limb_script = limb.AddComponent<Limb>();

				ArrayList attributes = (ArrayList) limbs[j];
				limb_script.setScale( (Vector3) attributes[1] );
				limb_script.setColour( (Color) chromosome.getLimbColour());

				if(j == 0) {
					limb_script.setPosition( (Vector3) attributes[0] );
					limb.transform.LookAt(root.transform);
				} else {
					limb_script.setPosition( actual_limbs[j-1].transform.localPosition );
					limb.transform.LookAt(root.transform);
					limb.transform.Translate(0,0,-actual_limbs[j-1].transform.localScale.z);
				}
				
				limb.AddComponent<Rigidbody>();
				limb.AddComponent<BoxCollider>();
				limb.collider.material = (PhysicMaterial)Resources.Load("Physics Materials/Creature");

				ConfigurableJoint joint = limb.AddComponent<ConfigurableJoint>();
				joint.axis = new Vector3(0.5F, 0F, 0F);
				joint.anchor = new Vector3(0F, 0F, 0.5F);
				if(j == 0) {
					joint.connectedBody = root.rigidbody;
				} else {
					joint.connectedBody = actual_limbs[j-1].rigidbody;
				}
				limb.rigidbody.mass = 1;

				joints.Add(joint);

				joint.xMotion = ConfigurableJointMotion.Locked;
				joint.yMotion = ConfigurableJointMotion.Locked;
				joint.zMotion = ConfigurableJointMotion.Locked;

				joint.angularXMotion = ConfigurableJointMotion.Free;
				joint.angularYMotion = ConfigurableJointMotion.Free;
				joint.angularZMotion = ConfigurableJointMotion.Locked;

				JointDrive angXDrive = new JointDrive();
				angXDrive.mode = JointDriveMode.Velocity;
				angXDrive.maximumForce = 200;
				joint.angularXDrive = angXDrive;
				joint.angularYZDrive = angXDrive;
			}
		}

		root.layer = LayerMask.NameToLayer("Creature");
		Physics.IgnoreLayerCollision(8,8);
	}

}









