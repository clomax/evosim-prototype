using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class Ether : MonoBehaviour {
	
	private GameObject foodbit;

	private Logger lg;	
	
	private int grossEnergy = 4000;
	private int energy;
	private int foodbitEnergy = 20;
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
			Vector3 pos = Utility.RandomFlatVec(-fbSpawnRange,
			                                Foodbit.foodbitHeight /2,
			                                fbSpawnRange
			                               );
			newFoodbit(pos);
			timeCreated = Time.time;
		}
	}
	
	/*
	 * Place a new foodbit at the given vector,
	 * assign the default energy value for
	 * all foodbits and attach the script
	 */
	public void newFoodbit (Vector3 vec) {
		GameObject fb = (GameObject)Instantiate(foodbit, vec, Quaternion.identity);
		fb.AddComponent("Foodbit");
		subtractEnergy(foodbitEnergy);
		foodbitCount++;
	}
	
	/*
	 * Return the energy value given to each new
	 * foodbit on instantiation
	 */
	public int getFoodbitEnergy () {
		return foodbitEnergy;
	}
	
	public double getEnergy() {
		return energy;
	}
	
	public int getFoodbitCount () {
		return foodbitCount;
	}
	
	public void subtractEnergy (int n) {
		energy -= n;
	}
	
	public void addToEnergy(int n) {
		energy += n;
	}

	public bool enoughEnergy(int n) {
		return energy >= n;
	}
	

}
