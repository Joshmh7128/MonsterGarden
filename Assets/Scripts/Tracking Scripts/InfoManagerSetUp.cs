using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManagerSetUp : MonoBehaviour
{
    // Puts stuff into the static script to avoid GetComponent
    void Start()
    {
        WorldManager.currentInfoManager = this.gameObject;
        WorldManager.currentAreaInfoScript = this.gameObject.GetComponent<AreaInfoScript>(); 
    }
}
