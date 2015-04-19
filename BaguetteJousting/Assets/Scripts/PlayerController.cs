using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    enum PlayerNum
    {
        PLAYER_ONE,
		PLAYER_BOT,
        PLAYER_TWO,
        PLAYER_THREE,
        PLAYER_FOUR
    };

    const float DIR_MULT = 0.3f;
    const float ACCEL_MOD = 0.01f;
    const float ACCEL_ROTATE_MOD = 0.05f;
    const float MAX_ACCEL_ROTATE = 0.5f;
    const float MAX_ACCEL = 0.4f;
    const float ROTATE_SPEED = 200.0f;
    const float STILL_ROTATE_SPEED = 110.5f;
    const float MAX_SPEED_ROTATE_MOD = ROTATE_SPEED - STILL_ROTATE_SPEED;
    const float MAX_SPEED = 12.0f;
    const float MAX_STILL_SPEED = 5.0f;
    const float MAX_BACK_SPEED = -10.0f;
    const float ACCEL_DECREASE = 1000.0f;
    const float MAX_SCALE = 3.0f;
    const float MIN_SCALE = 1.0f;
    const float SCALE_VAL = 1.2f;
    [SerializeField]
    PlayerNum _playerNum;

    [SerializeField]
    Transform _representation;

    [SerializeField]
    Transform _shadow;
    [SerializeField]
    List<Transform> _bikes;

    TrailRenderer _trail;

    float _baseY = 0;
    bool _isStill = true;
    float _acceleration = 0.0f;
    float _velocity = 0.0f;
    Transform _front;
    Transform _back;

	public float acceleration {			
		get {
			return _acceleration;
			}
	}

	public float velocity {			
		get {
			return _velocity;
		}
	}


	// Use this for initialization
	void Start () {
        _front = GetComponentInChildren<FrontRef>().transform;
        _back = GetComponentInChildren<BackRef>().transform;
        _baseY = transform.position.y;
        _trail = GetComponentInChildren<TrailRenderer>();
        
        switch(_playerNum)
        {
            case PlayerNum.PLAYER_ONE:
                _trail.renderer.material = Resources.Load("Materials/Red") as Material;
                _bikes[0].active = true;
                break;
            case PlayerNum.PLAYER_TWO:
                _trail.renderer.material = Resources.Load("Materials/Blue") as Material;
                _bikes[1].active = true;
                break;
            case PlayerNum.PLAYER_THREE:
                _trail.renderer.material = Resources.Load("Materials/Yellow") as Material;
                _bikes[2].active = true;
                break;
            case PlayerNum.PLAYER_FOUR:
                _trail.renderer.material = Resources.Load("Materials/Purple") as Material;
                _bikes[3].active = true;
                break;
			case PlayerNum.PLAYER_BOT:
				_trail.renderer.material = Resources.Load("Materials/Blue") as Material;
				_bikes[1].active = true;
			GetComponent<Bot>().isBot = true;
				break;
        }
	}

    public void setPlayerNum(int i)
    {
        if (i == 0)
            _playerNum = PlayerNum.PLAYER_ONE;
        if (i == 4) // TODO> change back here and in the enum
            _playerNum = PlayerNum.PLAYER_TWO;
        if (i == 2)
            _playerNum = PlayerNum.PLAYER_THREE;
        if (i == 3)
            _playerNum = PlayerNum.PLAYER_FOUR;
		if (i == 1)
			_playerNum = PlayerNum.PLAYER_BOT;

    }

	public Vector3 FacingDirection(){
		return (_front.position - _back.position).normalized;
		}

	// Update is called once per frame
	void Update () {

        if (_front == null || _back == null)
            return;


        float x = getRotation();

        if (_isStill)
        {
            rotateRigidBodyAroundPointBy(rigidbody, _back.position, new Vector3(0, 1, 0), x * ROTATE_SPEED * Time.deltaTime);
            //rigidBody.RotateAround(_back.position, new Vector3(0.0f, x, 0.0f), ROTATE_SPEED * Time.deltaTime);
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

        float scaleVal = Mathf.Clamp(transform.position.y / _baseY,MIN_SCALE,MAX_SCALE);
        
        scaleVal = (SCALE_VAL - 1) / 2 * scaleVal + 1 - (SCALE_VAL - 1) / 2;

        _representation.localScale = new Vector3(scaleVal,scaleVal,scaleVal);
        scaleVal = 1/scaleVal*0.5f;
        _shadow.localScale = new Vector3(scaleVal, scaleVal, scaleVal);

        if(transform.position.y - _baseY < 2.8f)
            rigidbody.MovePosition(transform.position + dirVector);
        
	
	}

    float getMovement()
    {
        float val = 0;
        switch (_playerNum)
        {
            case PlayerNum.PLAYER_ONE:
                if (Input.GetKey(KeyCode.W))
                    val = -1;
                else if (Input.GetKey(KeyCode.S))
                    val = 1;
                else
                 val= Input.GetAxis("Player1_axisMove");
                break;
            case PlayerNum.PLAYER_TWO:
                if (Input.GetKey(KeyCode.UpArrow))
                    val = -1;
                else if (Input.GetKey(KeyCode.DownArrow))
                    val = 1;
                else
                    val= Input.GetAxis("Player2_axisMove");
                break;
            case PlayerNum.PLAYER_THREE:
                val= Input.GetAxis("Player3_axisMove");
                break;
            case PlayerNum.PLAYER_FOUR:
                val= Input.GetAxis("Player4_axisMove");
                break;
			case PlayerNum.PLAYER_BOT:
				val = gameObject.GetComponent<Bot>().movementInput;
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
                if (Input.GetKey(KeyCode.D))
                    val = 1;
                else if (Input.GetKey(KeyCode.A))
                    val = -1;
                else
                    val= Input.GetAxis("Player1_axisX");
                break;
            case PlayerNum.PLAYER_TWO:
                if (Input.GetKey(KeyCode.RightArrow))
                    val = 1;
                else if (Input.GetKey(KeyCode.LeftArrow))
                    val = -1;
                else
                    val = Input.GetAxis("Player2_axisX");
                break;
            case PlayerNum.PLAYER_THREE:
                val = Input.GetAxis("Player3_axisX");
                break;
            case PlayerNum.PLAYER_FOUR:
                val = Input.GetAxis("Player4_axisX");
                break;
			case PlayerNum.PLAYER_BOT:
				val = gameObject.GetComponent<Bot>().rotationInput;
				break;
        }
        if (Mathf.Abs(val) < 0.8f)
            val = 0.0f;
        return val;
    }

    public void rotateRigidBodyAroundPointBy(Rigidbody rb, Vector3 origin, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        rb.MovePosition(q * (rb.transform.position - origin) + origin);
        rb.MoveRotation(rb.transform.rotation * q);
    }
 
        
}
