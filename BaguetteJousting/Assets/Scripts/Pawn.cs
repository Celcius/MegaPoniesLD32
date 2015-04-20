using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour {
	string pawnName = "Joe";
    public Baguette baguette;
	bool alive = true;
	GameObject oceanOfDeath;

	bool drowningCountdownStarted = false;
	
	float killZ = -20.0f;

    void Start()
    {
		oceanOfDeath = GameObject.Find ("OceanOfDeath");
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
        transform.gameObject.SetActive(false);
        Destroy(gameObject,1.0f);
        
	}
	
	
	
	void Update () {

		float pawnY = gameObject.transform.position.y;
		if(pawnY < killZ){
			this.Kill();
		}

		if (pawnY < oceanOfDeath.transform.position.y && !drowningCountdownStarted)
			StartCoroutine ("drowningCountDown");
	}

	IEnumerator drowningCountDown()
	{
		drowningCountdownStarted = true;
		yield return new WaitForSeconds (2);
		float pawnY = gameObject.transform.position.y;
		if (pawnY < oceanOfDeath.transform.position.y)
			gameObject.GetComponent<Rigidbody> ().detectCollisions = false;
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
