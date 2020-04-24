using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject GatePrefab;
    public GameObject BallPrefab;
    public GameObject PlayerPrefab;
    public GameObject CameraTargetPrefab;

    public Transform PlayerStartTransform;
    public Transform Balls;
    public Transform Gates;

    int m_CurLevelID = 0;
    Level m_CurLevel;
    int CurGatePassed = 0;
    bool m_CurLevelFailed = false;

    public System.Action OnLevelWin;

    public int CurLevelID { get => m_CurLevelID;}

    private void Awake()
    {
        ScoreArea.OnPassed += OnOneGatePassed;
        TimeManager.Instance.OnTimeOver += OnLevelFailed;
    }

    private void Start()
    {
        GenerateLevel();
    }

    public void LevelContinue()
    {
        m_CurLevelID++;
        GenerateLevel();
    }
    public void LevelRestart()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        GameObjectManager.Instance.DestroyAllGameObject();
        CurGatePassed = 0;

        m_CurLevel = new Level(m_CurLevelID);
        Random.InitState(m_CurLevel.Seed);

        int numColor = m_CurLevel.ListColors.Count;

        for (int i = 0; i < m_CurLevel.GateNum; i++)
        {
            float xPos = Random.Range(-4f, 4f) * 10;
            float zPos = Random.Range(-4f, 4f) * 10;
            float yRot = Random.Range(-90f, 90f);
            GameObject newGate = Instantiate(GatePrefab, new Vector3(xPos, 0, zPos), Quaternion.EulerAngles(Vector3.up * yRot));
            GameObjectManager.Instance.AddGate(newGate);
            newGate.transform.parent = Gates;

            Color newcolor = m_CurLevel.ListColors[i % numColor];
            newGate.GetComponentInChildren<ScoreArea>().InitializeColor(newcolor);
        }

        for (int i = 0; i < m_CurLevel.BallNum; i++)
        {
            float xPos = Random.Range(-4f, 4f) * 10;
            float zPos = Random.Range(-4f, 4f) * 10;
            GameObject newBall = Instantiate(BallPrefab, new Vector3(xPos, 2, zPos), Quaternion.identity);
            GameObjectManager.Instance.AddBall(newBall);
            newBall.transform.parent = Balls;

            Color newcolor = m_CurLevel.ListColors[i % numColor];
            newBall.GetComponent<Ball>().Color = newcolor;
            newBall.GetComponent<MeshRenderer>().material.color = newcolor;
        }

        GameObject newPlayer = Instantiate(PlayerPrefab, PlayerStartTransform.position, PlayerStartTransform.rotation);
        GameObject newCamera = Instantiate(CameraTargetPrefab, PlayerStartTransform.position, PlayerStartTransform.rotation);
        newCamera.GetComponent<CameraControll>().FollowerTrans = newPlayer.transform;
        GameObjectManager.Instance.AddPlayer(newPlayer);
        GameObjectManager.Instance.AddCamera(newCamera);

        m_CurLevelFailed = false;
    }
    void OnOneGatePassed()
    {
        CurGatePassed++;
        if (CurGatePassed == m_CurLevel.GateNum)
        {
            if(!m_CurLevelFailed)
                OnLevelWin();
        }
    }

    void OnLevelFailed()
    {
        m_CurLevelFailed = true;
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
    public List<Color> ListColors;

    public Level(int id)
    {
        ID = id;
        Seed = id;
        GateNum = id + 1;
        BallNum = id / 2 + 1;
        ListColors = new List<Color>();
        if (id < 4)
        {
            ListColors.Add(Color.yellow);
            if(id > 1)
                ListColors.Add(Color.blue);
        }
        else
        {
            ListColors.Add(Color.yellow);
            ListColors.Add(Color.blue);
            ListColors.Add(Color.red);
        }
    }
}
