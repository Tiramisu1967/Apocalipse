using UnityEngine;

public class Protactile : MonoBehaviour
{
    public GameObject Player;       //����
    public float speed;             //ȸ�� �ӵ�

    private void Update()
    {
        OrbitAround();
    }

    void OrbitAround()
    {
        this.transform.RotateAround(Player.transform.position, Vector3.back, speed * Time.deltaTime);
    }
    // RotateAround(Vector3 point, Vector3 axis, float angle)
}
