using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject GatePrefab;
    public GameObject BallPrefab;
    public GameObject PlayerPrefab;

    public Transform PlayerStartTransform;
    public Transform Balls;
    public Transform Gates;

    int m_CurLevelID = 0;
    Level m_CurLevel;
    int CurGatePassed = 0;

    private void Awake()
    {
        ScoreArea.OnPassed += OnOneGatePassed;
    }

    private void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        GameObjectManager.Instance.DestroyAllGameObject();
        CurGatePassed = 0;

        m_CurLevel = new Level(m_CurLevelID);
        Random.InitState(m_CurLevel.Seed);

        for (int i = 0; i < m_CurLevel.GateNum; i++)
        {
            float xPos = Random.Range(-4f, 4f) * 10;
            float zPos = Random.Range(-4f, 4f) * 10;
            GameObject newGate = Instantiate(GatePrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);
            GameObjectManager.Instance.AddGate(newGate);
            newGate.transform.parent = Gates;
        }

        for (int i = 0; i < m_CurLevel.BallNum; i++)
        {
            float xPos = Random.Range(-4f, 4f) * 10;
            float zPos = Random.Range(-4f, 4f) * 10;
            GameObject newBall = Instantiate(BallPrefab, new Vector3(xPos, 1, zPos), Quaternion.identity);
            GameObjectManager.Instance.AddBall(newBall);
            newBall.transform.parent = Balls;
        }

        GameObject newPlayer = Instantiate(PlayerPrefab, PlayerStartTransform.position, PlayerStartTransform.rotation);
        GameObjectManager.Instance.AddPlayer(newPlayer);
    }

    void LevelWin()
    {
        m_CurLevelID++;
        GenerateLevel();
    }
    void LevelRestart()
    {
        GenerateLevel();
    }

    void OnOneGatePassed()
    {
        CurGatePassed++;
        if (CurGatePassed == m_CurLevel.GateNum)
            LevelWin();
    }

    private void OnDestroy()
    {
        ScoreArea.OnPassed -= OnOneGatePassed;
    }
}

public class Level
{
    public int ID;
    public int GateNum;
    public int BallNum;
    public int Seed;

    public Level(int id)
    {
        ID = id;
        Seed = id;
        GateNum = id + 1;
        BallNum = (10 - id) < 2 ? 1 : (10 - id);
    }
}
