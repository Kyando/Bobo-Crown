using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class BasicEnemy : MonoBehaviour
{
    public List<Transform> pointsOfInterest;
    private Transform _targetPointOfInterest;


    private void Awake()
    {
        SelectRandomPointOfInterest();
    }

    private void SelectRandomPointOfInterest()
    {
        Transform nextPoint = _targetPointOfInterest;
        do
        {
            int pointIndex = Random.Range(0, pointsOfInterest.Count);
            nextPoint = pointsOfInterest[pointIndex];
        } while (nextPoint == _targetPointOfInterest);

        _targetPointOfInterest = nextPoint;
    }

    private void Update()
    {
        
    }
}