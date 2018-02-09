using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGraphContent : GraphContent {

	//private GraphType graphType = GraphType.Rect;
	public GraphManager graphMan;

	void Awake() {
		gcType = GraphContentType.Single;
	}

	public override void Init() {
		graphMan.Init ();
		graphTitleText.text = GetTitle ();

		/*
		if (graphMan.graphType.Equals (GraphType.Polar)) {
			graphRecTra.offsetMin = new Vector2 (graphRecTra.offsetMin.x, graphRecTra.offsetMax.x * -1f);
		}
		*/

		ShowView();
		//graphMan.ShowAxis ();
	}

	/*
	public override void OpenSettingWindow() {
		mwc.mwm.AddWindow ("SingleGraphSetting");
		mwc.mwm.GetLastWindowController ().gameObject.GetComponentInChildren<SingleGraphSettingContent> ().RegisterGraphContent(this);
	}
	*/

	public override void Set(string parameters) {
		char[] typeSeparator = { ' ' };
		string[] tmp = parameters.Split (typeSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		GraphType gt = (GraphType)(int.Parse(tmp [0]));
		if (graphMan == null || graphMan.graphType != gt) {
			if (graphMan != null) {
				Simulation.Remove (graphMan);
				Destroy (graphMan.gameObject);
			}
			GameObject obj = Instantiate (Resources.Load ("GraphManager/" + System.Enum.GetName (typeof(GraphType), gt) + "GraphManager"), graphRecTra) as GameObject;
			graphMan = obj.GetComponent<GraphManager> ();
			//memo = graphMan;
			Simulation.Register (graphMan);
		}

		Debug.Log ("SingleGraph = " + tmp [1]);
		graphMan.Set (tmp[1]);

		Init ();
	}

	public override void Plot (int step) {
		base.Plot (step);

		graphMan.Plot (step);
	}

	public override void Translate(Vector2 start, Vector2 end) {
		if (graphMan == null)
			return;
		graphMan.Translate (start, end);

		if (!Simulation.playing)
			graphMan.Plot (Simulation.step);

		base.Translate (start, end);
	}

	public override void Expand(float expand) {
		if (graphMan == null)
			return;
		graphMan.Expand (expand);

		if (!Simulation.playing)
			graphMan.Plot (Simulation.step);
		
		base.Expand (expand);
	}

	public override void RemoveGraphManager() {
		Simulation.Remove (graphMan);
	}
		
	public override void SetGrid() {
		if (graphMan == null)
			return;
		graphMan.SetGrid ();
	}

	public override void SetCompletely() {
		if (graphMan == null)
			return;
		graphMan.SetCompletely (true);
	}

	public override void ShowAxis() {
		base.ShowAxis ();

		if (graphMan == null)
			return;
		graphMan.ShowAxis ();
	}

	public override void ShowGrid() {
		base.ShowGrid ();

		if (graphMan == null)
			return;
		graphMan.ShowGrid ();
	}

	public override void ShowAxisCompletely () {
		base.ShowAxisCompletely ();

		if (graphMan == null)
			return;
		graphMan.ShowAxisCompletely ();
	}

	public override void ShowGridCompletely () {
		base.ShowGridCompletely ();

		if (graphMan == null)
			return;
		graphMan.ShowGridCompletely ();
	}

	public override void HideView() {
		if (graphMan == null)
			return;
		graphMan.HideView ();
	}

	public override string GetTitle () {
		return graphMan.GetFishText () + ", " + graphMan.GetTypeText ();
	}

	public override string GetParameterText() {
		string ret = "";
		if(graphMan != null)
			ret += graphMan.GetParameterText();

		return ret;
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
