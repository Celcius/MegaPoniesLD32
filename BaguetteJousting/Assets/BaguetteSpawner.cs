using UnityEngine;
using System.Collections;

public class BaguetteSpawner : MonoBehaviour {

    Baguette _baguette;
    float _spawnTimer = 0;
    


	// Use this for initialization
	void Start () {
        _baguette = (Baguette)((GameObject)Instantiate(Resources.Load("Prefabs/Baguette"))).GetComponent<Baguette>();

	}
	
	// Update is called once per frame
	void Update () {

        if (_spawnTimer > 0)
            _spawnTimer -= Time.deltaTime;
        if(_spawnTimer <= 0)
        {
            _spawnTimer = 0;
            spawnBaguette();
        }

	
	}

    public Baguette spawnBaguette()
    {
        if (_baguette == null)
            return null;

        _baguette.transform.position = transform.position;
        _baguette.transform.rotation = transform.localRotation;
        _baguette.transform.active = true;

        return _baguette;
    }

    public void destroyBaguette(bool respawn)
    {
        if (_baguette == null)
            return;

        _baguette.transform.active = false;
        _baguette.transform.position = transform.position;
        _baguette.transform.rotation = transform.localRotation;

        if (respawn)
            _spawnTimer = 5.0f;
    }
}
