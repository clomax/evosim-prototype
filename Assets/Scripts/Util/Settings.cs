using UnityEngine;
using System.Collections;
using System.IO;
using SimpleJSON;

public class Settings : MonoBehaviour {
	
	string settings_file = "settings.json";
	StreamReader sr;
	string _contents;
	public JSONNode contents;
	
	public static GameObject container;
	public static Settings instance;
	
	public Settings () {
		sr = new StreamReader(Application.dataPath + "/" + settings_file);
		string _contents = sr.ReadToEnd();
		contents = JSON.Parse(_contents);
		sr.Close();
	}
	
	public static Settings getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Settings";
			instance = container.AddComponent(typeof(Settings)) as Settings;
		}
		return instance;
	}
}
