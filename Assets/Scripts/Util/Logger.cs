using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

 
public class Logger : MonoBehaviour {
	
	public static GameObject container;
	public static Logger instance;

	Settings settings;
	CreatureCount cc;
	FoodbitCount fc;

	int log_pop_data;
	int log_fbit_data;

	float log_time;
	string data_folder = "data";
	string crt_count_filename;
	string fbit_count_filename;

	public static Logger getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Logger";
			instance = container.AddComponent<Logger>();
		}
		return instance;
	}

	void Start () {
		settings = Settings.getInstance();
		cc = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		fc = GameObject.Find("FoodbitCount").GetComponent<FoodbitCount>();

		if (!Directory.Exists(Application.dataPath + "/" + data_folder))
			System.IO.Directory.CreateDirectory(Application.dataPath + "/" + data_folder);

		log_pop_data = (int) settings.contents["config"]["population_logging"];
		log_fbit_data = (int) settings.contents["config"]["foodbit_logging"];

		String unixTime = Utility.UnixTimeNow().ToString();

		if (log_pop_data == 1) {
			log_time = float.Parse( settings.contents["config"]["log_time"].ToString() );
			crt_count_filename = "creatures-"+unixTime;
			write( log_time.ToString(), crt_count_filename );
			InvokeRepeating("log_crt",0,log_time);
		}

		log_fbit_data = (int) settings.contents["config"]["foodbit_logging"];
		if (log_fbit_data == 1) {
			log_time = float.Parse( settings.contents["config"]["log_time"].ToString() );
			fbit_count_filename = "foodbits-"+unixTime;
			write( log_time.ToString(), fbit_count_filename );
			InvokeRepeating("log_fbit",0,log_time);
		}
	}

	void log_crt () {
		write( ","+cc.number_of_creatures, crt_count_filename );
	}

	void log_fbit () {
		write( ","+fc.fbit_count, fbit_count_filename );
	}
	
	void write (String str, String file) {
		File.AppendAllText(Application.dataPath + "/" + data_folder + "/" + file + ".csv", str);
	}

}
