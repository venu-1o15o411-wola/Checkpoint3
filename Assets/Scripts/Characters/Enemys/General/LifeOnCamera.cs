using UnityEngine;

public class LifeOnCamera : MonoBehaviour
{
    public Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (!cam) { cam = Camera.main; if (!cam) return; }

        Vector3 dir = cam.transform.position - transform.position;
        dir.y = 0f; // solo yaw
        if (dir.sqrMagnitude > 0.000001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}
