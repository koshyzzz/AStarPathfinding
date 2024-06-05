using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public static ClickHandler instance;

    public enum CursorType
    {
        Swap,
        SetStart,
        SetEnd
    }

    public CursorType cursorType;

    private void Awake()
    {
        instance = this;
        cursorType = CursorType.SetEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    void Click()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.collider.GetComponent<Tile>() != null)
            {
                hit.collider.GetComponent<Tile>().OnClick(cursorType);
            }
        }
    }
}
