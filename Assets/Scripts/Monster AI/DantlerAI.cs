using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DantlerAI : MonoBehaviour
{
    Stats monsterStats;
    ActionScript actionScript;

    private void Start()
    {
        monsterStats = gameObject.GetComponent<Stats>();
        actionScript = WorldManager.actionScript;
        StartCoroutine(ChooseBehaviour());
    }

    private IEnumerator ChooseBehaviour()
    {
        //while(this.gameObject.activeSelf) // *** could be an issue later when monsters aren't there at start (look into)
        while(true)
        {
            int i = Random.Range(0, 3);

            switch (i)
            {
                case 0:
                    yield return StartCoroutine(actionScript.Explore(this.gameObject, monsterStats));
                    break;

                case 1:
                    yield return StartCoroutine(actionScript.Rest(this.gameObject, monsterStats));
                    break;

                case 2:
                    yield return StartCoroutine(actionScript.Eat(this.gameObject, monsterStats));
                    break;
            }
        }
    }
}
