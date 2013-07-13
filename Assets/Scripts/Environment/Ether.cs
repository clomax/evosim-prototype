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
	int foodbit_energy;
	double time_created;
	double time_to_spawn_foodbit;
	public int foodbit_count;
	int fb_spawn_range;
	int start_number_foodbits;
	
	
	void Start () {
		foodbit = (GameObject)Resources.Load("Prefabs/Foodbit");
		string name = "ether";
		
		settings = Settings.getInstance();
		total_energy = settings.contents			[name]["total_energy"].AsInt;
		foodbit_energy = settings.contents			[name]["foodbit_energy"].AsInt;
		time_to_spawn_foodbit = settings.contents	[name]["time_to_spawn_foodbit"].AsDouble;
		fb_spawn_range = settings.contents			[name]["fb_spawn_range"].AsInt;
		start_number_foodbits = settings.contents	[name]["start_number_foodbits"].AsInt;
		
		energy = total_energy;
		foodbit_count = 0;
		time_created = Time.time;
		
		for (int i=0; i<start_number_foodbits; i++) {
			Vector3 pos = Utility.RandomFlatVec(-fb_spawn_range,
			                                Foodbit.foodbitHeight /2,
			                                fb_spawn_range
			                               );
			newFoodbit(pos);
		}
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
		if (Time.time > (time_created + time_to_spawn_foodbit) && enoughEnergy(foodbit_energy)) {
			Vector3 pos = Utility.RandomFlatVec(-fb_spawn_range,
			                                Foodbit.foodbitHeight /2,
			                                fb_spawn_range
			                               );
			newFoodbit(pos);
			time_created = Time.time;
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
		subtractEnergy(foodbit_energy);
		foodbit_count++;
	}
	
	/*
	 * Return the energy value given to each new
	 * foodbit on instantiation
	 */
	public int getFoodbitEnergy () {
		return foodbit_energy;
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
