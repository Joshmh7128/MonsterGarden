using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DantlerAIInherit : ActionScript
{
    public override void Start()
    {
        base.Start();
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
