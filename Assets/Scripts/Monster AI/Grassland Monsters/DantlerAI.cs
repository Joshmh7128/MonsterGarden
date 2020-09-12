using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JM.LinqFaster;

public class DantlerAI : ActionScript
{
    public override void Start()
    {
        base.Start();
        // statuses of actions that shouldn't be interrupted by other actions // ** changed around as needed
        uninterruptableActions = new List<string>{"Eating", "Resting", "Finding Resting Spot", "Finding Food", "Herding", "Leading Herd"};
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
                case 3: // ** Herding
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
                case 4: // ** Seeking Fight

                    /*yield return new WaitForEndOfFrame();
                    // todo: expensive!!!!!!! too many getcomponents!!!!!!!!!!
                    List<GameObject> temp = InterruptableMonsters(new List<GameObject>(areaInfoScript.MonsterList.Keys));
                    GameObject enemyToFight = temp[Random.Range(0, temp.Count)];
                    areaInfoScript.MonsterList[enemyToFight].StopAllCoroutines();
                    yield return GeneralTurfDispute(enemyToFight);
                    break;
                    */
                    // ** temp = InterruptableMonsters(new List<GameObject>(areaInfoScript.MonsterList.Keys));
                    if (home != null)
                    {
                        /*
                        1) Get Monsters within radius (overlap sphere with layer)
                        2) Choose random monster (will be the collider) and get the game object
                        3) Stop that monsters coroutines
                        4) Start the turf dispute
                        */
                    }
                    else
                    {
                        /*
                        1) Get list of monsters who have a home that you can live in
                        2) Choose random monster from list
                        3) Stop that monsters coroutines
                        4) Start the turf dispute
                        */
                    }
                    break;
            }
        }
    }
}
