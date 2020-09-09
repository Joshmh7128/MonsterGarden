using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JM.LinqFaster;

public class DantlerAI : ActionScript
{
    // statuses of actions that shouldn't be interrupted by other actions // ** changed around as needed
    private List<string> uninterruptableActions = new List<string>{"Eating", "Resting", "Finding Resting Spot", "Finding Food", "Herding", "Leading Herd"};
    
    public override void Start()
    {
        base.Start();
        StartCoroutine(ChooseBehaviour());
    }

    void OnEnable()
    {
        // if monster was disabled, need to restart ChooseBehavior
        if (wasDisabled) 
        {
            StartCoroutine(ChooseBehaviour());
        }
    }

    private IEnumerator ChooseBehaviour()
    {
        yield return Explore();
        while(true)
        {
            int i = Random.Range(0, 4);
            currentStatus.text = null;
            switch (i)
            {
                case 0:
                    yield return Explore();
                    break;

                case 1:
                    yield return Rest();
                    break;

                case 2:
                    yield return Eat();
                    break;
                case 3:
                    yield return new WaitForEndOfFrame();
                    Debug.Log(this.gameObject.name);
                    List<GameObject> followerList = new List<GameObject>(); // monsters following this monster
                    foreach(GameObject go in friendList) // ** change to be other monsters outside of friendlist maybe
                    {
                        ActionScript temp = go.GetComponent<ActionScript>();
                        if (!uninterruptableActions.Contains(temp.currentStatus.text))
                        {
                            followerList.Add(go);
                            temp.currentStatus.text = "Herding";
                            temp.StopCurrentCoroutines();
                        }
                    }
                    yield return Herding(followerList);
                    break;
                case 4:
                    yield return new WaitForEndOfFrame();
                    // todo: 
                    List<GameObject> tempGameObjectList = friendList.WhereF(x => !uninterruptableActions.Contains(x.GetComponent<ActionScript>().currentStatus.text));
                    GameObject enemyToFight = tempGameObjectList[Random.Range(0, tempGameObjectList.Count)];
                    enemyToFight.GetComponent<ActionScript>().StopAllCoroutines();
                    yield return GeneralTurfDispute(enemyToFight);
                    break;
            }
        }
    }
}
