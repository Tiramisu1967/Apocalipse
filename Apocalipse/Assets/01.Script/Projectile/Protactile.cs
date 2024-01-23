using UnityEngine;

public class Protactile : MonoBehaviour
{
    public GameObject Player;       //기준
    public float speed;             //회전 속도

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
