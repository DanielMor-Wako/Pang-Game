//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallPop : MonoBehaviour
{
    public enum BallPrefab { None, BallSize_1, BallSize_2, BallSize_3, BallSize_4, BallSize_5, BallSize_6 }
    public BallPrefab nextPrefabOnPop;
    
    public UnityEvent OnPopEvent;

    void Awake()
    {
        if (OnPopEvent == null)
            OnPopEvent = new UnityEvent();
    }
}
