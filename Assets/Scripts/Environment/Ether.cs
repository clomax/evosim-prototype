using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */


public class Ether : MonoBehaviour {
	
	public GameObject foodbit;

	private Foodbit fb;
	private Logger lg;	
	
	private double grossEnergy = 20000.0f;
	private double energy;
	private double foodbitEnergy = 50.0f;
	private double timeCreated;
	private double timeToSpawnFoodbit = 0.0f;
	private int foodbitCount = 0;
	private int fbSpawnRange = 200;
	

	void Awake () {
		name = "Ether";
	}
	
	void Start () {
		foodbit = (GameObject)Resources.Load("Prefabs/Foodbit");
		lg = Logger.getInstance();
		lg.write("EVT: Ether_instantiated" + " " + Time.realtimeSinceStartup);
		timeCreated = Time.time;
	}
	
	public Ether () {
		energy = grossEnergy;		
	}
	
	void Update () {
		if (Time.time > (timeCreated + timeToSpawnFoodbit) && enoughEnergy(foodbitEnergy)) {
			Vector3 pos = Utility.RandomVec(-fbSpawnRange,
			                                Foodbit.foodbitHeight /2,
			                                fbSpawnRange
			                               );
			newFoodbit(pos);
			timeCreated = Time.time;
		}
	}
	
	/*
	 * Place a new foodbit at the given vector
	 * and assign the default energy value for
	 * all foodbits
	 */
	public void newFoodbit (Vector3 vec) {
		Instantiate(foodbit, vec, Quaternion.identity);
		energy -= foodbitEnergy;
		foodbitCount++;
	}
	
	/*
	 * Return the energy value given to each new
	 * foodbit on instantiation
	 */
	public double getFoodbitEnergy () {
		return foodbitEnergy;
	}
	
	public double getEnergy() {
		return energy;
	}
	
	public int getFoodbitCount () {
		return foodbitCount;
	}
	
	public void addToEnergy(double n) {
		energy += n;
	}

	public bool enoughEnergy(double nrg) {
		return energy >= nrg;
	}
	

}
