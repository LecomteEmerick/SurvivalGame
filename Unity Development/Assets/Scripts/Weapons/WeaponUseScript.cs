using UnityEngine;
using System.Collections;

public class WeaponUseScript : MonoBehaviour {

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float distanceWeapons=50;

    [SerializeField]
    private GameObject impactBullet;

    private RaycastHit rayCast;

	// Use this for initialization
	void Start () {
        rayCast = new RaycastHit();
	}
	
	// Update is called once per frame
	void Update () {
        checkFire();
	}

    private void checkFire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            fire();
        Vector3 beginPos = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;
        //Debug.DrawRay(beginPos, direction,Color.white,999,true);
    }

    private void fire()
    {
        Vector3 beginPos = Camera.main.transform.position + Camera.main.transform.forward ;
        Vector3 direction = Camera.main.transform.forward;
        Quaternion rotation = Camera.main.transform.rotation;
        rotation *= Quaternion.Euler(90, 0, 0);
        if (Physics.Raycast(beginPos, direction, out rayCast, Mathf.Infinity))
        {
            PlaceImpactScript impact = rayCast.collider.gameObject.GetComponent<PlaceImpactScript>();
            if (impact != null)
                impact.addImpact(rayCast);
        }
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, beginPos, rotation);
        bullet.GetComponent<BulletShotScript>().Init(direction);
    }
}
