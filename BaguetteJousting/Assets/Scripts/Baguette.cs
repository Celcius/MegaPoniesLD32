using UnityEngine;
using System.Collections;

public class Baguette : Pickup {

    public GameObject lateralColliderObject;
    public GameObject frontCollisionObject;

	// Use this for initialization
	
    void Start()
    {

        pickUpType = PickupTypes.PickupBaguette;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Baguette baguette = collision.gameObject.GetComponent<Baguette>();
        Pawn player = collision.gameObject.GetComponent<Pawn>();
        if (baguette)
            checkForBaguetteDisarm(collision, baguette);
        else if (player)
            checkForKill(player);
    }

    public void checkForBaguetteDisarm(Collision collision, Baguette baguette)
    {
        ContactPoint[] contactPoints = collision.contacts;

        foreach(ContactPoint c in contactPoints)
        {
            if (c.thisCollider.gameObject == lateralColliderObject)
            {
                disarmOpponentBaguette(baguette);
                break;
            }
        }
    }

    public void resolveCollisionWithPawn(Collision collision, Pawn pawn)
    {

        foreach (ContactPoint c in collision.contacts)
        {
            string thisColTag = c.thisCollider.tag;
            string otherColTag = c.otherCollider.tag;
            if ((thisColTag.Equals("FrontOfWeapon") || thisColTag.Equals("SideOfWeapon")) &&
                otherColTag.Equals("Player"))
            {
                checkForKill(pawn);
                break;
            }
        }
    }


    private void disarmOpponentBaguette(Baguette baguette)
    {
        baguette.gameObject.AddComponent<Rigidbody>();
        baguette.transform.SetParent(null);
        baguette.rigidbody.AddForce(new Vector3(0, 10f, 0), ForceMode.Impulse);
        baguette.GetComponent<Pickup>().dropped();
    }


	
    public void frontCollisionCallback(GameObject child, Collision collision)
    {
        Baguette baguette = collision.gameObject.GetComponent<Baguette>();
        Pawn player = collision.gameObject.GetComponent<Pawn>();
        if (baguette)
        {
            checkForBaguetteDisarm(collision, baguette);
        }
        else if (player)
            checkForKill(player);

    }

    public void checkForKill(Pawn pawn)
    {

        Arena.instance.PlayerDied(pawn);
        GameObject.Destroy(pawn.gameObject);
    }

}
