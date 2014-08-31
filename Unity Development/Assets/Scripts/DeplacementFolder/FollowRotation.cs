using UnityEngine;
using System.Collections;

public class FollowRotation : MonoBehaviour {

    [SerializeField]
    float declenchement = 0.1f;

    [SerializeField]
    float sensitivity = 400.0f;

    private Vector3 center;

	// Use this for initialization
	void Start () {
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
        var delta = (Input.mousePosition - center) / Screen.height;
        if (delta.x > declenchement)
            transform.Rotate(0, (delta.x - declenchement) * Time.deltaTime * sensitivity,0);
        if (delta.x < -declenchement)
            transform.Rotate(0, (delta.x + declenchement) * Time.deltaTime * sensitivity,0);

    }
}
