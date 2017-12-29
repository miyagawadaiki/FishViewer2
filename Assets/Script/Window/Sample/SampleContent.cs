﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleContent : MyWindowContent {

	// Use this for initialization
	void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		int num = mwc.mwm.Count ();
		Vector2 tmp = mwc.mwm.GetComponent<RectTransform> ().rect.size;
		Vector2 hoge = mwc.GetComponent<RectTransform> ().rect.size;
		Vector2 topLeft = new Vector2 (-tmp.x / 2 + hoge.x / 2, tmp.y / 2 - hoge.y / 2);
		mwc.MoveTo (topLeft + new Vector2 (10f * num + 10f, - 10f * num - 10f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnLeftClick(Vector2 pos) {
		mwc.AppearMenu ();
	}

	public override void OnRightClick(Vector2 pos) {
		
	}

	public override void OnLeftDrag(Vector2 start, Vector2 end) {

	}

	public override void OnRightDrag(Vector2 start, Vector2 end) {

	}

	public override void OnWheelChange(float value) {

	}
}