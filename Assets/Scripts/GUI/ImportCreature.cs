using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ImportCreature : MonoBehaviour
{
    string creatures_folder;
    string[] creature_files;

    StreamReader sr;
    string raw_contents;
    public JsonData contents;
    public SortedList<string, Chromosome> creatures;

    public GameObject selection_prefab;
    private UIElement ui_element;

    List<GameObject> selections;

    GameObject s;

    void OnEnable ()
    {
        SaveCreature.CreatureSaved += LoadCreatures;
    }

    void OnDisable()
    {
        SaveCreature.CreatureSaved -= LoadCreatures;
    }

    void Start ()
    {
        creatures = new SortedList<string, Chromosome>();
        creatures_folder = Application.dataPath + "/data/saved_creatures";
        ui_element = GetComponent<UIElement>();
        selections = new List<GameObject>();
    }

    public void OnVisible()
    {
        if (ui_element.visible)
        {
            LoadCreatures();
            PopulateMenu();
        }

        if(!ui_element.visible)
        {
            foreach(var s in selections)
            {
                GameObject.Destroy(s);
            }
        }
    }

    void LoadCreatures()
    {
        creatures.Clear();
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

                string name = contents["name"].ToString();

                Color root_col = new Color();
                root_col.r = float.Parse(contents["attributes"]["colour"]["r"].ToString());
                root_col.g = float.Parse(contents["attributes"]["colour"]["g"].ToString());
                root_col.b = float.Parse(contents["attributes"]["colour"]["b"].ToString());
                root_col.a = 1;

                Color limb_col = new Color();
                limb_col.r = float.Parse(contents["attributes"]["limb_colour"]["r"].ToString());
                limb_col.g = float.Parse(contents["attributes"]["limb_colour"]["g"].ToString());
                limb_col.b = float.Parse(contents["attributes"]["limb_colour"]["b"].ToString());
                limb_col.a = 1;

#if ZERO
                Vector3 root_scale = new Vector3();
                root_scale.x = float.Parse(contents["attributes"]["root_scale"]["x"].ToString());
                root_scale.y = float.Parse(contents["attributes"]["root_scale"]["y"].ToString());
                root_scale.z = float.Parse(contents["attributes"]["root_scale"]["z"].ToString());

                float bjf = (int) contents["attributes"]["base_joint_frequency"];
                float bja = (int) contents["attributes"]["base_joint_amplitude"];
                float bjp = (int) contents["attributes"]["base_joint_phase"];
                float ht =  (int) contents["attributes"]["hunger_threshold"];
#endif

                Chromosome chromosome = new Chromosome();

                chromosome.colour = root_col;
                chromosome.limb_colour = limb_col;

                creatures.Add(name, chromosome);
            }
        }
    }

    void PopulateMenu ()
    {
        for (int i=0; i<creatures.Count; ++i)
        {
            s = Instantiate(selection_prefab, selection_prefab.transform.position, selection_prefab.transform.rotation) as GameObject;
            s.transform.SetParent(transform, false);
            selections.Add(s);
            s.GetComponent<RectTransform>().localPosition += new Vector3(0, (-40F*i), 0);
            Button b = s.GetComponent<Button>();
            Text t = b.GetComponentInChildren<Text>();
            t.text = creatures.Keys[i];

            Image[] colours = new Image[2];
            colours = GameObject.FindObjectsOfType<Image>();
            colours[0].color = creatures.Values[i].colour;
            colours[1].color = creatures.Values[i].limb_colour;
        }
    }
}
