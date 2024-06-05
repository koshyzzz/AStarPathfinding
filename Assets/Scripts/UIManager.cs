using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private GameObject gameUI, editUI, generalUI, freeCamUI;

    [SerializeField]
    private InputField xSize, ySize;

    [SerializeField]
    private Text editModeButtonText;

    [SerializeField]
    private CanvasGroup errorHolder;

    [SerializeField]
    private Text errorText;

    private Coroutine errorCoroutine;

    private bool isFreeCamEnabled;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        errorHolder.alpha = 0f;
        isFreeCamEnabled = false;
        editUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isFreeCamEnabled && Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchFreeCam(false);
        }
    }

    public void ShowError(string message)
    {
        if(errorCoroutine != null)
        {
            StopCoroutine(errorCoroutine);
        }
        errorText.text = message;
        errorCoroutine = StartCoroutine(HideError());
    }

    IEnumerator HideError()
    {
        yield return null;
        errorHolder.alpha = 1.0f;
        yield return new WaitForSeconds(3);
        float alpha = 1.0f;
        while(alpha > 0f)
        {
            alpha -= Time.deltaTime;
            errorHolder.alpha = alpha;
            yield return null;
        }
        errorHolder.alpha = 0f;   
    }

    public void ChangeMode()
    {
        if(GameManager.instance.state != GameManager.State.EditMode)
        {
            GameManager.instance.SwitchState(GameManager.State.EditMode);
            editUI.SetActive(true);
            gameUI.SetActive(false);
            editModeButtonText.text = "Play Mode";            
        }
        else
        {
            GameManager.instance.SwitchState(GameManager.State.Idle);
            CursorSetEnd();
            editUI.SetActive(false);
            gameUI.SetActive(true);
            editModeButtonText.text = "Edit Mode";
        }
    }

    public void CursorSwap()
    {
        ClickHandler.instance.cursorType = ClickHandler.CursorType.Swap;
    }

    public void CursorSetStart()
    {
        ClickHandler.instance.cursorType = ClickHandler.CursorType.SetStart;
    }

    public void CursorSetEnd()
    {
        ClickHandler.instance.cursorType = ClickHandler.CursorType.SetEnd;
    }

    public void ChangeGameState()
    {
        GameManager.instance.SwitchState(GameManager.State.Running);
    }

    public void SwitchFreeCam(bool start)
    {
        if (start)
        {
            GameManager.instance.StartFreeCam();
            isFreeCamEnabled = true;
            editUI.SetActive(false);
            gameUI.SetActive(false);
            generalUI.SetActive(false);
            freeCamUI.SetActive(true);
        }
        else
        {
            GameManager.instance.StopFreeCam();
            isFreeCamEnabled = false;
            editUI.SetActive(GameManager.instance.state == GameManager.State.EditMode);
            gameUI.SetActive(GameManager.instance.state != GameManager.State.EditMode);
            generalUI.SetActive(true);
            freeCamUI.SetActive(false);
        }
    }

    public void GenerateGrid()
    {
        int x, y;
        if(!int.TryParse(xSize.text, out x) || !int.TryParse(ySize.text, out y))
        {
            ShowError("X and Y must be numbers");
            return;
        }
        if(x < 2 || y < 2)
        {
            ShowError("Grid must be at least 2x2");
            return;
        }
        TileManager.instance.GenerateGrid(x, y);
        ResizeCam.instance.ResizeCamera(x, y);
    }
}
