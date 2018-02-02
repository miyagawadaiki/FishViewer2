using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWindowContent : MonoBehaviour {

	public MyWindowController mwc;
	public string typeName;
	public Vector2 defaultSize;

	// Use this for initialization
	void Start () {
		/*
		mwc = this.GetComponentInParent<MyWindowController> (); 
		 */

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//public abstract MyWindowContent Clone ();

	public virtual void OnTranslate(Vector2 vec) {

	}

	public virtual void OnExpand(Vector2 vec, Vector2 expandDir) {

	}

	public virtual void OnLeftClick (Vector2 pos) {
		mwc.AppearMenu ();
	}

	public virtual void OnRightClick (Vector2 pos) {

	}

	public virtual void OnLeftDrag (Vector2 start, Vector2 end) {
		
	}

	public virtual void OnRightDrag (Vector2 start, Vector2 end) {

	}

	public virtual void OnWheelChange (float value) {

	}
}
