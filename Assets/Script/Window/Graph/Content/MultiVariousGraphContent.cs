using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiVariousGraphContent : GraphContent {

	//[SerializeField]
	//private MultiVariousPlotViewController mvpvc = null;

	public List<GraphManager> graphManList;
	public GraphManager selected;

	void Awake() {
		graphManList = new List<GraphManager> ();
		gcType = GraphContentType.MultiVarious;
	}

	public override void Init () {
		foreach (GraphManager gm in graphManList) {
			gm.Init ();
		}

		selected = graphManList [0];

		ShowView ();
	}

	public override void Set(string parameters) {
		RemoveGraphManager ();

		if (parameters.Equals (""))
			return;

		char[] graphSeparator = {'\t'}, typeSeparator = { ' ' };
		string[] graphParameters = parameters.Split (graphSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < graphParameters.Length; i++) {
			string[] tmp = graphParameters [i].Split (typeSeparator, System.StringSplitOptions.RemoveEmptyEntries);
			string graphTypeName = System.Enum.GetNames(typeof(GraphType))[(int.Parse (tmp [0]))];
			GameObject graphManObj = Resources.Load ("GraphManager/" + graphTypeName + "GraphManager") as GameObject;
			GameObject obj = Instantiate (graphManObj, graphRecTra) as GameObject;
			graphManList.Add (obj.GetComponent<GraphManager> ());
			Simulation.Register (graphManList [graphManList.Count - 1]);

			Debug.Log ("MultiVarious " + i + " = " + tmp [1]);
			graphManList [i].Set (tmp [1]);
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

		if (graphManList.Count == 0)
			return ret;

		foreach (GraphManager gm in graphManList) {
			ret += gm.GetParameterText () + "\t";
		}

		return ret;
	}

	void OnDestroy() {
		RemoveGraphManager ();
	}
}
