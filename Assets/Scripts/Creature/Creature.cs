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
	
	CreatureCount crt_count;
	Settings settings;
	
	double age;
	public GameObject eye;
	public GameObject mouth;
	public GameObject genital;

	public ConfigurableJoint config_joint;
	public ConfigurableJoint config_joint2;
	public float x = 0.0F;
	
	SoftJointLimit zAngularLimit;
	JointDrive zAngDrive;
	
	GameObject root;
	GameObject c1limb21;
	GameObject c1limb22;
	
	public static float frequency = 5F;
	public static float amplitude = 3F;
	
	public static double init_energy;
	public double energy;
	Transform _t;
	Logger lg;
	public int line_of_sight;
	int matingEnergyDeduction;
	double hunger_threshold;
	double metabolic_rate;
	public enum State { hungry, persuing_mate, mating, eating, neutral };
	public State state;
	MeshRenderer mr;
	Material mat;
#pragma warning restore 0414

	void Start () {
		_t = transform;
		gameObject.AddComponent<Rigidbody>();
		
		name = "Creature";
		
		eye = new GameObject();
		eye.name = "Eye";
		eye.transform.parent = _t;
		eye.transform.localEulerAngles = Vector3.zero;
		eye.transform.localPosition = Vector3.zero;
		eye.AddComponent<Eye>();
		
		mouth = new GameObject();
		mouth.name = "Mouth";
		mouth.transform.parent = _t;
		mouth.transform.localEulerAngles = Vector3.zero;
		mouth.transform.localPosition = Vector3.zero;
		mouth.AddComponent<Mouth>();
		
		genital = new GameObject();
		genital.name = "Genital";
		genital.transform.parent = _t;
		genital.transform.localEulerAngles = Vector3.zero;
		genital.transform.localPosition = Vector3.zero;
		genital.AddComponent<Genitalia>();
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		
		settings = Settings.getInstance();
		
		_t = transform;
		name = "Creature";
		//lg = Logger.getInstance();
		
		init_energy =		(double) settings.contents ["creature"]["init_energy"];
		hunger_threshold = 	(double) settings.contents ["creature"]["hunger_threshold"];
		line_of_sight = 	(int) 	 settings.contents ["creature"]["line_of_sight"];
		metabolic_rate = 	(double) settings.contents ["creature"]["metabolic_rate"];
		
		age = 0.0D;
		
		InvokeRepeating("updateAge",0,1.0f);
		InvokeRepeating("metabolise",0,1.0f);
		
		
		
		

		
		/*
		// second limb
		c1limb21 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		c1limb21.name = "c1limb21";
		c1limb21.transform.localPosition = new Vector3(2.8f,10,-28.5f);
		c1limb21.transform.localScale = new Vector3(5,1,1);
		c1limb21.transform.rotation = new Quaternion(0f,-.2f,0f,1f);
		c1limb21.AddComponent("Rigidbody");
		c1limb21.transform.parent = transform;
		c1limb21.renderer.material.color = Color.green;
		
		c1limb22 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		c1limb22.name = "c1limb22";
		c1limb22.transform.localPosition = new Vector3(2.8f,10,-31.5f);
		c1limb22.transform.localScale = new Vector3(5,1,1);
		c1limb22.transform.rotation = new Quaternion(0f,.2f,0f,1f);
		c1limb22.AddComponent("Rigidbody");
		c1limb22.transform.parent = transform;
		c1limb22.renderer.material.color = Color.green;

		
		//configurable joint limb1 - 2
		config_joint = (ConfigurableJoint)root.AddComponent("ConfigurableJoint");
		config_joint.connectedBody = c1limb21.rigidbody;
		
		config_joint2 = (ConfigurableJoint)root.AddComponent("ConfigurableJoint");
		config_joint2.connectedBody = c1limb22.rigidbody;
		
		
		
		// anchor
		config_joint.anchor = new Vector3(.5F,0,0);
		
		//lock axes
		config_joint.xMotion = ConfigurableJointMotion.Locked;
		config_joint.yMotion = ConfigurableJointMotion.Locked;
		config_joint.zMotion = ConfigurableJointMotion.Locked;
		
		//lock angular rotation for x and y, limit to 30 for z
		config_joint.angularXMotion = ConfigurableJointMotion.Locked;
		config_joint.angularYMotion = ConfigurableJointMotion.Locked;
		
		config_joint.angularZMotion = ConfigurableJointMotion.Limited;
		zAngularLimit = new SoftJointLimit();
		zAngularLimit.limit = 150F;
		config_joint.angularZLimit = zAngularLimit;
		
		// enable angluar z drive
		zAngDrive = new JointDrive();
		zAngDrive.mode = JointDriveMode.Velocity;
		zAngDrive.maximumForce = 3F;
		config_joint.angularYZDrive = zAngDrive;
		
		
		// anchor
		config_joint2.anchor = new Vector3(.5F,0,0);
		
		//lock axes
		config_joint2.xMotion = ConfigurableJointMotion.Locked;
		config_joint2.yMotion = ConfigurableJointMotion.Locked;
		config_joint2.zMotion = ConfigurableJointMotion.Locked;
		
		//lock angular rotation for x and y, limit to 30 for z
		config_joint2.angularXMotion = ConfigurableJointMotion.Locked;
		config_joint2.angularYMotion = ConfigurableJointMotion.Locked;
		
		config_joint2.angularZMotion = ConfigurableJointMotion.Limited;
		zAngularLimit = new SoftJointLimit();
		zAngularLimit.limit = 150F;
		config_joint2.angularZLimit = zAngularLimit;
		
		// enable angluar z drive
		zAngDrive = new JointDrive();
		zAngDrive.mode = JointDriveMode.Velocity;
		zAngDrive.maximumForce = 3F;
		config_joint2.angularYZDrive = zAngDrive;
		
		
	
		
		// anchor
		config_joint.anchor = new Vector3(.5F,0,0);
		
		//lock axes
		config_joint.xMotion = ConfigurableJointMotion.Locked;
		config_joint.yMotion = ConfigurableJointMotion.Locked;
		config_joint.zMotion = ConfigurableJointMotion.Locked;
		
		//lock angular rotation for x and y, limit to 30 for z
		config_joint.angularXMotion = ConfigurableJointMotion.Locked;
		config_joint.angularYMotion = ConfigurableJointMotion.Locked;
		
		config_joint.angularZMotion = ConfigurableJointMotion.Limited;
		zAngularLimit = new SoftJointLimit();
		zAngularLimit.limit = 150F;
		config_joint.angularZLimit = zAngularLimit;
		
		// enable angluar z drive
		zAngDrive = new JointDrive();
		zAngDrive.mode = JointDriveMode.Velocity;
		zAngDrive.maximumForce = 3F;
		config_joint2.angularYZDrive = zAngDrive;
		
		*/
	

	}
		
	// Update is called once per frame
	void FixedUpdate () {
		x = Sine(x);
		//Vector3 z = new Vector3(0.0F,0.0F, x * amplitude);
		//config_joint2.targetAngularVelocity = z;
		//config_joint.targetAngularVelocity = z;
	}
	
	private static float Sine (float x){
		return Mathf.Sin(Time.timeSinceLevelLoad * frequency);
	}
	
	/**
	 * Given a variable of constant delta (age of a creature, typically),
	 * the frequency, and amplitude, return the sine
	 */
	private static float Sine (float x, float frequency, float amplitude){
		return amplitude * Mathf.Sin(2 * Mathf.PI
		                             * (frequency * x)
		                             + Time.timeSinceLevelLoad
		                            );
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
