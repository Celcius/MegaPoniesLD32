using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum roundState
{
    ROUND_STATE_INVALID = 0,
    ROUND_STATE_3 = 1,
    ROUND_STATE_2 = 2,
    ROUND_STATE_1 = 3,
    ROUND_STATE_START = 4,
    ROUND_STATE_NUM= 5,
}

public class RoundL : MonoBehaviour {

    [SerializeField]
    Text roundText1;
    [SerializeField]
    Text roundText2;

    RectTransform _image;
    float time = 0.0f;

    roundState state = roundState.ROUND_STATE_INVALID;

    public void roundStart()
    {
        time = 0.01f;
    }

	// Use this for initialization
	void Start () {
        _image = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
            if(time <= 0)
            {
                time = 0.0f;
                if (state < roundState.ROUND_STATE_START)
                    state = (state + 1 % (int)roundState.ROUND_STATE_NUM);
                else
                {
                    if(state == roundState.ROUND_STATE_START)
                        Arena.instance.startRound();
                    state = roundState.ROUND_STATE_INVALID;
                }
                    

                switch (state)
                {
                    case roundState.ROUND_STATE_INVALID:
                        roundText1.text = "";
                        roundText2.text = "";
                        _image.sizeDelta = new Vector2(0, 0);
                        
                        return;
                    case roundState.ROUND_STATE_3:
                        roundText1.text = "3";
                        roundText2.text = "3";
                        _image.sizeDelta = new Vector2(116, 116);
                        time = 1.0f;
                        return;
                    case roundState.ROUND_STATE_2:
                        roundText1.text = "2";
                        roundText2.text = "2";
                        _image.sizeDelta = new Vector2(116, 116);
                        time = 1.0f;
                        return;
                    case roundState.ROUND_STATE_1:
                        roundText1.text = "1";
                        roundText2.text = "1";
                        _image.sizeDelta = new Vector2(116, 116);
                        time = 1.0f;
                        return;
                    case roundState.ROUND_STATE_START:
                        roundText1.text = "Grab Your Baguettes!";
                        roundText2.text = "Grab Your Baguettes!";
                        _image.sizeDelta = new Vector2(700, 116);
                        time = 1.0f;
                        return;
                }
            }
        }
	    
	}
}
