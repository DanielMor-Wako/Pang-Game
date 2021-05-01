//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallPop : MonoBehaviour
{
    public BallModel.BallPrefab nextPrefabOnPop;

    public UnityEvent OnPopEvent;

    void Awake()
    {
        if (OnPopEvent == null)
            OnPopEvent = new UnityEvent();
    }

    public void PopBall()
    {
        GameManager._instance.NotifyBallPopped(gameObject.tag);

        GameObject LeftBall = ObjectPoolList._instance.SpawnObject(nextPrefabOnPop.ToString(), transform.position);
        GameObject RightBall = ObjectPoolList._instance.SpawnObject(nextPrefabOnPop.ToString(), transform.position);

        if (LeftBall != null)
        {
            BallMovement LeftBallMove = LeftBall.GetComponent<BallMovement>();
            if (LeftBallMove != null) {
                LeftBallMove.SetBallxDirection(-1);
                LeftBallMove.InitialJump();
            }
        }

        if (RightBall != null) {
            BallMovement RightBallMove = RightBall.GetComponent<BallMovement>();
            if (RightBallMove != null) {
                RightBallMove.SetBallxDirection(1);
                RightBallMove.InitialJump();
            }
        }

        gameObject.SetActive(false);
    }
}
