using UnityEngine;

public class Protactile : MonoBehaviour
{
    public int numberOfObjects = 3;
    public float circleRadius = 1f;
    public float objectSpeed = 30f;
    public float MoveSpeed = 2f;

    private float degrees;
    private Vector3 direction;

    [SerializeField]
    private float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        degrees += Time.deltaTime * objectSpeed;

        if (degrees >= 360)
        {
            degrees = 0;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            float rad = Mathf.Deg2Rad * (degrees + (i * (360f / numberOfObjects)));
            float x = circleRadius * Mathf.Sin(rad);
            float y = circleRadius * Mathf.Cos(rad);
            Vector3 newPosition = new Vector3(x, y, 0);

            transform.position = transform.position + newPosition;
            transform.rotation = Quaternion.Euler(0, 0, degrees + (i * (360f / numberOfObjects)));

            // 또는 지속적인 회전을 위해 transform.Rotate 사용 가능
            // transform.Rotate(Vector3.forward * Time.deltaTime * objectSpeed);
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void OnDestroy()
    {
        // Instantiate(ExplodeFX, transform.position, Quaternion.identity);
    }
}
