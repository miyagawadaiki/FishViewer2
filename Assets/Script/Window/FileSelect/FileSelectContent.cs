using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectData;

public class FileSelectContent : MyWindowContent {

	[SerializeField]
	private FileKey key = FileKey.Read;
	//[SerializeField]
	//private RectTransform recTra = null;

	public Button doneButton;

	//private MyWindowController mwc;
	private FileInputFieldManager fifm;

	//private Vector2 defaultSize = new Vector2 (321f, 294f);

	public override void Awake () {
		base.Awake ();
		defaultPosition = new Vector2 ();
		mwc.canMove = false;
		mwc.canExpand = false;
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();

		key = (FileKey)System.Enum.Parse (typeof(FileKey), typeName);
		fifm = this.GetComponent<FileInputFieldManager> ();
		fifm.key = key;

		doneButton.onClick.AddListener (() => mwc.Destroy ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Set() {
		FileName.Set (key, fifm.GetPath (), fifm.GetName ());
		if (key == FileKey.Input) {
			char[] sep = { '.' };
			string[] tmp = fifm.GetName ().Split (sep, StringSplitOptions.RemoveEmptyEntries);
			FileName.Set (FileKey.Output, FileName.GetPath(FileKey.Output), tmp[0] + "-fv." + tmp[1]);
		}
	}

	/*
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
	*/
}
