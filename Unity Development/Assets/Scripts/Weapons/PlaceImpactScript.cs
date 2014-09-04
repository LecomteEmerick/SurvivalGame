using UnityEngine;
using System.Collections;

public class PlaceImpactScript : MonoBehaviour
{
    [SerializeField]
    private GameObject impactBullet;

    [SerializeField]
    private float timeDestroy;

    public void addImpact(RaycastHit rayCast)
    {
        GameObject impact = (GameObject)Network.Instantiate(impactBullet, rayCast.point, Quaternion.FromToRotation(Vector3.up,rayCast.normal),100);
        StartCoroutine("destroyImpact", impact);
    }

    public IEnumerator destroyImpact(GameObject impact)
    {
        yield return new WaitForSeconds(timeDestroy);
        Network.Destroy(impact);
    }
}
