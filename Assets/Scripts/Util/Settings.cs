using UnityEngine;
using System.Collections;
using System.IO;

public class Settings : MonoBehaviour {
	
	string settings_file = "settings.cfg";
	StreamReader sr;
	
	public static GameObject container;
	public static Settings instance;
	
	public Settings () {
		sr = new StreamReader(Application.dataPath + "/" + settings_file);
	}
	
	public static Settings getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Settings";
			instance = container.AddComponent(typeof(Settings)) as Settings;
		}
		return instance;
	}
	
	public string read() {
		string contents = sr.ReadToEnd();
		return contents;
	}
}
