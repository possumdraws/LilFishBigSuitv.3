using UnityEngine;

public class TEST_Float_In_Water2 : MonoBehaviour
{
    public float speed = 0.3f;
    public float height = 0.2f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}