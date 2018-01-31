using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileMakeContent : MyWindowContent {

	// Use this for initialization
	void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.SetSize (defaultSize);
		mwc.canMove = false;
		mwc.canExpand = false;
		mwc.MoveTo (new Vector2 (0f, 0f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
