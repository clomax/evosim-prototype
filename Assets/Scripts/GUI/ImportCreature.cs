using UnityEngine;
using LitJson;
using System.Collections;
using System.IO;

public class ImportCreature : MonoBehaviour
{
    string creatures_folder;
    string[] creature_files;

    StreamReader sr;
    string raw_contents;
    public JsonData contents;

    void Start ()
    {
        creatures_folder = Application.dataPath + "/data/saved_creatures";
    }

    public void OnVisible ()
    {
        // get all files
        string[] fs;
        creature_files = Directory.GetDirectories(creatures_folder);
        foreach (var s in creature_files)
        {
            fs = Directory.GetFiles(s, "*.json");
            foreach (var f in fs)
            {
                sr = new StreamReader(f);
                raw_contents = sr.ReadToEnd();
                contents = JsonMapper.ToObject(raw_contents);
                sr.Close();
            }
        }
    }
}
