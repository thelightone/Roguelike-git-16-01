﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starFxController : MonoBehaviour {

	public GameObject[] starFX;
	public int ea;
	public int currentEa;
	public float delay;
	public float currentDelay;
	public bool isEnd;
	public int idStar;
	public static starFxController myStarFxController;

	private AudioSource _source;

	void Awake () {
		myStarFxController = this;
	}

	void Start () 
	{
		_source =  GetComponent<AudioSource>();
		//Reset ();
	}

    private void OnEnable()
    {
		//Reset();
    }

    void Update () {
		if (!isEnd) {
			currentDelay -= Time.deltaTime;
			if (currentDelay <= 0) {
				if (currentEa != ea) {
					currentDelay = delay;
					starFX [currentEa].SetActive (true);
                    AudioManager.Instance.OnStarAppear();
                    currentEa++;
				} else {
					isEnd = true;
					currentDelay = delay;
					currentEa = 0;
				}
			}
		}
	}

	public void Reset () {
		for (int i = 0; i < 3; i++) {
			starFX [i].SetActive (false);
		}
		currentDelay = delay;
		currentEa = 0;
		isEnd = false;
		for (int i = 0; i < 3; i++) {
			starFX [i].SetActive (false);
		}
	}
}