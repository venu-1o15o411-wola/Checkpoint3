using UnityEngine;

public class TornadoRotation : MonoBehaviour
{
    [SerializeField] private float speed = 100f;

    void Update()
    {
        // Rota el objeto en el eje Y cada frame
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
