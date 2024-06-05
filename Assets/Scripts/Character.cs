using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character instance;

    private static readonly float SPEED = 4f;
    private static readonly float ROT_SPEED = 10f;

    private Tile currentTile;

    private List<Tile> currentPath;

    [SerializeField]
    Animator anim;

    Coroutine activeCoroutine;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopRunning()
    {
        if (anim != null)
            anim.SetBool("Walking", false);
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);
    }

    public void StartRunning(List<Tile> path)
    {
        transform.position = path[0].transform.position;
        currentPath = new(path);
        if(activeCoroutine != null )
            StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        yield return null;
        if (anim != null)
            anim.SetBool("Walking", true);
        while (currentPath.Count > 0)
        {
            yield return Turn();
            yield return Walk();
            currentPath.RemoveAt(0);
        }

        GameManager.instance.SwitchState(GameManager.State.Idle);
        if (anim != null)
            anim.SetBool("Walking", false);
    }

    IEnumerator Walk()
    {
        yield return null;
        
        currentTile = currentPath[0];
        float distance = Vector3.Distance(transform.position, currentTile.transform.position);
        while (distance > 0)
        {
            float step = Time.deltaTime * SPEED;
            distance -= step;
            transform.Translate(Vector3.forward * step);
            yield return null;
        }
        transform.position = currentTile.transform.position;
    }

    IEnumerator Turn()
    {
        yield return null;
        Vector3 turnVector = currentPath[0].transform.position - transform.position;
        while(true)
        {
            Vector3 dir = Vector3.RotateTowards(transform.forward, turnVector, Time.deltaTime * ROT_SPEED, 0f);
            if (Vector3.Angle(dir, turnVector) < 5f)
                break;
            transform.rotation = Quaternion.LookRotation(dir);
            //Debug.Log(Vector3.Angle(dir, turnVector));
            yield return null;
        }
        transform.rotation = Quaternion.LookRotation(turnVector);
    }

    public Tile GetCurrentTile()
    {
        return currentTile;
    }
}
