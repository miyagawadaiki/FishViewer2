using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWindowContent : MonoBehaviour {

	public MyWindowController mwc;
	public string typeName;
	public Vector2 defaultSize;
	public Vector2 defaultPosition;

	public virtual void Awake () {
		mwc = this.GetComponentInParent<MyWindowController> ();

		int num = mwc.mwm.Count ();
		defaultPosition = new Vector2 (0.1f * num - 1f, 1f - 0.1f * num);
	}

	// Use this for initialization
	public virtual void Start () {
		mwc.SetSize (defaultSize);
		mwc.MoveTo (defaultPosition);
		mwc.TranslateIntoWindowManager ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void MakeClone () {

	}

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
