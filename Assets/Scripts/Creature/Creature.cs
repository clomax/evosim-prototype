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
	float sensitivityFwd;
	float sensitivityHdg;
	
	public static double init_energy;
	public double energy;
	float hdg = 0F;
	Transform _t;
	Logger lg;
	public int line_of_sight;
	int matingEnergyDeduction;
	double hunger_threshold;
	public enum State { hungry, persuing_mate, mating, eating, neutral };
	public State state;
	MeshRenderer mr;
	Material mat;
#pragma warning restore 0414
	
	void Start () {
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		
		settings = Settings.getInstance();
		
		_t = transform;
		name = "Creature";
		hdg = transform.localEulerAngles.y;
		//lg = Logger.getInstance();
		mr = _t.gameObject.GetComponent<MeshRenderer>();
		mat = (Material)Resources.Load("Materials/creature");
		mr.material = mat;
		
		init_energy =		(double) settings.contents ["creature"]["init_energy"];
		hunger_threshold = 	(double) settings.contents ["creature"]["hunger_threshold"];
		line_of_sight = 	(int) settings.contents [name.ToLower()]["line_of_sight"];
		
		age = 0.0D;
		
		sensitivityFwd = 1.0F;
		sensitivityHdg = 2.5F;
		
		eye = new GameObject();
		eye.name = "Eye";
		eye.transform.parent = transform;
		eye.transform.localPosition = Vector3.zero;
		eye.AddComponent<Eye>();
		SphereCollider sp = eye.AddComponent<SphereCollider>();
		sp.isTrigger = true;
		sp.radius = line_of_sight;
		
		mouth = new GameObject();
		mouth.name = "Mouth";
		mouth.transform.parent = transform;
		mouth.transform.localPosition = new Vector3(0,0,0.5F);
		mouth.transform.localEulerAngles = new Vector3(0,0,0);
		mouth.AddComponent("Mouth");
		
		genital = new GameObject();
		genital.name = "Genital";
		genital.transform.parent = transform;
		genital.transform.localPosition = new Vector3(0,0,-.5f);
		genital.transform.localEulerAngles = new Vector3(0,180,0);
		genital.AddComponent<Genitalia>();
		
		InvokeRepeating("updateAge",0,1.0f);
	}
	
	void updateAge() {
		age += 1;
	}
	
	void Update () {				
		changeHeading(Input.GetAxis("Horizontal") * sensitivityHdg);
		moveForward(Input.GetAxis("Vertical") * sensitivityFwd);
		
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
		if(energy < 0) energy = 0;
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
	
	
	/*
	 * Stuff to make the creatures move to keyboard commands
	 */
	
	void moveForward (float n) {
		Vector3 fwd = _t.forward;
		fwd.y = 0;
		fwd.Normalize();
		_t.position += n * fwd;
	}
	
	void changeHeading (float n) {
		hdg += n;
		wrapAngle(hdg);
		_t.localEulerAngles = new Vector3(0,hdg,0);
	}
	
	void wrapAngle (float angle) {
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
	}

}
