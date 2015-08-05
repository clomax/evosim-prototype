using UnityEngine;
using System;
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

	Transform _t;

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

	ArrayList limbs;

// TODO: Fix this shit "state machine"
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
	public Vector3 target_direction;
	private Quaternion lookRotation;
	private float sine;
	private float lambda;
	private Vector3 direction;

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
		//root.rigidbody.mass = 15F;
		root.GetComponent<Rigidbody>().angularDrag 	= float.Parse( settings.contents["creature"]["angular_drag"].ToString() );
		root.GetComponent<Rigidbody>().drag 				= float.Parse( settings.contents["creature"]["drag"].ToString() );
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
		mouth.transform.localPosition 	= new Vector3(0,0,.5F);
		mouth.AddComponent<Mouth>();

		genital = new GameObject();
		genital.name = "Genital";
		genital.transform.parent 		= root.transform;
		genital.transform.eulerAngles 	= root.transform.eulerAngles;
		genital.transform.localPosition	= new Vector3(0,0,-.5F);
		genital.AddComponent<Genitalia>();

		line_of_sight 		= (double) 	settings.contents ["creature"]["line_of_sight"];
		metabolic_rate 		= (double) 	settings.contents ["creature"]["metabolic_rate"];
		age_sexual_maturity = (int)		settings.contents ["creature"]["age_sexual_maturity"];

		setupLimbs();

		age = 0.0D;
		state = State.neutral;
		times_eaten = 0;
		times_mated = 0;

		InvokeRepeating("updateState",0,0.1f);
		InvokeRepeating("metabolise",1.0f,1.0f);
		InvokeRepeating("RandomDirection", 1F, 5F);

		root.GetComponent<Rigidbody>().SetDensity(4F);
		lambda = 20F;
	}


// TODO: Find a better way of controlling the joints with wave functions
//				the current way needs some sort of magic scalar
	void FixedUpdate () {
		sine = Sine(chromosome.base_joint_frequency, chromosome.base_joint_amplitude, chromosome.base_joint_phase);
		for (int i=0; i<joints.Count; i++) {
			joints[i].targetRotation = Quaternion.Euler (sine * new Vector3(5F,0F,0F));
		}

		if(eye_script.goal) {
			target_direction = (eye_script.goal.transform.position - root.transform.position).normalized;
		}

		if (target_direction != Vector3.zero) {
			lookRotation = Quaternion.LookRotation(target_direction);
		}

		float abs_sine = Mathf.Abs(sine);
		float pos_sine = System.Math.Max(sine,0);
		root.transform.rotation = Quaternion.Slerp(root.transform.rotation, lookRotation, Time.deltaTime * (abs_sine * 2F));

		if (pos_sine == 0) {
			direction = root.transform.forward;
		}

		root.GetComponent<Rigidbody>().AddForce(direction * pos_sine * chromosome.getBranchCount());
	}

	float Sine (float freq, float amplitude, float phase_shift) {
		return Mathf.Sin((float)age * freq + phase_shift) * amplitude;
	}

	void Update () {
		age += Time.deltaTime;
	}

	private void RandomDirection () {
		target_direction = new Vector3 (
						UnityEngine.Random.Range(-1F,1F),
						UnityEngine.Random.Range(-1F,1F),
						UnityEngine.Random.Range(-1F,1F)
		);
	}

	public void setEnergy(double n) {
		energy = n;
	}

	void updateState() {
		if(state != Creature.State.mating) {
			if (energy < chromosome.hunger_threshold) {
				state = (eye_script.targetFbit != null) ? State.persuing_food : State.searching_for_food;
			}
			if (energy >= chromosome.hunger_threshold && age > age_sexual_maturity) {
				state = (eye_script.targetCrt != null) ? State.persuing_mate : State.searching_for_mate;
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
	 * Remove a specified amount of energy from the creature,
	 * kill it if the creature's energy reaches zero.
	 */
	public void subtractEnergy (double n) {
		if (energy <= n) {
			eth.energy += energy;
			kill ();
		} else {
			energy -= n;
		}
	}

	/*
	 * Remove energy from the creature for merely existing,
	 * return it to the ether.
	 */
	private void metabolise () {
		subtractEnergy(metabolic_rate);
		eth.energy += metabolic_rate;
	}

	/*
	 * Remove the creature from existence and return
	 * the creature's energy.
	 */
	public double kill () {
		Destroy(gameObject);
		crt_count.number_of_creatures -= 1;
		return energy;
	}

// TODO: Limbs should be made into a better tree structure, not this
// 				list of lists rubbish
	private void setupLimbs () {
		int num_branches = chromosome.getBranchCount();

		for (int i=0; i<num_branches; i++) {
			limbs = chromosome.getLimbs(i);
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
				limb.GetComponent<Collider>().material = (PhysicMaterial)Resources.Load("Physics Materials/Creature");

				ConfigurableJoint joint = limb.AddComponent<ConfigurableJoint>();
				//joint.configuredInWorldSpace = true;
				joint.axis = new Vector3(0.5F, 0F, 0F);
				joint.anchor = new Vector3(0F, 0F, 0.5F);
				if(j == 0) {
					joint.connectedBody = root.GetComponent<Rigidbody>();
				} else {
					joint.connectedBody = actual_limbs[j-1].GetComponent<Rigidbody>();
				}
				limb.GetComponent<Rigidbody>().drag = 1F;

				joints.Add(joint);

				joint.xMotion = ConfigurableJointMotion.Locked;
				joint.yMotion = ConfigurableJointMotion.Locked;
				joint.zMotion = ConfigurableJointMotion.Locked;

				joint.angularXMotion = ConfigurableJointMotion.Free;
				joint.angularYMotion = ConfigurableJointMotion.Free;
				joint.angularZMotion = ConfigurableJointMotion.Free;

				JointDrive angXDrive = new JointDrive();
				angXDrive.mode = JointDriveMode.Position;
				angXDrive.positionSpring = 7F;
				angXDrive.maximumForce = 100000000F;
				joint.angularXDrive = angXDrive;
				joint.angularYZDrive = angXDrive;

				limb.GetComponent<Rigidbody>().SetDensity(1F);
			}
		}
	}

}
