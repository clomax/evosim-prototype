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
	double 		energy;
	double 		foodbit_energy;

	Vector3 pos;

	float 	wide_spread;
	int		start_number_foodbits;
	float 	spore_time;
	int 	spore_range;
	
	ArrayList foodbits;
	
	
	void Start () {
		foodbit = (GameObject)Resources.Load("Prefabs/Foodbit");
		string name = this.name.ToLower();
		
		settings = Settings.getInstance();
		
		total_energy = 			(double) 	settings.contents[name]	["total_energy"];
		foodbit_energy = 		(double) 	settings.contents["foodbit"]["init_energy"];
		start_number_foodbits = (int)	 	settings.contents[name]	["start_number_foodbits"];
		spore_range = 			(int)	 	settings.contents["foodbit"]["spore_range"];
		wide_spread = 			float.Parse(settings.contents["foodbit"]["wide_spread"].ToString() );
		spore_time = 			float.Parse(settings.contents["foodbit"]["spore_time"].ToString() );

		energy = total_energy;

		foodbits = new ArrayList();
		
		for (int i=0; i<start_number_foodbits; i++) {
			Vector3 pos = Utility.RandomFlatVec( -wide_spread,
				                                  Foodbit.foodbitHeight /2,
				                                  wide_spread
				               				   );
			newFoodbit(pos);
		}
		
		InvokeRepeating("fbSpawn",spore_time, spore_time);
	}
	
	/*
	 * Place a new foodbit at a random vector,
	 * assign the default energy value for
	 * all foodbits and attach the script
	 */
	public void newFoodbit (Vector3 pos) {
		if(enoughEnergy(foodbit_energy)) {
			GameObject fb = (GameObject)Instantiate(foodbit, pos, Quaternion.identity);
			fb.AddComponent("Foodbit");
			subtractEnergy(foodbit_energy);
			foodbits.Add(fb);
		}
	}
	
	private void fbSpawn () {
		int fb_count = getFoodbitCount();
		if (fb_count >= 1) {
			int fb_index = Random.Range(0,fb_count);
			GameObject fb = (GameObject) foodbits[fb_index];
			Foodbit fb_script = fb.GetComponent<Foodbit>();
			Vector3 fb_pos = fb_script.transform.localPosition;
			pos = Utility.RandomFlatVec (-spore_range,
		                                 Foodbit.foodbitHeight / 2,
		                                 spore_range
										);
			
			Vector3 new_pos = fb_pos + pos;
			if (new_pos.x > wide_spread  || new_pos.x < -wide_spread
				|| new_pos.z > wide_spread || new_pos.z < -wide_spread)
			{
				new_pos = Utility.RandomFlatVec(-wide_spread,
					                         	Foodbit.foodbitHeight / 2,
					                         	wide_spread
					               		   	   );
			}
			
			newFoodbit(new_pos);
		}
	}
	
	public void removeFoodbit (GameObject fb) {
		foodbits.Remove(fb);	
	}
	
	public int getFoodbitCount () {
		return foodbits.Count;
	}
	
	public static Ether getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Ether";
			instance = container.AddComponent(typeof(Ether)) as Ether;
		}
		return instance;
	}
	
	public double getEnergy() {
		return energy;
	}
	
	public void subtractEnergy (double n) {
		energy -= n;
	}
	
	public void addToEnergy(double n) {
		energy += n;
		if (energy > total_energy) energy = total_energy;
	}

	public bool enoughEnergy(double n) {
		return energy >= n;
	}
	

}
