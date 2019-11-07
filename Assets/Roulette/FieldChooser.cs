using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldChooser : MonoBehaviour
{

    public GameObject gameFields;
    public GameObject cursor;
    public GameObject coins;

    //public FieldChooser(GameObject gameFields, GameObject cursor, GameObject coins)
    //{
    //    this.gameFields = gameFields;
    //    this.cursor = cursor;
    //    this.coins = coins;
    //}

    public void MoveCursorToClosestFieldOnThe(Side side)
    {
        switch (side)
        {
            case Side.LEFT:
                getClosestFieldOnTheLeft();
                break;
            case Side.RIGHT:
                getClosestFieldOnTheRight();
                break;
            case Side.UP:
                getClosestFieldOnTheUp();
                break;
            case Side.DOWN:
                getClosestFieldOnTheDown();
                break;
            default:
                Debug.Log("Incorrect way.");
                break;
        }
    }

    private void getClosestFieldOnTheLeft()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = cursor.transform.position;
        foreach (Transform field in gameFields.GetComponentsInChildren<Transform>())
        {
            if (!object.Equals(field.position, cursor.transform.position))
            {
                Vector3 directionToTarget = field.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr && directionToTarget.x < -0.1 && directionToTarget.z < 0.2 && directionToTarget.z > -0.2)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = field;
                }
            }
        }
        resolveClosestField(bestTarget);
    }

    private void getClosestFieldOnTheRight()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = cursor.transform.position;
        foreach (Transform field in gameFields.GetComponentsInChildren<Transform>())
        {
            if (!object.Equals(field.position, cursor.transform.position))
            {
                Vector3 directionToTarget = field.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr && directionToTarget.x > 0.1 && directionToTarget.z < 0.2 && directionToTarget.z > -0.2)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = field;
                }
            }
        }
        resolveClosestField(bestTarget);
    }

    private void getClosestFieldOnTheUp()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = cursor.transform.position;
        foreach (Transform field in gameFields.GetComponentsInChildren<Transform>())
        {
            if (!object.Equals(field.position, cursor.transform.position))
            {
                Vector3 directionToTarget = field.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr && directionToTarget.z > 0.1 && directionToTarget.z < 1)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = field;
                }
            }
        }
        resolveClosestField(bestTarget);
    }

    private void getClosestFieldOnTheDown()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = cursor.transform.position;
        foreach (Transform field in gameFields.GetComponentsInChildren<Transform>())
        {
            if (!object.Equals(field.position, cursor.transform.position))
            {
                Vector3 directionToTarget = field.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr && directionToTarget.z < -0.1)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = field;
                }
            }
        }
        resolveClosestField(bestTarget);
    }

    private void resolveClosestField(Transform bestTarget) 
    {
        if (!object.Equals(bestTarget, null))
        {
            Debug.Log("NAME: " + bestTarget.name);
            cursor.transform.position = bestTarget.position;
            if (checkIfPosEmpty(bestTarget.position))
            {
                CursorView.setAsNoBets(cursor);
            }
            else
            {
                CursorView.setAsHasBets(cursor);
            }
        }
        else
        {
            Debug.Log("WRONG WAY!!!!!!!!!!!!!!" + Side.DOWN);
        }
    }


    private bool checkIfPosEmpty(Vector3 targetPos)
    {
        GameObject[] allMovableThings = GameObject.FindGameObjectsWithTag("bet");
        foreach (GameObject current in allMovableThings)
        {
            if (current.transform.position == targetPos)
                return false;
        }
        return true;
    }
}
