using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Data : MonoBehaviour
{
    public delegate void Count();
    public static event Count DataUpdated;

    public static GameObject container;
    public static Data instance;

    public List<int> creature_population;
    public List<int> foodbit_population;

    public CreatureCount cc;
    public FoodbitCount fc;

    private UIElement ui_element;

    float log_time;

    public static Data getInstance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "Data";
            instance = container.AddComponent<Data>();
        }
        return instance;
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {

    }

    public void OnVisible()
    {
        if (ui_element.visible)
        {
        }

        if (!ui_element.visible)
        {
        }
    }

    void Start ()
    {
        ui_element = GetComponent<UIElement>();
        cc = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
        fc = GameObject.Find("FoodbitCount").GetComponent<FoodbitCount>();
        creature_population = new List<int>();
        foodbit_population = new List<int>();
        log_time = float.Parse(Settings.getInstance().contents["config"]["log_time"].ToString());

        InvokeRepeating("UpdateCounts", 0F, log_time);
    }

    private void UpdateCounts ()
    {
        creature_population.Add(cc.number_of_creatures);
        foodbit_population.Add(fc.fbit_count);
        DataUpdated();
    }
}
