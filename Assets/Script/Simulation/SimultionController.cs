using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimultionController : MonoBehaviour {

	private bool playing = false, repeating = false;
	private float time = 0f, dt = 0.1f;

	void Awake() {
		Simulation.Init ();
		dt = DataBase.dt;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		time += Time.deltaTime;

		if (time < dt)
			return;

		time = 0f;

		if (playing) {
			Debug.Log ("<color=blue>Simulating... step=" + Simulation.step + "</color>");
			Simulation.GoNext ();
		}

		if (repeating) {

		}
	}

}
