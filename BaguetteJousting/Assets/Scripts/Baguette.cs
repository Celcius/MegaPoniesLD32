using UnityEngine;
using System.Collections;

public class Baguette : Pickup {


    private BaguetteMode baguetteMode;
    private Vector3 throwDirection = Vector3.zero;

    enum BaguetteMode
    {
        NormalMode,
        EmpoweredMode,
    }

    BaguetteSpawner _spawner;


    const float KILL_Z = -20.0f;

	// Use this for initialization
	
    void Start()
    {

        pickUpType = PickupTypes.PickupBaguette;
        baguetteMode = BaguetteMode.NormalMode;
    }


    void Update()
    {
        if (gameObject.transform.position.y < KILL_Z && _spawner != null)
        {


            destroyBaguette();
        }
    }

    public void destroyBaguette()
    {
        Pickup pickup = (Pickup)GetComponent<Pickup>();


        if (pickup.carrier != null)
            pickup.carrier.destroyedPickUp();
        pickup.carrier = null;
        _spawner.destroyBaguette(true);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Pawn player = collision.gameObject.GetComponent<Pawn>();
        if (player)
            resolveCollisionWithPawn(collision, player);
    }

    public void resolveCollisionWithPawn(Collision collision, Pawn pawn)
    {
        if (thrower != null && pawn.gameObject == thrower.gameObject)
            return;

        if (carrier != null && pawn.gameObject == carrier.gameObject)
            return;

        foreach (ContactPoint c in collision.contacts)
        {
            string thisColTag = c.thisCollider.tag;
            string otherColTag = c.otherCollider.tag;
            if (thisColTag.Equals("Weapon")  &&  otherColTag.Equals("Player"))
            {
                baguetteSugoiPush(pawn);
                break;
            }
        }
    }

    public override void throwPickup()
    {
        StartCoroutine("throwBaguetteCoroutine");

    }

    private IEnumerator throwBaguetteCoroutine()
    {
        thrower = carrier;
        isAvaiableForPickup = false;
        baguetteMode = BaguetteMode.EmpoweredMode;
        dropped();
        throwDirection = carrier.transform.right;
        carrier = null;
        Rigidbody rigBody = gameObject.GetComponent<Rigidbody>();
        if (!GetComponent<Rigidbody>())
            rigBody = gameObject.AddComponent<Rigidbody>();

        rigBody.AddForce(throwDirection * 100, ForceMode.Impulse);

        yield return new WaitForSeconds(SECONDS_BEFORE_BAGUETTE_IS_PICKABLE_AFTER_THROW);

        isAvaiableForPickup = true;
        thrower = null;
        baguetteMode = BaguetteMode.NormalMode;
    }


    void baguetteSugoiPush(Pawn pawn)
    {
        Debug.Log("SUGOU PUSSSSHSHS");
        Vector3 forceDir = carrier != null ? carrier.transform.right : throwDirection;
        float pushStr = 75.0f;

        if (baguetteMode == BaguetteMode.EmpoweredMode)
            pushStr = 1000f;

        pawn.GetComponent<PlayerController>().addPushForce(forceDir, pushStr);
    }

    public void registerSpawner(BaguetteSpawner spawner)
    {
        _spawner = spawner;
    }

    public override void dropped()
    {
        baguetteMode = BaguetteMode.NormalMode;
        base.dropped();
    }


    void checkForKill(Pawn pawn)
    {
        Arena.instance.PlayerDied(pawn);
        GameObject.Destroy(pawn.gameObject);

        if (_spawner != null)
            _spawner.destroyBaguette(true);
    }

}
