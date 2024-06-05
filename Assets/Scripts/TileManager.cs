using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;

    [SerializeField]
    GameObject tilePrefab, obstaclePrefab;

    [SerializeField]
    GameObject startFlagPrefab, endFlagPrefab;

    public Tile startPoint, endPoint;

    public List<List<Tile>> tileList;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tileList = new();
        GenerateGrid(10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapTile(Tile tile)
    {
        if(tile == startPoint || tile == endPoint)
        {
            UIManager.instance.ShowError("Cannot set obstacle as start point or end point");
            return;
        }

        GameObject prefab = tile.type == Tile.Type.Traversable ? obstaclePrefab : tilePrefab;
        Tile t = Instantiate(prefab, tile.transform.position, Quaternion.identity).GetComponent<Tile>();

        tileList[tile.x][tile.y] = t;
        t.x = tile.x;
        t.y = tile.y;
        Destroy(tile.gameObject);
    }

    public bool SetStartPoint(Tile t)
    {
        if(t == endPoint)
        {
            UIManager.instance.ShowError("Start point and End point cannot be the same");
            return false;
        }
        if (t.type == Tile.Type.Obstacle)
        {
            UIManager.instance.ShowError("Cannot set obstacle as start point");
            return false;
        }
        if(startPoint != null)
        {
            Destroy(startPoint.flag);
            startPoint.flag = null;
        }
        startPoint = t;
        t.flag = Instantiate(startFlagPrefab, t.transform.position, Quaternion.identity);
        return true;
    }

    public bool SetEndPoint(Tile t)
    {
        if (t == startPoint)
        {
            UIManager.instance.ShowError("Start point and End point cannot be the same");
            return false;
        }
        if (t.type == Tile.Type.Obstacle)
        {
            UIManager.instance.ShowError("Cannot set obstacle as end point");
            return false;
        }
        if (endPoint != null)
        {
            Destroy(endPoint.flag);
            endPoint.flag = null;
        }
        endPoint = t;
        t.flag = Instantiate(endFlagPrefab, t.transform.position, Quaternion.identity);
        return true;
    }

    public void GenerateGrid(int x, int y)
    {
        if (startPoint != null)
            Destroy(startPoint.flag);
        if(endPoint != null)
            Destroy(endPoint.flag);

        for(int i = 0; i < tileList.Count; i++)
        {
            for(int j = 0; j < tileList[i].Count; j++)
            {
                if (tileList[i][j] != null)
                    Destroy(tileList[i][j].gameObject);
            }
        }

        tileList.Clear();
        for (int i = 0; i < x; i++)
        {
            tileList.Add(new());
            for (int j = 0; j < y; j++)
            {
                Tile t = Instantiate(tilePrefab, new Vector3(i - x/2f + 0.5f, 0, j - y/2f + 0.5f), Quaternion.identity).GetComponent<Tile>();
                t.x = i; 
                t.y = j;
                t.gameObject.name = i.ToString() + "  " + j.ToString();
                tileList[i].Add(t);
            }
        }

        SetStartPoint(tileList[0].ElementAt(Random.Range(0, tileList[0].Count)));
        SetEndPoint(tileList[x - 1].ElementAt(Random.Range(0, tileList[x - 1].Count)));
    }

    public void ClearPreviousPath()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            for (int j = 0; j < tileList[i].Count; j++)
            {
                tileList[i][j].ChangeColor(Color.white);
            }
        }
    }

    public void DrawPath(List<Tile> path)
    {
        foreach (Tile tile in path)
        {
            tile.ChangeColor(Color.magenta);
        }
    }
}
