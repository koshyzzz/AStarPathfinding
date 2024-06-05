using UnityEngine;

public class ResizeCam : MonoBehaviour
{
    public static ResizeCam instance;

    private void Awake()
    {
        instance = this;
    }

    public void ResizeCamera(int x, int y)
    {
        float zx = -0.7f * x - 4;
        float zy = -1.3f * y - 2;

        float z = Mathf.Min(zx, zy);
        z = Mathf.Min(z, -7);
        transform.localPosition = new Vector3(0, 0, z);
    }
}
