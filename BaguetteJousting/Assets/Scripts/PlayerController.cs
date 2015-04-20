using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour {

    enum PlayerNum
    {
        PLAYER_ONE,
        PLAYER_TWO,
        PLAYER_THREE,
        PLAYER_FOUR
    };

    const float DIR_MULT = 0.3f;
    const float ACCEL_MOD = 0.01f;
    const float ACCEL_ROTATE_MOD = 0.05f;
    const float MAX_ACCEL_ROTATE = 0.5f;
    const float MAX_ACCEL = 0.4f;
    const float ROTATE_SPEED = 300.0f;
    const float STILL_ROTATE_SPEED = 190.5f;
    const float MAX_SPEED_ROTATE_MOD = ROTATE_SPEED - STILL_ROTATE_SPEED;
    const float MAX_SPEED = 12.0f;
    const float MAX_STILL_SPEED = 5.0f;
    const float MAX_BACK_SPEED = -5.0f;
    const float ACCEL_DECREASE =800.0f;
    const float ACCEL_PUSH_DECREASE = 0.1f;
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

	bool _isBot = false;

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


    Vector3 _pushDir;
    float _pushAccel = 0;
    float _pushSpeed = 0;


	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().Stop();
        _front = GetComponentInChildren<FrontRef>().transform;
        _back = GetComponentInChildren<BackRef>().transform;
        _baseY = transform.position.y;
        _trail = GetComponentInChildren<TrailRenderer>();
        
        switch(_playerNum)
        {
            case PlayerNum.PLAYER_ONE:
                _trail.GetComponent<Renderer>().material = Resources.Load("Materials/Red") as Material;
                _bikes[0].gameObject.SetActive(true);
                if (ServiceLocator.instance._bots[0])
                {
                    setBot();
                }
                break;
            case PlayerNum.PLAYER_TWO:
                _trail.GetComponent<Renderer>().material = Resources.Load("Materials/Blue") as Material;
                _bikes[1].gameObject.SetActive(true);
                if (ServiceLocator.instance._bots[1])
                {
                    setBot();
                }
                break;
            case PlayerNum.PLAYER_THREE:
                _trail.GetComponent<Renderer>().material = Resources.Load("Materials/Yellow") as Material;
				_bikes[2].gameObject.SetActive(true);
                if (ServiceLocator.instance._bots[2])
                {
                    setBot();
                }
                break;
            case PlayerNum.PLAYER_FOUR:
                _trail.GetComponent<Renderer>().material = Resources.Load("Materials/Purple") as Material;
				_bikes[3].gameObject.SetActive(true);
                if (ServiceLocator.instance._bots[3])
                {
                    setBot();
                }
                break;
        }
	}

    void setBot()
    {
        bool isHard = ServiceLocator.instance._hardBots;
		GetComponent<Bot> ().dumbness = (isHard) ? 0.1f : 0.8f;
		GetComponent<Bot>().isBot = true;
        _isBot = true;
    }
    public int getPlayerIndex()
    {
        switch(_playerNum)
        {
            case PlayerNum.PLAYER_ONE:
                return 0;
            case PlayerNum.PLAYER_TWO:
                return 1;
            case PlayerNum.PLAYER_THREE:
                return 2;
            case PlayerNum.PLAYER_FOUR:
                return 3;
        }
        return -1;
    }

    public void setPlayerNum(int i)
    {
        if (i == 0)
            _playerNum = PlayerNum.PLAYER_ONE;
        if (i == 1)
            _playerNum = PlayerNum.PLAYER_TWO;
        if (i == 2)
            _playerNum = PlayerNum.PLAYER_THREE;
        if (i == 3)
            _playerNum = PlayerNum.PLAYER_FOUR;
    }

	public Vector3 FacingDirection(){
		return (_front.position - _back.position).normalized;
		}

	// Update is called once per frame
	void Update () {

         checkForActionButtonPressed();
        if (_front == null || _back == null)
            return;


        float x = getRotation();

        if (_isStill)
        {
            rotateRigidBodyAroundPointBy(GetComponent<Rigidbody>(), _back.position, new Vector3(0, 1, 0), x * ROTATE_SPEED * Time.deltaTime);
            //rigidBody.RotateAround(_back.position, new Vector3(0.0f, x, 0.0f), ROTATE_SPEED * Time.deltaTime);
            if(x != 0) 
                _acceleration = ACCEL_ROTATE_MOD;

            
        }
        else
        {
            float rotateMult = (ROTATE_SPEED - STILL_ROTATE_SPEED)  * (1- Mathf.Abs(_velocity) / MAX_SPEED) + STILL_ROTATE_SPEED;
            rotateRigidBodyAroundPointBy(GetComponent<Rigidbody>(), _back.position, Vector3.up, x * rotateMult * Time.deltaTime);
            //transform.Rotate(new Vector3(0, x * rotateMult*Time.deltaTime, 0));

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

        _pushAccel += ACCEL_PUSH_DECREASE;
        _pushSpeed -= _pushAccel;
        if (_pushSpeed <= 0)
        { 
            _pushAccel = 0;
            _pushSpeed = 0;
        }


        dirVector *=_velocity* Time.deltaTime;

        dirVector += _pushDir * _pushSpeed * Time.deltaTime;

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
   //     _shadow.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
     //   if (dirVector.y > _baseY)
       //     dirVector.y = _baseY;



        if(transform.position.y - _baseY < 2.8f)
            GetComponent<Rigidbody>().MovePosition(transform.position + dirVector);


        checkSound();
	}

    public void addPushForce(Vector3 dir, float accel)
    {
        _pushSpeed = accel;
        _pushAccel = 0;
        _pushDir = dir.normalized;
    }

    float getMovement()
    {
        if (!Arena.instance.roundStarted)
            return 0;

        float val = 0;

		if(_isBot){
			return gameObject.GetComponent<Bot>().movementInput;
		}

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
        }
        if (Mathf.Abs(val) < 0.8f)
            val = 0.0f;
        return val;
    }
    
    float getRotation()
    {
        if (!Arena.instance.roundStarted)
            return 0;

        float val = 0;
		if(_isBot){
			return gameObject.GetComponent<Bot>().rotationInput;
		}
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
        }
        if (Mathf.Abs(val) < 0.8f)
            val = 0.0f;
        return val;
    }

    void playHonk()
    {
        //AudioClip clip = Resources.Load("185806__jkaas28__bike-horn") as AudioClip;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }
    void checkSound()
    {
        if (_isBot)
        {
            return;
        }
        switch (_playerNum)
        {
            case PlayerNum.PLAYER_ONE:
                if (Input.GetKey(KeyCode.LeftShift)
                    | Input.GetKey(KeyCode.Joystick1Button1)
                    || Input.GetKey(KeyCode.Joystick1Button2)
                    || Input.GetKey(KeyCode.Joystick1Button3))
                    playHonk();
                break;
            case PlayerNum.PLAYER_TWO:
                if (Input.GetKey(KeyCode.RightShift)
                    || Input.GetKey(KeyCode.Joystick2Button1)
                    || Input.GetKey(KeyCode.Joystick2Button2)
                    || Input.GetKey(KeyCode.Joystick2Button3))
                    playHonk();
                break;
                    playHonk();
                break;
            case PlayerNum.PLAYER_THREE:
                if (Input.GetKey(KeyCode.Joystick3Button1)
                    || Input.GetKey(KeyCode.Joystick3Button2)
                    || Input.GetKey(KeyCode.Joystick3Button3))
                    playHonk();
                break;
            case PlayerNum.PLAYER_FOUR:
                if (Input.GetKey(KeyCode.Joystick4Button1)
                    || Input.GetKey(KeyCode.Joystick4Button2)
                    || Input.GetKey(KeyCode.Joystick4Button3))
                    playHonk();
                break;
                break;
        }
    }

    void checkForActionButtonPressed()
    {
        switch (_playerNum)
        {
            case PlayerNum.PLAYER_ONE:
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                    GetComponent<Pawn>().useAction();
                break;
            case PlayerNum.PLAYER_TWO:
                if(Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.Joystick2Button0))
                    GetComponent<Pawn>().useAction();
                break;
            case PlayerNum.PLAYER_THREE:
                if (Input.GetKeyDown(KeyCode.Joystick3Button0))
                    GetComponent<Pawn>().useAction();
                break;
            case PlayerNum.PLAYER_FOUR:
                if (Input.GetKeyDown(KeyCode.Joystick4Button0))
                    GetComponent<Pawn>().useAction();
                break;
            default:
                break;
        }
    }

    public void rotateRigidBodyAroundPointBy(Rigidbody rb, Vector3 origin, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        rb.MovePosition(q * (rb.transform.position - origin) + origin);
        rb.MoveRotation(rb.transform.rotation * q);
    }
 
        
}
