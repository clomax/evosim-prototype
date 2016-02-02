﻿using UnityEngine;
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

    public List<Creature> creatures;
    Ether eth;

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
        Creature.CreatureDead += OnCreatureDead;
    }

    void OnDisable()
    {
        Creature.CreatureDead -= OnCreatureDead;
    }

    void OnCreatureDead (Creature c)
    {
        creatures.Remove(c);
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
        creatures = new List<Creature>();
        eth = Ether.getInstance();
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

    public decimal TotalCreatureEnergy()
    {
        decimal result = 0m;
        foreach(var c in creatures)
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
