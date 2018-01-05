using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGraphContent : GraphContent {

	private GraphManager graphMan;
	private GraphType graphType = GraphType.Rect;

	void Awake() {
		
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();

		/*
		GameObject obj = Instantiate (Resources.Load ("GraphManager/" + typeName + "GraphManager"), this.transform) as GameObject;
		graphMan = obj.GetComponent<GraphManager> ();

		if (Simulation.isEnabled) {
			Init ();
			Simulation.Register (graphMan);
		}
		*/

		OpenSettingWindow ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Init() {
		graphMan.Init ();
		//graphMan.ShowAxis ();
	}

	public override void OpenSettingWindow() {
		mwc.mwm.AddWindow ("SingleGraphSetting");
		mwc.mwm.GetLastWindowController ().gameObject.GetComponentInChildren<SingleGraphSettingContent> ().RegisterGraphContent(this);
	}

	public void Set(GraphType gt, string parameters) {
		if (graphMan == null || graphType != gt) {
			if (graphMan != null)
				Destroy (graphMan.gameObject);
			GameObject obj = Instantiate (Resources.Load ("GraphManager/" + System.Enum.GetName (typeof(GraphType), graphType) + "GraphManager"), this.transform) as GameObject;
			graphMan = obj.GetComponent<GraphManager> ();
			Simulation.Register (graphMan);
		}
		graphType = gt;

		//Init ();
		graphMan.Set (parameters);

		//
		ShowAxis ();
		//
	}

	public override void RemoveGraphManager() {
		Simulation.Remove (graphMan);
	}

	public override void ShowAxis() {
		graphMan.ShowAxis ();
	}

	public override void ShowGrid() {

	}

	void OnDestroy() {
		RemoveGraphManager ();
	}
}
