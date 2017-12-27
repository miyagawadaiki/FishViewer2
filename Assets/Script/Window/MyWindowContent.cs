using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyWindowContent : MonoBehaviour {

	public MyWindowController mwc;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//public abstract MyWindowContent Clone ();

	public abstract void OnLeftClick (Vector2 pos);

	public abstract void OnRightClick (Vector2 pos);

	public abstract void OnLeftDrag (Vector2 start, Vector2 end);

	public abstract void OnRightDrag (Vector2 start, Vector2 end);

	public abstract void OnWheelChange (float value);
}
