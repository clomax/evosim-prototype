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
 	int foodbit_spawn_time;
	public int foodbit_count;
	int fb_spawn_range;
	int start_number_foodbits;
	
	
	void Start () {
		foodbit = (GameObject)Resources.Load("Prefabs/Foodbit");
		string name = this.name.ToLower();
		
		settings = Settings.getInstance();
		
		total_energy = 			(int) settings.contents	[name]["total_energy"];
		foodbit_energy = 		(int) settings.contents	[name]["foodbit_energy"];
		foodbit_spawn_time = 	(int) settings.contents	[name]["foodbit_spawn_time"];
		fb_spawn_range = 		(int) settings.contents	[name]["fb_spawn_range"];
		start_number_foodbits = (int) settings.contents [name]["start_number_foodbits"];
		
		energy = total_energy;
		foodbit_count = 0;
		
		for (int i=0; i<start_number_foodbits; i++) {
			newFoodbit();
		}
		
		InvokeRepeating("newFoodbit", 0, (float)foodbit_spawn_time);
	}
	
	public static Ether getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Ether";
			instance = container.AddComponent(typeof(Ether)) as Ether;
		}
		return instance;
	}
	
		
	/*
	 * Place a new foodbit at a random vector,
	 * assign the default energy value for
	 * all foodbits and attach the script
	 */
	public void newFoodbit () {
		Vector3 pos = Utility.RandomFlatVec( -fb_spawn_range,
			                                	 Foodbit.foodbitHeight /2,
			                                	 fb_spawn_range
			               				   );
		GameObject fb = (GameObject)Instantiate(foodbit, pos, Quaternion.identity);
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
