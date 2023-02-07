using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite Icon;

    public Item(int id, string _name, string description, Sprite icon)
    {
        ID = id;
        Name = _name;
        Description = description;
        Icon = icon;
    }
}


   

