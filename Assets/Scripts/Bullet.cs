using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float Speed = 40.0f;
    public float LifeTime;
    public GameObject DeathPrefab;
    private Vector3 MoveDirection;

	// Use this for initialization
	void Start ()
    {
        MoveDirection = transform.forward * Speed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        var newPosition = transform.position + MoveDirection * Time.deltaTime;
        RaycastHit hit;
        if (Physics.Linecast(transform.position, newPosition, out hit))
        {
            var enemyScript = hit.transform.GetComponent<Enemy>();
            enemyScript.DealDamage(1);

            var deathObj = Instantiate(DeathPrefab, hit.point, Quaternion.identity) as GameObject;
            deathObj.transform.forward = hit.normal;
            Destroy(gameObject);
            return;
        }

        transform.position = newPosition;
        if (Time.time > LifeTime)
        {
            Destroy(gameObject);
            return;
        }
	}
}
