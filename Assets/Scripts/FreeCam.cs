using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public Transform pivot;
    public Camera cam;

    static readonly float MAX_TILT = 90f;
    static readonly float MIN_TILT = 20f;
    static readonly float SPEED = 5f;
    static readonly float ROT_SPEED = 1f;

    float tilt, pan;

    // Start is called before the first frame update
    void Start()
    {
        tilt = 45f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * SPEED * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        tilt -= Input.GetAxis("Mouse Y") * ROT_SPEED;
        pan += Input.GetAxis("Mouse X") * ROT_SPEED;
        tilt = Mathf.Clamp(tilt, MIN_TILT, MAX_TILT);
        transform.rotation = Quaternion.Euler(0, pan, 0);
        pivot.localRotation = Quaternion.Euler(tilt, 0, 0);
    }
}
