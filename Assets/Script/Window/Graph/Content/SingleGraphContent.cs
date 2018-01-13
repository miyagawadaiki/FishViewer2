﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGraphContent : GraphContent {

	//private GraphType graphType = GraphType.Rect;
	public GraphManager graphMan;

	void Awake() {
		
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
		OpenSettingWindow ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Init() {
		graphMan.Init ();
		graphTitleText.text = GetTitle ();

		ShowView();
		//graphMan.ShowAxis ();
	}

	public override void OpenSettingWindow() {
		mwc.mwm.AddWindow ("SingleGraphSetting");
		mwc.mwm.GetLastWindowController ().gameObject.GetComponentInChildren<SingleGraphSettingContent> ().RegisterGraphContent(this);
	}

	public void Set(GraphType gt, string parameters) {
		if (graphMan == null || graphMan.graphType != gt) {
			if (graphMan != null) {
				Simulation.Remove (graphMan);
				Destroy (graphMan.gameObject);
			}
			GameObject obj = Instantiate (Resources.Load ("GraphManager/" + System.Enum.GetName (typeof(GraphType), gt) + "GraphManager"), graphTra) as GameObject;
			graphMan = obj.GetComponent<GraphManager> ();
			memo = graphMan;
			Simulation.Register (graphMan);
		}
		//graphType = gt;

		//Init ();
		graphMan.Set (parameters);
		//graphMan.Init ();

		//SetAxis ();

		Init ();
	}

	public override void Translate(Vector2 start, Vector2 end) {
		if (graphMan == null)
			return;
		graphMan.Translate (start, end);
		ShowView ();
	}

	public override void Expand(float expand) {
		if (graphMan == null)
			return;
		graphMan.Expand (expand);
		ShowView ();
	}

	public override void RemoveGraphManager() {
		Simulation.Remove (graphMan);
	}
		
	public override void SetGrid() {
		if (graphMan == null)
			return;
		graphMan.SetGrid ();
	}

	public override void ShowAxis() {
		if (graphMan == null)
			return;
		graphMan.ShowAxis ();
	}

	public override void ShowGrid() {
		if (graphMan == null)
			return;
		graphMan.ShowGrid ();
	}

	public override void HideView() {
		if (graphMan == null)
			return;
		graphMan.HideView ();
	}

	public override string GetTitle () {
		return graphMan.GetFishText () + ", " + graphMan.GetTypeText ();
	}

	/*
	public override void OnExpand(Vector2 vec) {
		ShowAxis ();
	}
	*/

	void OnDestroy() {
		RemoveGraphManager ();
	}
}
