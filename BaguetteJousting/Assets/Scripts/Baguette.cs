using UnityEngine;
using System.Collections;

public class Baguette : MonoBehaviour {

	// Use this for initialization


	void Start () {
        GameObject lateralColliderObject = GameObject.Find("Colliders/LateralCollisionCollider");
        GameObject frontCollision = GameObject.Find("Colliders/FrontalCollisionCollider");

        WarnParentOfCollision lateralScript = lateralColliderObject.GetComponent<WarnParentOfCollision>();
        WarnParentOfCollision frontScript = frontCollision.GetComponent<WarnParentOfCollision>();

        lateralScript._onCollisionEnterDelegate = new WarnParentOfCollision.OnCollisionEnterDelegate(frontCollisionCallback);
        frontScript._onTriggerEnterDelegate = new WarnParentOfCollision.OnTriggerEnterDelegate(lateralCollisionCallback);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void frontCollisionCallback(GameObject child, Collision collision)
    {
        Debug.Log("Foddase");
        collision.collider.rigidbody.AddForce(new Vector3(0, 50f, 0), ForceMode.Impulse);
    }

    public void lateralCollisionCallback(GameObject child, Collider collider)
    {
        rigidbody.AddForce(new Vector3(0, 50f, 0), ForceMode.Impulse);
    }



}
