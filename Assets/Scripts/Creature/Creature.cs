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
	
	private GameObject mouth;
	private int id;
	private float sensitivityFwd = 1.0F;
	private float sensitivityHdg = 2.5F;
	private double energy;
	private float hdg = 0F;
	private Transform _t;
	#pragma warning disable 0414
	private Logger lg;
	#pragma warning restore 0414
	
	void Start () {
		this._t = transform;
		this.name = "Creature";
		this.hdg = transform.localEulerAngles.y;
		this.lg = Logger.getInstance();
		
		this.mouth = (GameObject)Resources.Load("Prefabs/Creature/Mouth");
		this.mouth.transform.localPosition = new Vector3(0,0,0.5F);
	}
	
	public Creature () {
		this.id = GetInstanceID();
		//induce effects of genes
	}
	
	void Update () {
		this.changeHeading(Input.GetAxis("Horizontal") * this.sensitivityHdg);
		this.moveForward(Input.GetAxis("Vertical") * this.sensitivityFwd);
	}
	
	

	
	/*
	 * Return the current energy value for the creature
	 */
	public double getEnergy () {
		return this.energy;
	}
	
	/*
	 * Add to the creature the energy of what it ate
	 */
	public void eat (double n) {
		this.energy += n;
	}
	
	public void subtractEnergy (double n) {
		this.energy -= n;	
	}
	
	/*
	 * Remove the creature from existence and return
	 * the creature's energy.
	 */
	public double kill () {
		Destroy(gameObject);
		return (this.getEnergy());
	}
	
	public int getID () {
		return this.id;
	}
	
	
	
	
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
