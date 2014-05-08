using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class GeneticsMain : MonoBehaviour {

	private Ether eth;
	private Settings settings;
	private Spawner spw;
	public static GameObject container;
	public static GeneticsMain instance;
	
	Chromosome chromosome;
	
	int starting_creatures;
	float creature_spread;
	
	Vector3 max_root_scale;
	Vector3 min_root_scale;

	Vector3 max_limb_scale;
	Vector3 min_limb_scale;
	
	
	void Start () {
		spw = Spawner.getInstance();
		settings = Settings.getInstance();
		eth = Ether.getInstance();
		
		max_root_scale 		= new Vector3();
		max_root_scale.x 	= float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["x"].ToString() );
		max_root_scale.y 	= float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["y"].ToString() );
		max_root_scale.z 	= float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["z"].ToString() );
		
		min_root_scale 		= new Vector3();
		min_root_scale.x 	= float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["x"].ToString() );
		min_root_scale.y 	= float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["y"].ToString() );
		min_root_scale.z 	= float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["z"].ToString() );



		max_limb_scale		= new Vector3();
		max_limb_scale.x 	= float.Parse( settings.contents["creature"]["limb"]["max_limb_scale"]["x"].ToString() );
		max_limb_scale.y 	= float.Parse( settings.contents["creature"]["limb"]["max_limb_scale"]["y"].ToString() );
		max_limb_scale.z 	= float.Parse( settings.contents["creature"]["limb"]["max_limb_scale"]["z"].ToString() );

		min_limb_scale 		= new Vector3();
		min_limb_scale.x 	= float.Parse( settings.contents["creature"]["limb"]["min_limb_scale"]["x"].ToString() );
		min_limb_scale.y 	= float.Parse( settings.contents["creature"]["limb"]["min_limb_scale"]["y"].ToString() );
		min_limb_scale.z 	= float.Parse( settings.contents["creature"]["limb"]["min_limb_scale"]["z"].ToString() );



		starting_creatures	= (int) 		settings.contents["ether"]["starting_creatures"];
		creature_spread		= float.Parse(	settings.contents["ether"]["creature_spread"].ToString() );
		double creature_init_energy	= (double)		settings.contents["creature"]["init_energy"];
		int branch_limit 	= (int)			settings.contents["creature"]["branch_limit"];
		int recurrence_limit = (int)		settings.contents["creature"]["recurrence_limit"];

		
		/*
		 * For each new creature, generate random genes and spawn the bugger
		 */
		for (int i=0; i<starting_creatures; i++) {
			chromosome = new Chromosome();
			
			// random colours
			Color col = new Color( (float)Random.Range(0.0F,1.0F),
								   (float)Random.Range(0.0F,1.0F),
								   (float)Random.Range(0.0F,1.0F)
								 );
			chromosome.setColour(col.r, col.g, col.b);
			
			// random root scale
			Vector3 rootScale = new Vector3((float) Random.Range(min_root_scale.x,max_root_scale.x),
											(float) Random.Range(min_root_scale.y,max_root_scale.y),
											(float) Random.Range(min_root_scale.z,max_root_scale.z)
								  		   );
			chromosome.setRootScale(rootScale);
			
			// random initial limbs
			int bs = Random.Range (1,branch_limit+1);
			MultiDimList branches = new MultiDimList();

			for (int j=0; j<bs; j++) {
				branches.Add(new List<GameObject>());

				int recurrences = Random.Range(0,recurrence_limit);
				for (int k=0; k<=recurrences; k++) {

					branches[j].Add(new GameObject());
					GameObject limb = branches[j][k];
					Limb limb_script = limb.AddComponent<Limb>();

					/* Randomly select point on root's surface */
					Vector3 tmp = new Vector3(0F,0F,0F);
					// set all axes to random float between -.5 and .5
					for (int l=0; l<3; l++) {
						tmp[l] = Random.Range(-0.5F, 0.5F);
					}
					// randomly select between x,y,z
					int axis = Random.Range(0,3);
					// randomly set that axis to -.5 or .5
					int rnd = Random.Range(0,1);
					if (rnd == 0)	tmp[axis] = -0.5F;
					else 	 		tmp[axis] = 0.5F;
					// set anchor point to new vector
					Vector3 point = tmp;
					Vector3 scale = new Vector3 ((float) Random.Range(min_limb_scale.x,max_limb_scale.x),
					                             (float) Random.Range(min_limb_scale.y,max_limb_scale.y),
					                             (float) Random.Range(min_limb_scale.z,max_limb_scale.z)
					                            );

					// Generate joint parameters
					float freq = Random.Range(0F, 15F);
					float amp = Random.Range (0F, 30F);				

					point = Vector3.zero;	// Position is not a factor in child limbs, set it to zero
					scale = new Vector3 ((float) Random.Range(min_limb_scale.x,max_limb_scale.x),
		                                 (float) Random.Range(min_limb_scale.y,max_limb_scale.y),
		                                 (float) Random.Range(min_limb_scale.z,max_limb_scale.z)
		                                );

					Vector3 position = Utility.RandomPointInsideCube(rootScale);

					limb_script.create(col, position, scale, new Vector3(0.5F,0,0));
				}
			}

			chromosome.setBranches(branches);

			if (eth.enoughEnergy(creature_init_energy)) {
				spw.spawn(Utility.RandomFlatVec(-creature_spread,10,creature_spread), Utility.RandomRotVec(), creature_init_energy, chromosome);
				eth.subtractEnergy(creature_init_energy);
			}
		}
	}
	
	public static GeneticsMain getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "GeneticsMain";
			instance = container.AddComponent<GeneticsMain>();
		}
		return instance;
	}
}
