using UnityEngine;
using System.Collections;

public class Baguette : Pickup {


    private BaguetteMode baguetteMode;
    private Vector3 throwDirection = Vector3.zero;
	private AudioSource smallThud;
	private AudioSource bigThud;

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
		smallThud = gameObject.GetComponents<AudioSource> ()[0];
		bigThud = gameObject.GetComponents<AudioSource> ()[1];

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
        if (player && thrower != null)
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
				bigThud.Play();
                baguetteSugoiPush(pawn);
                break;
            }
			else if(thisColTag.Equals("Weapon")  &&  otherColTag.Equals("Weapon"))
			{
				smallThud.Play();
			}
        }
    }

    public override void throwPickup()
    {
		throwBaguetteCoroutine ();

    }

    private void throwBaguetteCoroutine()
    {
        thrower = carrier;
        isAvaiableForPickup = false;
        baguetteMode = BaguetteMode.EmpoweredMode;
        throwDirection = carrier.transform.right;
		dropped();
        Rigidbody rigBody = gameObject.GetComponent<Rigidbody>();
        if (!GetComponent<Rigidbody>())
            rigBody = gameObject.AddComponent<Rigidbody>();

        rigBody.AddForce(throwDirection * 100, ForceMode.Impulse);

		StartCoroutine("checkForPickupAvailable");
    }

	void canNowBePickedUp()
	{
		isAvaiableForPickup = true;
		thrower = null;
		baguetteMode = BaguetteMode.NormalMode;
	}

	IEnumerator checkForPickupAvailable()
	{
		yield return new WaitForSeconds(0.2f);
		float timeElapsed = 0;
		float lasTime = Time.time;
		while(true)
		{
			Debug.Log (timeElapsed);
			timeElapsed += Time.time - lasTime;
			Rigidbody rBody = GetComponent<Rigidbody> ();
			if (rBody && rBody.velocity.magnitude < 1) {
				canNowBePickedUp();
				break;
			}
			else if(timeElapsed > SECONDS_BEFORE_BAGUETTE_IS_PICKABLE_AFTER_THROW)
			{
				canNowBePickedUp();
				break;
			}
			lasTime = Time.time;
			yield return new WaitForSeconds(0.2f);
		}
		return true;

	}


    void baguetteSugoiPush(Pawn pawn)
    {
        Vector3 forceDir = carrier != null ? carrier.transform.right : throwDirection;
        float pushStr = 75.0f;

        if (baguetteMode == BaguetteMode.EmpoweredMode)
            pushStr = 1000f;

        if (carrier == null && throwDirection != Vector3.zero)
            pushStr = 100.0f;


        Debug.Log(pushStr + " " + forceDir);

        pawn.GetComponent<PlayerController>().addPushForce(forceDir, pushStr);
        throwDirection = Vector3.zero;
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
