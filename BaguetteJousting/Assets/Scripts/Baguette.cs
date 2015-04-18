using UnityEngine;
using System.Collections;

public class Baguette : MonoBehaviour {

    public GameObject lateralColliderObject;
    public GameObject frontCollisionObject;

	// Use this for initialization
	

    public void OnCollisionEnter(Collision collision)
    {
        Baguette baguette = collision.gameObject.GetComponent<Baguette>();
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (baguette)
            checkForBaguetteDisarm(collision, baguette);
        else if (player)
            killPlayer(player);


        


    }

    public void checkForBaguetteDisarm(Collision collision, Baguette baguette)
    {
        ContactPoint[] contactPoints = collision.contacts;

        foreach(ContactPoint c in contactPoints)
        {
            if (c.thisCollider.gameObject == lateralColliderObject)
            {
                DisarmOpponentBaguette(baguette);
                break;
            }
        }
    }


    private void killPlayer(PlayerController player)
    {
        Destroy(player.gameObject);
    }


    private void DisarmOpponentBaguette(Baguette baguette)
    {
        baguette.gameObject.AddComponent<Rigidbody>();
        baguette.transform.SetParent(null);
        baguette.rigidbody.AddForce(new Vector3(0, 10f, 0), ForceMode.Impulse);
        baguette.GetComponent<Pickup>().dropped();
    }

}
