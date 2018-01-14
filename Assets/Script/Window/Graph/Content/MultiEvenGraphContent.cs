using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEvenGraphContent : GraphContent {

	[SerializeField]
	private GameObject nodeObj = null;
	[SerializeField]
	private Transform plotViewTra = null;

	public List<GraphManager> graphManList;

	void Awake() {
		graphManList = new List<GraphManager> ();
	}

	public override void Init() {

		foreach (GraphManager gm in graphManList) {
			gm.Init ();
			GameObject obj = Instantiate (nodeObj, plotViewTra) as GameObject;
			obj.GetComponent<MultiEvenNodeController> ().Set (gm.pointColorNum, gm.fish);
		}

		graphTitleText.text = GetTitle ();

		ShowView ();
	}

	/*
	public override void OpenSettingWindow() {
		mwc.mwm.AddWindow ("MultiEvenGraphSetting");
		mwc.mwm.GetLastWindowController ().gameObject.GetComponentInChildren<MultiEvenGraphSettingContent> ().RegisterGraphContent (this);
	}
	*/

	public override void Set(string parameters) {
		RemoveGraphManager ();
		foreach (Transform t in plotViewTra)
			Destroy (t.gameObject);

		if (parameters.Equals(""))
			return;

		char[] fishSeparator = {'\t'}, typeSeparator = { ' ' };
		string[] fishParameters = parameters.Split (fishSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		string[] tmp = fishParameters[0].Split (typeSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		GraphType gt = (GraphType)(int.Parse(tmp [0]));

		GameObject graphManObj = Resources.Load ("GraphManager/" + System.Enum.GetName (typeof(GraphType), gt) + "GraphManager") as GameObject;
		for (int i = 0; i < fishParameters.Length; i++) {
			GameObject obj = Instantiate (graphManObj, graphTra) as GameObject;
			graphManList.Add (obj.GetComponent<GraphManager> ());
			Simulation.Register (graphManList [graphManList.Count - 1]);
		}

		memo = graphManList [0];

		for (int i = 0; i < fishParameters.Length; i++) {
			tmp = fishParameters[i].Split (typeSeparator, System.StringSplitOptions.RemoveEmptyEntries);
			graphManList[i].Set (tmp[1]);
		}

		Init ();
	}

	public override void Translate(Vector2 start, Vector2 end) {
		if (graphManList.Count == 0)
			return;

		foreach (GraphManager gm in graphManList)
			gm.Translate (start, end);

		ShowView ();
	}

	public override void Expand(float expand) {
		if (graphManList.Count == 0)
			return;

		foreach (GraphManager gm in graphManList)
			gm.Expand(expand);

		ShowView ();
	}

	public override void RemoveGraphManager() {
		int num = graphManList.Count;
		for (int i = 0; i < num; i++) {
			GraphManager gm = graphManList [0];
			Simulation.Remove (gm);
			graphManList.RemoveAt (0);
			Destroy (gm.gameObject);
		}
	}

	public override void SetGrid() {
		if (graphManList.Count == 0)
			return;

		graphManList [graphManList.Count - 1].SetGrid ();
	}

	public override void ShowAxis() {
		if (graphManList.Count == 0)
			return;

		graphManList [graphManList.Count - 1].ShowAxis ();
	}

	public override void ShowGrid() {
		if (graphManList.Count == 0)
			return;

		graphManList [graphManList.Count - 1].ShowGrid ();
	}

	public override void HideView() {
		if (graphManList.Count == 0)
			return;

		graphManList [graphManList.Count - 1].HideView ();
	}

	public override string GetTitle () {
		string fishText = "fish";
		foreach (GraphManager gm in graphManList)
			fishText += (gm.fish + 1) + ",";

		return fishText + " " + graphManList [0].GetTypeText ();
	}

	public override string GetParameterText () {
		string ret = "";
		if (graphManList.Count > 0) {
			foreach (GraphManager gm in graphManList) {
				ret += gm.GetParameterText () + "\t";
			}
		}

		return ret;
	}

	void OnDestroy() {
		RemoveGraphManager ();
	}
}
