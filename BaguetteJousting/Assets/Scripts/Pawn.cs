using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour {
	string pawnName = "Joe";
    Baguette baguette;
	bool alive = true;
	
	float killZ = -50.0f;

    void Start()
    {
        Carrier carrier = GetComponent<Carrier>();
        if (carrier)
            carrier.setOnPickUpCallBack(onItemPickup);
    }

	public bool isAlive(){
		return alive;
	}
	
	void Kill(){
		Debug.Log(this + " died");
		this.alive = false;
		Arena.instance.PlayerDied(this);
		
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
        Debug.Log(collision.collider.name);
        Debug.Log(collision.gameObject.name);
        if (pawn && baguette)
            baguette.resolveCollisionWithPawn(collision, pawn);


    }

}
