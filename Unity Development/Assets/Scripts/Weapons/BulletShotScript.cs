using UnityEngine;
using System.Collections;

public class BulletShotScript : MonoBehaviour {

    [SerializeField]
    private float speed = 500.0f;

    [SerializeField]
    private float SecondsUntilDestroy = 5;

    private Vector3 direction;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }


	// Use this for initialization
    public void Init(Vector3 direction)
    {
        this.direction = direction;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        this.gameObject.rigidbody.MovePosition(this.gameObject.transform.position + this.direction *
                    speed * Time.deltaTime);
        if (Time.time - startTime >= SecondsUntilDestroy)
        {
            Destroy(this.gameObject);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
