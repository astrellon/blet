using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float Speed = 10.0f;
    public float CoolDownTime = 0.0f;
    public GameObject BulletPrefab;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        var moveX = 0.0f;
        var moveY = 0.0f;
        if (Input.GetKey(KeyCode.A))
        {
            moveX -= Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX += Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveY += Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY -= Speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space) && Time.time > CoolDownTime)
        {
            SpawnBullet(transform.position, 0.0f);
            SpawnBullet(transform.position + new Vector3(-0.7f, 0.0f, -0.7f), 0.0f);
            SpawnBullet(transform.position + new Vector3(0.7f, 0.0f, -0.7f), 0.0f);
            CoolDownTime = Time.time + 0.1f;
        }

        transform.Translate(new Vector3(moveX, 0, moveY));
	}

    void SpawnBullet(Vector3 position, float angleOffset)
    {
        var bulletObject = Instantiate(BulletPrefab, position, transform.rotation) as GameObject;
        bulletObject.transform.rotation *= Quaternion.Euler(0, angleOffset, 0);

        var bulletScript = bulletObject.GetComponent<Bullet>();
        bulletScript.LifeTime = Time.time + 1.0f;
    }
}
