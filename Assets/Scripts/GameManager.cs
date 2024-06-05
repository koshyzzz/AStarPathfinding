using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    GameObject characterPrefab;

    [SerializeField]
    GameObject freeCam, mainCam;

    public enum State
    {
        None,
        Idle,
        Running,
        EditMode
    }

    public State state;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(State.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(State s)
    {
        if (state == s)
            return;

        switch (s)
        {
            case State.Idle:
                if(Character.instance == null)
                    Instantiate(characterPrefab, TileManager.instance.startPoint.transform.position, Quaternion.identity);
                break;

            case State.Running:
                    GetAStarPath();
                break;

            case State.EditMode:
                if(Character.instance != null)
                    Destroy(Character.instance.gameObject);
                break;
        }

        state = s;
    }

    public void GetAStarPath()
    {
        if (TileManager.instance.startPoint == TileManager.instance.endPoint)
            return;
        List<Tile> path = AStar.FindPath(TileManager.instance.startPoint, TileManager.instance.endPoint);
        if (path == null)
        {
            UIManager.instance.ShowError("Cannot find path, check obstacles placement");
            return;
        }
        Character.instance.StartRunning(path);

        TileManager.instance.ClearPreviousPath();
        TileManager.instance.DrawPath(path);
    }

    public void RecalculatePath(Tile newEndTile)
    {
        if (!TileManager.instance.SetEndPoint(newEndTile))
            return;
        Character.instance.StopRunning();
        Tile start = Character.instance.GetCurrentTile();
        if(start != null)
            TileManager.instance.SetStartPoint(start);
        state = State.Running;
        GetAStarPath();
    }

    public void StartFreeCam()
    {
        freeCam.SetActive(true);
        mainCam.SetActive(false);
        freeCam.transform.position = Vector3.zero;
    }

    public void StopFreeCam()
    {
        freeCam.SetActive(false);
        mainCam.SetActive(true);
    }
}