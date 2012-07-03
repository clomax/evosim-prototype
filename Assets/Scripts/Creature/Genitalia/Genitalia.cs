using UnityEngine;
using System.Collections;

/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class Genitalia : MonoBehaviour {

#pragma warning disable 0414
	private Creature crt;
	private Logger lg;
	private Spawner spw;
	private CollisionMediator co;
	private GenitalRadius gr;
	private Transform _t;
	private int crt_mate_range = 10;
	private float curr_dist = 1;
	private int id;
	private double timeCreated;
	public double timeToEnableMating = 3.0f;
#pragma warning restore 0414

	void Start () {
		this._t = transform;
		this.gameObject.tag = "Genital";
		this.crt = (Creature)_t.parent.gameObject.GetComponent("Creature");
		this.id = GetInstanceID();
		co = CollisionMediator.getInstance();
		
		_t = transform;
		timeCreated = Time.time;
	}
	
	void Update () {
		if (this.crt.state == Creature.State.mating && Time.time > (timeCreated + timeToEnableMating)) {
			this.crt.state = Creature.State.persuing_mate;
			timeCreated = Time.time;
		}
		
		if (curr_dist < crt_mate_range) {
			Genitalia other_genital = c.transform.gameObject.GetComponent<Genitalia>();
			Creature other_crt = c.gameObject.GetComponent<Creature>();
			if (this.crt.state == Creature.State.persuing_mate || other_crt.state == Creature.State.persuing_mate) {
				co.observe(this.gameObject, other_genital.gameObject);
				other_crt.state = Creature.State.mating;
				this.crt.state = Creature.State.mating;
			}
			dist = curr_dist;
		}
		
	}

}
