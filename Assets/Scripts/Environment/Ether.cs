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
	
	public static GameObject container;
	public static Ether instance;
	
	GameObject foodbit;

	Logger lg;
	Settings settings;
	
	public int total_energy;
	int energy;
	int foodbitEnergy = 20;
	double timeCreated;
	double timeToSpawnFoodbit = 0.0f;
	public int foodbitCount = 0;
	int fbSpawnRange = 200;
	
	
	void Start () {
		foodbit = (GameObject)Resources.Load("Prefabs/Foodbit");
		settings = Settings.getInstance();
		total_energy = settings.contents["ether"]["total_energy"].AsInt;
		energy = total_energy;
		timeCreated = Time.time;
	}
	
	public static Ether getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Ether";
			instance = container.AddComponent(typeof(Ether)) as Ether;
		}
		return instance;
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
	
	public int getEnergy() {
		return energy;
	}
	
	void subtractEnergy (int n) {
		energy -= n;
	}
	
	public void addToEnergy(int n) {
		energy += n;
	}

	bool enoughEnergy(int n) {
		return energy >= n;
	}
	

}
