using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    const float DIR_MULT = 0.3f;
    const float ACCEL_MOD = 0.1f;
    const float MAX_ACCEL = 2.0f;
    const float ROTATE_SPEED = 7.0f;
    const float MAX_SPEED_ROTATE_MOD = 2.0f;
    const float MAX_SPEED = 9.0f;
    const float MAX_BACK_SPEED = -4.0f;
    const float ACCEL_DECREASE = 18.0f;


    float _acceleration = 0.0f;
    float _velocity = 0.0f;
    Transform _front;
    Transform _back;
	// Use this for initialization
	void Start () {
        _front = GetComponentInChildren<FrontRef>().transform;
        _back = GetComponentInChildren<BackRef>().transform;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (_front == null || _back == null)
            return;


        
        float x = Input.GetAxis("Horizontal");

        float rotateMult =  ROTATE_SPEED - MAX_SPEED_ROTATE_MOD*(Mathf.Abs(_velocity)/MAX_SPEED);
        transform.Rotate(new Vector3(0, x * rotateMult, 0));


        Vector3 dirVector = _front.position - _back.position;
        Debug.DrawLine(transform.position, transform.position + dirVector);

        _acceleration = _acceleration + ACCEL_MOD * -Input.GetAxis("Move");
        _acceleration = Mathf.Clamp(_acceleration, -MAX_ACCEL, MAX_ACCEL);

        _velocity += _acceleration;
        if(_velocity > MAX_SPEED)
            _velocity = MAX_SPEED;
        if (_velocity < MAX_BACK_SPEED)
            _velocity = MAX_BACK_SPEED;


        dirVector *=_velocity* Time.deltaTime;

        rigidbody.MovePosition(transform.position + dirVector);

	
	}
}
