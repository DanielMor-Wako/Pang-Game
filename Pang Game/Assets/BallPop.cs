﻿//using System.Collections.Generic;
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

    public void PopBall()
    {
        Debug.Log(gameObject.tag+" has poped");

        GameObject LeftBall = ObjectPoolList._instance.SpawnObject(nextPrefabOnPop.ToString(), transform.position);
        GameObject RightBall = ObjectPoolList._instance.SpawnObject(nextPrefabOnPop.ToString(), transform.position);

        if (LeftBall != null)
        {
            BallMovement LeftBallMove = LeftBall.GetComponent<BallMovement>();
            if (LeftBallMove != null) { LeftBallMove.SetBallxDirection(-1); }
        }

        if (RightBall != null) {
            BallMovement RightBallMove = RightBall.GetComponent<BallMovement>();
            if (RightBallMove != null) { RightBallMove.SetBallxDirection(1); }
        }
    }
}
