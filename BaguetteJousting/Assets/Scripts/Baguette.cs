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
        lateralScript._onTriggerEnterDelegate = new WarnParentOfCollision.OnTriggerEnterDelegate(frontTriggerCallback);


        frontScript._onCollisionEnterDelegate = new WarnParentOfCollision.OnCollisionEnterDelegate(lateralCollisionCallback);
        frontScript._onTriggerEnterDelegate = new WarnParentOfCollision.OnTriggerEnterDelegate(lateralTriggerCallback);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void frontCollisionCallback(GameObject child, Collision collision)
    {
        Baguette baguette = collider.gameObject.GetComponent<Baguette>();
        if (baguette)
        {
            DisarmOpponentBaguette(baguette);
        }
    }

    public void frontTriggerCallback(GameObject child, Collider collider)
    {
        Baguette baguette = collider.gameObject.GetComponent<Baguette>();
        if (baguette)
        {
            DisarmOpponentBaguette(baguette);
        }
    }


    public void lateralTriggerCallback(GameObject child, Collider collider)
    {
        rigidbody.AddForce(new Vector3(0, 50f, 0), ForceMode.Impulse);
    }

    public void lateralCollisionCallback(GameObject child, Collision collision)
    {
        rigidbody.AddForce(new Vector3(0, 50f, 0), ForceMode.Impulse);
    }





    private void DisarmOpponentBaguette(Baguette baguette)
    {
        baguette.transform.SetParent(null);
        baguette.rigidbody.isKinematic = false;
        baguette.rigidbody.AddForce(new Vector3(0, 50f, 0), ForceMode.Impulse);  
    }

}
