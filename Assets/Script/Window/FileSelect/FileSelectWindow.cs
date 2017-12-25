using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectData;

public class FileSelectWindow : MyWindowContent {

	[SerializeField]
	private FileKey key = FileKey.Read;
	[SerializeField]
	private RectTransform recTra = null;

	private MyWindowController mwc;
	private FileInputFieldManager fifm;

	// Use this for initialization
	void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.canMove = false;
		mwc.canExpand = false;
		fifm = this.GetComponent<FileInputFieldManager> ();
		fifm.key = key;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Set() {
		FileName.Set (key, fifm.GetPath (), fifm.GetName ());
	}

	public override void OnLeftClick(Vector2 pos) {

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
