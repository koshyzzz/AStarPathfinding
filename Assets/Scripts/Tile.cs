using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum Type
    {
        Traversable,
        Obstacle
    }

    public Type type;

    public int x, y;

    public GameObject flag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(ClickHandler.CursorType cursor)
    {
        if(GameManager.instance.state != GameManager.State.EditMode)
        {
            GameManager.instance.RecalculatePath(this);
            return;
        }

        if (cursor == ClickHandler.CursorType.SetEnd)
        {
            TileManager.instance.SetEndPoint(this);
        }

        if (cursor == ClickHandler.CursorType.SetStart)
        {
            TileManager.instance.SetStartPoint(this);
        }

        if (cursor == ClickHandler.CursorType.Swap)
        {
            TileManager.instance.SwapTile(this);
        }
    }

    public void ChangeColor(Color color)
    {
        if (type == Type.Obstacle)
            return;
        GetComponent<MeshRenderer>().material.SetColor("_Color", color);
    }
}
