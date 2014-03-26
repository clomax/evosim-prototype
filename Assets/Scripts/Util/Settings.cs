using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class Settings : MonoBehaviour {
	
	string settings_file = "settings.json";
	StreamReader sr;
	string raw_contents;
	public JsonData contents;
	
	public static GameObject container;
	public static Settings instance;
	
	public Settings () {
		sr = new StreamReader(Application.dataPath + "/Resources/" + settings_file);
		raw_contents = sr.ReadToEnd();
		contents = JsonMapper.ToObject(raw_contents);
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
