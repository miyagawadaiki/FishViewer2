using System.Collections;
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
			GameObject obj = Instantiate (Resources.Load ("GraphManager/" + System.Enum.GetName (typeof(GraphType), gt) + "GraphManager"), this.transform) as GameObject;
			graphMan = obj.GetComponent<GraphManager> ();
			memo = graphMan;
			Simulation.Register (graphMan);
		}
		//graphType = gt;

		//Init ();
		graphMan.Set (parameters);
		graphMan.Init ();

		//SetAxis ();
		ShowView();


		//mGraphType = gt;
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

	public override void ShowView() {
		switch(viewMode) {
		case ViewMode.ShowAxis:
			SetAxis ();
			break;
		case ViewMode.ShowGrid:
			SetAxis ();
			SetGrid ();
			break;
		case ViewMode.Hide:
			HideView ();
			break;
		default:
			break;
		}
	}

	public override void SetAxis() {
		if (graphMan == null)
			return;
		graphMan.SetAxis ();
	}

	public override void SetGrid() {
		if (graphMan == null)
			return;
		graphMan.SetGrid ();
	}

	public override void HideView() {
		if (graphMan == null)
			return;
		graphMan.HideView ();
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
