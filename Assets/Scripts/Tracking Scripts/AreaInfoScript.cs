using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaInfoScript : MonoBehaviour
{
    // Keeps track of whats in the area
    public Dictionary<GameObject, ActionScript> MonsterList = new Dictionary<GameObject, ActionScript>();
    public List<GameObject> PlantList = new List<GameObject>();
    public List<GameObject> ObjectList = new List<GameObject>();
}
