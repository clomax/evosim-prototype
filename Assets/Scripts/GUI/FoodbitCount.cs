using UnityEngine;
using System.Collections;

public class FoodbitCount : MonoBehaviour {

	public Ether eth;
	Settings settings;
	Logger lg;
	
	int log_data;
	float log_time;
	string filename;
	
	void Start() {
		name = "FoodbitCount";
		eth = Ether.getInstance();
		settings = Settings.getInstance();
		lg = Logger.getInstance();
		
		log_data = (int) settings.contents["config"]["foodbit_logging"];
		if (log_data == 1) {
			log_time = float.Parse( settings.contents["config"]["log_time"].ToString() );
			filename = "foodbits-"+Utility.UnixTimeNow().ToString();
			lg.write( log_time.ToString(), filename );
			InvokeRepeating("log",0,log_time);
		}
	}

	void Update ()	{
		guiText.text = "Foodbits: " + eth.getFoodbitCount();
	}
	
	void log () {
		lg.write( ","+eth.getFoodbitCount().ToString(), filename );
	}
}
