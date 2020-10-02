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
                    if (home != null)
                    {
                        yield return RestAtHome();
                    }
                    else
                    {
                        yield return Rest();
                    }
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
    
    void CheckRequirements() // todo: see if best way to do this but feels good
    {
        // ** Claiming Home after eating/resting after arriving is within the Eating/Resting scripts
        if (home == null && happiness < 1)
        {
            LeaveGarden();
        }
        // if no home
            // if rested or ate once and a home available
                // ClaimHome
            // if no home available FIGHT FOR HOME
        // if home has no home due to removal, never claiming one, loses home in turf dispute, etc. and happiness is less than 1
            // Leave garden
    }
}
