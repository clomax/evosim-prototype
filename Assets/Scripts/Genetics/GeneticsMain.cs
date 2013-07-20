using UnityEngine;
using System.Collections;


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
	
	void Start () {
		spw = Spawner.getInstance();
		settings = Settings.getInstance();
		eth = Ether.getInstance();
		
		double energy = (double)settings.contents["creature"]["init_energy"];
		
		for (int i=0; i<20; i++) {
			spw.spawn(Utility.RandomFlatVec(-200,5,200), Utility.RandomRotVec(), energy);
			eth.subtractEnergy(energy);
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
