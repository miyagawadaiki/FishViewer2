﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileReadContent : MyWindowContent {

	[SerializeField]
	private Text nameText = null;
	[SerializeField]
	private Button button = null;
	[SerializeField]
	private Text valueText = null;
	[SerializeField]
	private Transform content = null;
	[SerializeField]
	private GameObject nodeObj = null;
	[SerializeField]
	private Button doneButton = null;

	private bool addListenerFlag = false;
	private StreamReader sr;
	private char[] separator = { ',' };

	public override void Awake () {
		base.Awake ();
		defaultPosition = new Vector2 ();
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();

		//mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.canMove = false;
		mwc.canExpand = false;

		button.onClick.AddListener (() => this.GetComponentInParent<MyWindowManager> ().AddWindow ("FileSelect/Read"));
		button.onClick.AddListener (() => CallAddListener());

		doneButton.onClick.AddListener (() => mwc.Destroy ());
		doneButton.onClick.AddListener (() => Simulation.Init ());
		//doneButton.onClick.AddListener (() => DataBase.SetDataBase ());
		if(File.Exists(ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Read)))
			UpdateContent ();
	}
	
	// Update is called once per frame
	void Update () {
		if (addListenerFlag) {
			AddListener ();
			addListenerFlag = false;
		}
	}

	public void UpdateContent() {
		nameText.text = ProjectData.FileName.GetName (ProjectData.FileKey.Read);

		sr = new StreamReader (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Read), System.Text.Encoding.GetEncoding("UTF-8"));
		string[] tmp = sr.ReadLine ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		valueText.text = tmp [0] + "\n" + tmp [1] + "\n" + tmp [2];
		//int step = int.Parse (tmp [0]);
		int fish = int.Parse (tmp [1]);
		//float deltaTime = float.Parse (tmp [2]);

		foreach (Transform t in content)
			Destroy (t.gameObject);
		tmp = sr.ReadLine ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < tmp.Length / fish; i++) {
			GameObject obj = Instantiate (nodeObj, content) as GameObject;
			obj.GetComponentInChildren<Text> ().text = tmp [i];
		}

		sr.Close ();
	}

	public void CallAddListener() {
		addListenerFlag = true;
	}

	public void AddListener() {
		GameObject.Find("FileSelectContent(Clone)").GetComponent<FileSelectContent>().doneButton.onClick.AddListener(() => UpdateContent());
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
