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
        uninterruptableActions = new List<string>{"Eating", "Resting", "Finding Resting Spot", "Finding Food", "Herding", "Leading Herd", "Getting Fought", "Fighting"};
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
                    yield return Herding();
                    break;
                case 4:
                    yield return TurfDispute();
                    break;
            }
        }
    }
}
