using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */

public class Creature : MonoBehaviour {

#pragma warning disable 0414
	public static int MAX_ENERGY = 100;
	
	private GameObject mouth;
	private GameObject genital;
	private int id;
	private float sensitivityFwd = 1.0F;
	private float sensitivityHdg = 2.5F;
	private int energy;
	private float hdg = 0F;
	private Transform _t;
	private Logger lg;
	private int line_of_sight;
	private int matingEnergyDeduction;
	private int hungerThreshold;
	public enum State { hungry, persuing_mate, mating, eating };
	public State state;
#pragma warning restore 0414
	
	void Start () {
		this._t = transform;
		this.name = "Creature";
		this.hdg = transform.localEulerAngles.y;
		this.lg = Logger.getInstance();
		this.line_of_sight = 2000;
		
		this.energy = MAX_ENERGY / 2;
		this.hungerThreshold = 50;
		
		mouth = new GameObject();
		mouth.name = "Mouth";
		mouth.transform.parent = transform;
		mouth.transform.localPosition = new Vector3(0,0,0.5F);
		mouth.transform.localEulerAngles = new Vector3(0,0,0);
		mouth.AddComponent("Mouth");
		
		genital = new GameObject();
		genital.name = "Genital";
		genital.transform.parent = transform;
		genital.transform.localPosition = new Vector3(0,0,-0.5F);
		genital.transform.localEulerAngles = new Vector3(0,180,0);
		genital.AddComponent("Genitalia");
	}
	
	public Creature () {
		this.id = GetInstanceID();
	}
	
	void Update () {
		this.changeHeading(Input.GetAxis("Horizontal") * this.sensitivityHdg);
		this.moveForward(Input.GetAxis("Vertical") * this.sensitivityFwd);
		if (this.energy < this.hungerThreshold) {
			this.state = State.hungry;
		}
		if (this.energy >= this.hungerThreshold) {
			this.state = State.persuing_mate;
		}
	}
	
	
	/*
	 * Return the current energy value for the creature
	 */
	public int getEnergy () {
		return this.energy;
	}
	
	/*
	 * Add to the creature the energy of what it ate
	 */
	public void eat (int n) {
		this.energy += n;
		if (this.energy > MAX_ENERGY) {
			this.energy = MAX_ENERGY;
		}
	}
	
	public void subtractEnergy (int n) {
		this.energy -= n;	
	}
	
	/*
	 * Remove the creature from existence and return
	 * the creature's energy.
	 */
	public int kill () {
		Destroy(gameObject);
		return (this.getEnergy());
	}
	
	public int getID () {
		return this.id;
	}
	
	public int getLOS() {
		return this.line_of_sight;
	}
	
	
	/*
	 * Stuff to make the creatures move to keyboard commands
	 */
	
	void moveForward (float n) {
		Vector3 fwd = _t.forward;
		fwd.y = 0;
		fwd.Normalize();
		this._t.position += n * fwd;
	}
	
	void changeHeading (float n) {
		this.hdg += n;
		this.wrapAngle(hdg);
		this._t.localEulerAngles = new Vector3(0,hdg,0);
	}
	
	void wrapAngle (float angle) {
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
	}

}
