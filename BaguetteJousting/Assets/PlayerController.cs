using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    enum PlayerNum
    {
        PLAYER_ONE,
        PLAYER_TWO,
        PLAYER_THREE,
        PLAYER_FOUR,
    };

    const float DIR_MULT = 0.3f;
    const float ACCEL_MOD = 0.01f;
    const float ACCEL_ROTATE_MOD = 0.05f;
    const float MAX_ACCEL_ROTATE = 0.5f;
    const float MAX_ACCEL = 0.4f;
    const float ROTATE_SPEED = 170.0f;
    const float STILL_ROTATE_SPEED = 110.5f;
    const float MAX_SPEED_ROTATE_MOD = ROTATE_SPEED - STILL_ROTATE_SPEED;
    const float MAX_SPEED = 30.0f;
    const float MAX_STILL_SPEED = 5.0f;
    const float MAX_BACK_SPEED = -10.0f;
    const float ACCEL_DECREASE = 1000.0f;
    const float MAX_SCALE = 3.0f;
    const float MIN_SCALE = 0.5f;
    [SerializeField]
    PlayerNum _playerNum;

    [SerializeField]
    Transform _representation;

    float _baseY = 0;
    bool _isStill = true;
    float _acceleration = 0.0f;
    float _velocity = 0.0f;
    Transform _front;
    Transform _back;
	// Use this for initialization
	void Start () {
        _front = GetComponentInChildren<FrontRef>().transform;
        _back = GetComponentInChildren<BackRef>().transform;
        _baseY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

        if (_front == null || _back == null)
            return;


        float x = getRotation();

        if (_isStill)
        {
            transform.RotateAround(_back.position, new Vector3(0.0f, x, 0.0f), ROTATE_SPEED * Time.deltaTime);
            if(x != 0) 
                _acceleration = ACCEL_ROTATE_MOD;

            
        }
        else
        {
            float rotateMult = (ROTATE_SPEED - STILL_ROTATE_SPEED)  * (1- Mathf.Abs(_velocity) / MAX_SPEED) + STILL_ROTATE_SPEED;
            transform.Rotate(new Vector3(0, x * rotateMult*Time.deltaTime, 0));

        }


        Vector3 dirVector = _front.position - _back.position;
        Debug.DrawLine(transform.position, transform.position + dirVector);

        float input = getMovement();
        bool noInput = Mathf.Abs(input) < 0.15f;

        if (!noInput)
            _isStill = false;

        Debug.Log("Input " + input + " " + _acceleration);

        if (input != 0 && Mathf.Sign(_acceleration) != Mathf.Sign(-input))
        {
            _acceleration = MAX_ACCEL * Mathf.Sign(-input);
            _velocity = _velocity *0.6f;
        }

        _acceleration = _acceleration + ACCEL_MOD * -input;
        _acceleration = Mathf.Clamp(_acceleration, -MAX_ACCEL, MAX_ACCEL);

        dirVector *=_velocity* Time.deltaTime;


        if (noInput)
        {
            _acceleration -= _velocity / ACCEL_DECREASE;
        }
        float velSign = Mathf.Sign(_velocity);

        _velocity += _acceleration;
        if(_velocity > (_isStill ? MAX_STILL_SPEED : MAX_SPEED))
            _velocity = (_isStill ? MAX_STILL_SPEED : MAX_SPEED);
        if (_velocity < MAX_BACK_SPEED)
            _velocity = MAX_BACK_SPEED;

        if (velSign != Mathf.Sign(_velocity) && noInput)
        {
            _isStill = true;
            _velocity = 0.0f;
            _acceleration = 0.0f;
        }

        float scaleVal = transform.position.y / _baseY;
        scaleVal = Mathf.Clamp(Mathf.Pow(scaleVal, 1),MIN_SCALE,MAX_SCALE);
        _representation.localScale = new Vector3(scaleVal,scaleVal,scaleVal);

        rigidbody.MovePosition(transform.position + dirVector);
        
	
	}

    float getMovement()
    {
        float val = 0;
        switch (_playerNum)
        {
            case PlayerNum.PLAYER_ONE:
                val= Input.GetAxis("Player1_axisMove");
                break;
            case PlayerNum.PLAYER_TWO:
                val= Input.GetAxis("Player2_axisMove");
                break;
            case PlayerNum.PLAYER_THREE:
                val= Input.GetAxis("Player3_axisMove");
                break;
            case PlayerNum.PLAYER_FOUR:
                val= Input.GetAxis("Player4_axisMove");
                break;
        }
        if (Mathf.Abs(val) < 0.8f)
            val = 0.0f;
        return val;
    }
    
    float getRotation()
    {
        float val = 0;
        switch(_playerNum)
        {
            case PlayerNum.PLAYER_ONE:
                val= Input.GetAxis("Player1_axisX");
                break;
            case PlayerNum.PLAYER_TWO:
                val = Input.GetAxis("Player2_axisX");
                break;
            case PlayerNum.PLAYER_THREE:
                val = Input.GetAxis("Player3_axisX");
                break;
            case PlayerNum.PLAYER_FOUR:
                val = Input.GetAxis("Player4_axisX");
                break;
        }
        if (Mathf.Abs(val) < 0.8f)
            val = 0.0f;
        return val;
    }
        
}
