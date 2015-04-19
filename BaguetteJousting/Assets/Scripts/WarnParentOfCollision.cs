using UnityEngine;
using System.Collections;

public class WarnParentOfCollision : MonoBehaviour {

    public delegate void OnCollisionEnterDelegate(GameObject child, Collision collision);
    public delegate void OnTriggerEnterDelegate(GameObject child, Collider collider);

    public OnCollisionEnterDelegate _onCollisionEnterDelegate;
    public OnTriggerEnterDelegate _onTriggerEnterDelegate;

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Fodase");
        if (_onTriggerEnterDelegate != null)
            _onTriggerEnterDelegate(gameObject, collider);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Fodasesss");
        if (_onCollisionEnterDelegate != null)
            _onCollisionEnterDelegate(gameObject, collision);

    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Sai");
        if (_onCollisionEnterDelegate != null)
            _onCollisionEnterDelegate(gameObject, collision);

    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Fodasesss");
        if (_onCollisionEnterDelegate != null)
            _onCollisionEnterDelegate(gameObject, collision);

    }
}
