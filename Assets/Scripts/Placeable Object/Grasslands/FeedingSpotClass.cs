using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedingSpotClass : MonoBehaviour
{
    // script works with feeding spots
    public enum FeedingSpotTypes { none, grass, grain, leaves}
    // what are you
    public FeedingSpotTypes feedingSpotType;
    // food percentage
    [SerializeField] int foodPercentage;
    // food regen speed
    [SerializeField] int regenSpeed;
    // food regen increment
    [SerializeField] int regenIncrement;
    // do we have food?
    public bool hasFood;
    // who is our manager
    ObjectTrackingClass objectTrackingClass;
    // what is our position
    public Transform ourTransform;

    // start runs at the start of the object
    private void Start()
    {
        // find our tracker
        objectTrackingClass = WorldManager.objectTrackingClass;
        // depending on our food type add ourselves to the list
        switch (feedingSpotType)
        {
            case FeedingSpotTypes.grass:
                objectTrackingClass.grassFeedingSpotList.Add(this);
                break;

            case FeedingSpotTypes.grain:
                objectTrackingClass.grainFeedingSpotList.Add(this);
                break;

            case FeedingSpotTypes.leaves:
                objectTrackingClass.leavesFeedingSpotList.Add(this);
                break;
        }
    }

    // what happens when we are eaten?
    public void Eaten()
    {
        hasFood = false;
        foodPercentage = 0;
        StartCoroutine("FoodRegen");
    }

    private IEnumerator FoodRegen()
    {
        // wait
        yield return new WaitForSeconds(regenSpeed);
        // add to our increment
        foodPercentage += regenIncrement;
        // check to see if we are at 100 yet
        if (foodPercentage < 100)
        {   // if there's more to go, regen more
            hasFood = false;
            StartCoroutine("FoodRegen");
        }
        else if (foodPercentage >= 100)
        {
            hasFood = true;
        }
    }
}
