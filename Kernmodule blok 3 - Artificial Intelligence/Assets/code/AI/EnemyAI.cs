using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * This MonoBehaviour loops through all of the conditions to check if they return true.
 * It then checks if this condition's associatedAction has a higher priority than the previously highest one.
 * It then returns the condition's index that has the highest priority action.
 * Using that integer, it executes the associated action. This way, it prioritises all current conditions and executes the most important action right now.
 * 
 * TL;DR: It executes actions by priority.
 * 
 * Alles heb ik zelfgemaakt, inclusief de pathfinding, maar ik comment als ik alleen werk vaak tweetalig, excuses dus voor de gemixte comments!
 * 
 */

public class EnemyAI : MonoBehaviour {

    public ConditionBase[] conditions; /////put all of your conditions into this array

    private void FixedUpdate()
    {
        if (GameManager.instance.executeAI)
        {
            ExecuteAgentActions(GetHighestPriorityCondition()); ////executes the associated action of all current conditions that has the highest actionPriority
        }
    }

    private int GetHighestPriorityCondition() /////Returns the condition's index in conditions[] that returns true and whose action has the highest priority
    {
        int conditionIndexToReturn = 0;
        float highestPriorityYet = 0;

        for(int i = 0; i < conditions.Length; i++)
        {
            if (conditions[i].Condition() && conditions[i].associatedAction.actionPriority > highestPriorityYet)
            {
                    conditionIndexToReturn = i;
            }
        }

        return conditionIndexToReturn;
    }

    private void ExecuteAgentActions(int conditionIndex) /////executes the condition at index condition's associated action.
    {
        if (conditions[conditionIndex].Condition())
        { 
            conditions[conditionIndex].associatedAction.Action();
            Debug.Log("Current action: " + conditions[conditionIndex].associatedAction);
        }
    }

}
