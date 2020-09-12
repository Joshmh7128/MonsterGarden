using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirbAI : ActionScript
{
    public bool onGround;
    private bool wasDisabled = false;
    
    public override void Start()
    {
        base.Start();
        onGround = (startY <= 1); // if startY < 1, its probably on the ground // ** for now
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
        //while(this.gameObject.activeSelf) // *** could be an issue later when monsters aren't there at start (look into)
        // **** also maybe change ratios bc birds rest and fly a LOT
        while (true)
        {
            /*int i = Random.Range(0,4);
            switch(i)
            {
                case 0: // Fly/Explore OR Hop
                    if (onGround) yield return StartCoroutine(Hop());
                    else yield return StartCoroutine(FlyAround());
                    break;
                    
                case 1: // Move To Ground OR Move to Tree
                    yield return StartCoroutine(ChangeBetweenGroundAndSky(onGround));
                    break;

                case 2: // Eat
                    yield return StartCoroutine(Eat());
                    break;

                case 3: // Rest (chill at tree)
                    yield return StartCoroutine(Rest());
                    break;

                default:
                    Debug.Log("yoinks");
                    break;
            }*/
        }
    }
}
