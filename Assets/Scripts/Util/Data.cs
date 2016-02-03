using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Data : MonoBehaviour
{
    public delegate void Count();
    public static event Count DataUpdated;

    public static GameObject container;
    public static Data instance;

    Ether eth;

    public List<int> creature_population;
    public List<int> foodbit_population;

    public CreatureCount cc;

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
        creature_population = new List<int>();
        eth = Ether.getInstance();
        foodbit_population = new List<int>();
        log_time = float.Parse(Settings.getInstance().contents["config"]["log_time"].ToString());

        InvokeRepeating("UpdateCounts", 0F, log_time);
    }

    private void UpdateCounts ()
    {
        foodbit_population.Add(eth.getFoodbitCount());
        DataUpdated();
    }

    public decimal TotalCreatureEnergy()
    {
        decimal result = 0m;
        foreach(Creature c in eth.creatures)
        {
            result += c.energy;
        }
        return (result);
    }

    internal decimal TotalFoodbitEnergy()
    {
        decimal result = 0m;
        foreach (GameObject f in eth.foodbits)
        {
            result += f.GetComponent<Foodbit>().energy;
        }
        return (result);
    }
}
