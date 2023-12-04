using UnityEngine;

public class Move : MonoBehaviour
{
    public float radius = 5.0f;
    public float speed = 2.0f;
    private float angle = 0.0f;
    private Vector3 centerPosition;

    void Start()
    {
        centerPosition = transform.position;
    }

    void Update()
    {
        float x = centerPosition.x + Mathf.Cos(angle) * radius;
        float z = centerPosition.z + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, transform.position.y, z);
        angle += speed * Time.deltaTime;
        angle = Mathf.Repeat(angle, 2 * Mathf.PI);
    }
}
