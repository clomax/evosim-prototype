using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Linq;
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
    public static Data data_instance;

	Settings settings;
	CreatureCount cc;
	FoodbitCount fc;

	int log_pop_data;
	int log_fbit_data;

	float log_time;
	string data_folder = "data";
	string crt_count_filename;
	string fbit_count_filename;

    FileStream fs;

	public static Logger getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Logger";
			instance = container.AddComponent<Logger>();
		}
		return instance;
	}

    void OnEnable ()
    {
        Data.DataUpdated += log;
    }

    void OnDisable()
    {
        Data.DataUpdated -= log;
    }

    void Start () {
        data_instance = Data.getInstance();
		settings = Settings.getInstance();
		cc = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		fc = GameObject.Find("FoodbitCount").GetComponent<FoodbitCount>();

		log_pop_data = (int) settings.contents["config"]["population_logging"];
        log_fbit_data = (int)settings.contents["config"]["foodbit_logging"];
        log_time = float.Parse(settings.contents["config"]["log_time"].ToString());

        String unixTime = Utility.UnixTimeNow().ToString();
        crt_count_filename = "creatures-" + unixTime;
        fbit_count_filename = "foodbits-" + unixTime;

        if (log_pop_data == 1) {
			write( log_time.ToString(), crt_count_filename );
		}

		log_fbit_data = (int) settings.contents["config"]["foodbit_logging"];
		if (log_fbit_data == 1) {
			write( log_time.ToString(), fbit_count_filename );
		}
	}

	private void log () {
		if(log_pop_data != 0) write( ","+data_instance.creature_population.Last(), crt_count_filename );
		if(log_fbit_data != 0) write( ","+data_instance.foodbit_population.Last(), fbit_count_filename );
	}
	
	private void write (String str, String file) {
        fs = new FileStream(Application.dataPath + "/" + data_folder + "/" + file + ".csv", FileMode.Append);
        using (StreamWriter sw = new StreamWriter(fs))
        {
            sw.Write(str);
        }
	}

}
