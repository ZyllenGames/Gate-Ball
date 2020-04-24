using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : GenericSingleton<GameObjectManager>
{
    List<GameObject> GateList;
    List<GameObject> BallList;
    GameObject Player;
    GameObject CameraTarget;

    private void Awake()
    {
        GateList = new List<GameObject>();
        BallList = new List<GameObject>();
    }

    public void DestroyAllGameObject()
    {
        foreach (var gate in GateList)
        {
            Destroy(gate);
        }
        foreach (var ball in BallList)
        {
            Destroy(ball);
        }
        Destroy(Player);
        Destroy(CameraTarget);
        GateList.Clear();
        BallList.Clear();
    }

    public void AddBall(GameObject ball)
    {
        BallList.Add(ball);
    }

    public void AddGate(GameObject gate)
    {
        GateList.Add(gate);
    }

    public void AddPlayer(GameObject player)
    {
        Player = player;
    }
    public void AddCamera(GameObject camera)
    {
        CameraTarget = camera;
    }
}
