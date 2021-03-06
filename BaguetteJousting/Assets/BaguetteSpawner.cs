﻿using UnityEngine;
using System.Collections;

public class BaguetteSpawner : MonoBehaviour {

    Baguette _baguette;
    float _spawnTimer = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (_spawnTimer > 0)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0)
            {
                _spawnTimer = 0;
                spawnBaguette();
            }

        }


	
	}

    public Baguette spawnBaguette()
    {
        _spawnTimer = 0.0f;
        if (_baguette != null)
        {
            Destroy(_baguette);
        }

        _baguette = (Baguette)((GameObject)Instantiate(Resources.Load("Prefabs/Baguette"))).GetComponent<Baguette>();
        _baguette.registerSpawner(this);

        Arena.instance.allPickups.Add(_baguette);

        _baguette.gameObject.SetActive(true);
        
        _baguette.transform.position = transform.position;
        _baguette.transform.rotation = transform.rotation;
        

        return _baguette;
    }

    public void destroyBaguette(bool respawn)
    {

        if (respawn)
            _spawnTimer = 5.0f;
        else
            _spawnTimer = 0.0f;

        if (_baguette == null)
            return;
        Arena.instance.allPickups.Remove(_baguette.GetComponent<Pickup>());
        Destroy(_baguette);

    }
}
