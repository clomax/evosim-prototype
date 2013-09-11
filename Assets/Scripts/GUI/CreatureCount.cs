using UnityEngine;
using System.Collections;

public class CreatureCount : MonoBehaviour {

	public int number_of_creatures = 0;
	float log_time;
	string filename;
	
	Settings settings;
	Logger lg;
	
	void Start () {
		settings = Settings.getInstance();
		lg = Logger.getInstance();
		
		log_time = float.Parse( settings.contents["config"]["log_time"].ToString() );
		filename = "creatures-"+Utility.UnixTimeNow().ToString();
		lg.write( log_time.ToString()+",", filename );
		InvokeRepeating("log",0,log_time);
	}
	
	void Update ()	{
		guiText.text = "Creatures: " + number_of_creatures;
	}
	
	void log () {
		lg.write( number_of_creatures.ToString()+",", filename );
	}
}
