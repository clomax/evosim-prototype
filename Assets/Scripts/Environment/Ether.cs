using UnityEngine;
using System.Collections;
using LitJson;

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
	
	public double total_energy;
	double energy;
	double foodbit_energy;
 	float foodbit_spawn_time;
	public int foodbit_count;
	int fb_spawn_range;
	int start_number_foodbits;
	
	
	void Start () {
		foodbit = (GameObject)Resources.Load("Prefabs/Foodbit");
		string name = this.name.ToLower();
		
		settings = Settings.getInstance();
		
		total_energy = 			(double) settings.contents[name]["total_energy"];
		foodbit_energy = 		(double) settings.contents	[name]["foodbit_energy"];
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
	
	public double getEnergy() {
		return energy;
	}
	
	public void subtractEnergy (double n) {
		energy -= n;
	}
	
	public void addToEnergy(double n) {
		energy += n;
	}

	bool enoughEnergy(double n) {
		return energy >= n;
	}
	

}
