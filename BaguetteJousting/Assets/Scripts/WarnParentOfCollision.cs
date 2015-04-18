using UnityEngine;
using System.Collections;

public class WarnParentOfCollision : MonoBehaviour {

    public delegate void OnCollisionEnterDelegate(GameObject child, Collision collision);
    public delegate void OnTriggerEnterDelegate(GameObject child, Collider collider);

    public OnCollisionEnterDelegate _onCollisionEnterDelegate;
    public OnTriggerEnterDelegate _onTriggerEnterDelegate;

    void OnTriggerEnter(Collider collider)
    {
        if (_onTriggerEnterDelegate != null)
            _onTriggerEnterDelegate(gameObject, collider);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_onCollisionEnterDelegate != null)
            _onCollisionEnterDelegate(gameObject, collision);

    }
}
