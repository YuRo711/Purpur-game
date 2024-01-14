using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameTimer : MonoBehaviour
{
    [field: SerializeField] public float TimeDuration { get; set; }
    [field: SerializeField] public float Acceleration { get; private set; }
    [field: SerializeField] public float MinDuration { get; private set; }
    public float TimeRemaining { get ; private set; }

    public bool TimeIsUp
        => TimeRemaining <= 0;

    public void Restart()
    {
        TimeRemaining = TimeDuration;
        TimeDuration = Math.Max(MinDuration, TimeDuration - Acceleration);
    }

    private void Update()
    {
        TimeRemaining = Math.Max(0, TimeRemaining - Time.deltaTime);
    }
}