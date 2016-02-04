using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class Ether : MonoBehaviour
{	
	public static GameObject container;
	public static Ether instance;
	
	GameObject foodbit;

	Logger lg;
	Settings settings;
    Data data;
    public Spawner spawner;
	
	public decimal total_energy;
	public decimal energy;
	decimal foodbit_energy;
    float init_energy_min;
    float init_energy_max;
    float init_scale_min;
    float init_scale_max;

    Vector3 pos;

	float 	wide_spread;
	int		start_number_foodbits;
	float 	spore_time;
	int 	spore_range;

    public ArrayList creatures;
	public ArrayList foodbits;

    public delegate void EtherInfo(decimal energy);
    public static event EtherInfo EnergyUpdated;
    public static event EtherInfo EnergyInitialised;

    public delegate void FoodbitInfo(int count);
    public static event FoodbitInfo FoodbitsUpdated;

    void OnEnable ()
    {
        Spawner.CreatureSpawned += OnCreatureSpawned;
        Creature.CreatureDead += OnCreatureDeath;
    }

    void OnDisable()
    {
        Spawner.CreatureSpawned -= OnCreatureSpawned;
        Creature.CreatureDead -= OnCreatureDeath;
    }

    void Start ()
    {
		foodbit = (GameObject)Resources.Load("Prefabs/Foodbit");
		string name = this.name.ToLower();
		
		settings = Settings.getInstance();
        data = Data.getInstance();
        spawner = Spawner.getInstance();
		
		total_energy = 			decimal.Parse(settings.contents[name]	["total_energy"].ToString());
		start_number_foodbits = (int)	 	settings.contents[name]	["start_number_foodbits"];
		spore_range = 			(int)	 	settings.contents["foodbit"]["spore_range"];
		wide_spread = 			float.Parse(settings.contents["foodbit"]["wide_spread"].ToString() );
		spore_time = 			float.Parse(settings.contents["foodbit"]["spore_time"].ToString() );

        init_energy_min = float.Parse(settings.contents["foodbit"]["init_energy_min"].ToString());
        init_energy_max = float.Parse(settings.contents["foodbit"]["init_energy_max"].ToString());

        init_scale_min = float.Parse(settings.contents["foodbit"]["init_scale_min"].ToString());
        init_scale_max = float.Parse(settings.contents["foodbit"]["init_scale_max"].ToString());

        energy = total_energy;
        EnergyInitialised(energy);

        creatures = new ArrayList();
		foodbits = new ArrayList();
		
		for (int i=0; i<start_number_foodbits; i++)
        {
			Vector3 pos = Utility.RandomVec(-wide_spread,
				                             wide_spread,
				                             wide_spread
				               				);
			newFoodbit(pos);
		}

        InvokeRepeating("FixEnergyLeak", 1F*60F, 1F*60F);
		InvokeRepeating("fbSpawn",spore_time, spore_time);
	}

    void OnCreatureSpawned (Creature c)
    {
        creatures.Add(c);
    }

    void OnCreatureDeath (Creature c)
    {
        creatures.Remove(c);
    }
	
	public void newFoodbit (Vector3 pos)
    {
        foodbit_energy = (decimal)Random.Range(init_energy_min, init_energy_max);
        if (enoughEnergy(foodbit_energy))
        {
			GameObject fb = (GameObject)Instantiate(foodbit, pos, Quaternion.identity);
			Foodbit fb_s = fb.AddComponent<Foodbit>();
            fb_s.energy = foodbit_energy;
			subtractEnergy(foodbit_energy);
            float scale = Utility.ConvertRange((float)foodbit_energy, init_energy_min, init_energy_max, init_scale_min, init_scale_max);
            fb.transform.localScale = new Vector3(scale, scale, scale);
            foodbits.Add(fb);
            FoodbitsUpdated(foodbits.Count);
		}
	}
	
	private void fbSpawn ()
    {
		int fb_count = getFoodbitCount();
		if (fb_count >= 1)
        {
			int fb_index = Random.Range(0,fb_count);
			GameObject fb = (GameObject) foodbits[fb_index];
			Foodbit fb_script = fb.GetComponent<Foodbit>();
            foodbit_energy = fb_script.energy;
			Vector3 fb_pos = fb_script.transform.localPosition;
			pos = Utility.RandomVec (-spore_range,
	                                 Foodbit.foodbitHeight / 2,
	                                 spore_range
									);
			
			Vector3 new_pos = fb_pos + pos;
			if (new_pos.x > wide_spread  || new_pos.x < -wide_spread
				|| new_pos.z > wide_spread || new_pos.z < -wide_spread)
			{
				new_pos = Utility.RandomVec(-wide_spread,
					                         wide_spread,
					                         wide_spread
					               		   );
			}
			
			newFoodbit(new_pos);
		}
	}
	
	public void removeFoodbit (GameObject fb)
    {
        FoodbitsUpdated(foodbits.Count);
        foodbits.Remove(fb);
	}
	
	public int getFoodbitCount ()
    {
		return foodbits.Count;
	}
	
	public static Ether getInstance ()
    {
		if(!instance)
        {
			container = new GameObject();
			container.name = "Ether";
			instance = container.AddComponent(typeof(Ether)) as Ether;
		}
		return instance;
	}
	
	public decimal getEnergy()
    {
		return energy;
	}

    public void addEnergy (decimal n)
    {
        energy += n;
        EnergyUpdated(energy);
    }
	
	public void subtractEnergy (decimal n)
    {
        energy -= n;
        EnergyUpdated(energy);
	}

	public bool enoughEnergy(decimal n)
    {
		return energy >= n;
	}
	
    private void FixEnergyLeak ()
    {
        decimal total_crt = data.TotalCreatureEnergy();
        decimal total_fb = data.TotalFoodbitEnergy();
        decimal total = energy + total_crt + total_fb;
        print("crt: " + total_crt + "     fb: " + total_fb + "     ether: " + energy + "        total: " + total);
        if (total != total_energy)
        {
            decimal fix = total - total_energy;
            print("Fixing energy leak... "+fix);
            subtractEnergy(fix);
        }
    }
}
