using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DantlerAI : ActionScript
{
    private bool wasDisabled = false;
    
    public override void Start()
    {
        base.Start();
        StartCoroutine(ChooseBehaviour());
    }

    void OnDisable()
    {
        if (!wasDisabled) wasDisabled = true;
    }

    void OnEnable()
    {
        if (wasDisabled) StartCoroutine(ChooseBehaviour()); // see if causes issue with start
    }

    private IEnumerator ChooseBehaviour()
    {
        while(true)
        {
            int i = Random.Range(0, 3);

            switch (i)
            {
                case 0:
                    yield return StartCoroutine(Explore());
                    break;

                case 1:
                    yield return StartCoroutine(Rest());
                    break;

                case 2:
                    yield return StartCoroutine(Eat());
                    break;
            }
        }
    }
}
