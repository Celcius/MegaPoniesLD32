using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour {
	string pawnName = "Joe";
    public Baguette baguette;
	bool alive = true;
	
	float killZ = -20.0f;

    void Start()
    {
        Carrier carrier = GetComponent<Carrier>();
        if (carrier)
            carrier.setOnPickUpCallBack(onItemPickup);
    }

	public bool isAlive(){
		return alive;
	}
	
	public void Kill(){


        Pickup pick = GetComponent<Carrier>().pickup;
        if(pick != null)
            pick.GetComponent<Baguette>().destroyBaguette();
		this.alive = false;
		Arena.instance.PlayerDied(this);
        transform.active = false;
        Destroy(gameObject,1.0f);
        
	}
	
	
	
	void Update () {
		if(gameObject.transform.position.y < killZ){
			this.Kill();
		}
	}

    void onItemPickup(Pickup item)
    {
        if (item.PickUpType == PickupTypes.PickupBaguette)
            baguette = (Baguette)item;
    }


    void OnCollisionEnter(Collision collision)
    {
        Pawn pawn = collision.gameObject.GetComponent<Pawn>();
        if (pawn && baguette)
            baguette.resolveCollisionWithPawn(collision, pawn);
    }

    public void useAction()
    {
        GetComponent<Carrier>().throwPickup();
    }

}
