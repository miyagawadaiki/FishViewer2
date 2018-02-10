using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiVariousGraphContent : GraphContent {

	[SerializeField]
	private Toggle allToggle = null;
	[SerializeField]
	private GameObject nodeObj = null;
	[SerializeField]
	private Transform plotViewTra = null;

	public List<GraphManager> graphManList;
	public List<MultiVariousLabelNode> nodes;
	public GraphManager selected;

	public override void Awake() {
		base.Awake ();
		graphManList = new List<GraphManager> ();
		gcType = GraphContentType.MultiVarious;

		allToggle.gameObject.SetActive (false);
	}

	public override bool IsReady () {
		return graphManList != null;
	}

	public void Select(int index) {
		if (graphManList.Count == 0 || index < 0 || index > graphManList.Count - 1)
			return;

		for (int i = 0; i < nodes.Count; i++)
			nodes [i].UnSelect ();

		nodes [index].Select ();
		selected = graphManList [index];
		selected.transform.SetAsLastSibling ();
	}

	public override void Init () {
		foreach (GraphManager gm in graphManList) {
			gm.Init ();
			GameObject obj = Instantiate (nodeObj, plotViewTra) as GameObject;
			nodes.Add (obj.GetComponent<MultiVariousLabelNode> ());
			nodes [nodes.Count - 1].Set (nodes.Count - 1, gm.pointColorNum, gm.GetLabelText ());
		}

		graphTitleText.text = GetTitle ();

		selected = graphManList [0];

		allToggle.gameObject.SetActive (true);
		allToggle.isOn = false;

		ShowView ();

		base.Init ();
	}

	public override void Set(string parameters) {
		RemoveGraphManager ();

		nodes.Clear ();
		foreach (Transform t in plotViewTra)
			Destroy (t.gameObject);

		if (parameters.Equals ("")) {
			allToggle.gameObject.SetActive (false);
			return;
		}

		char[] graphSeparator = {'\t'}, typeSeparator = { ' ' };
		string[] graphParameters = parameters.Split (graphSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		Simulation.Register (this);

		for (int i = 0; i < graphParameters.Length; i++) {
			string[] tmp = graphParameters [i].Split (typeSeparator, System.StringSplitOptions.RemoveEmptyEntries);
			string graphTypeName = System.Enum.GetNames(typeof(GraphType))[(int.Parse (tmp [0]))];
			GameObject graphManObj = Resources.Load ("GraphManager/" + graphTypeName + "GraphManager") as GameObject;
			GameObject obj = Instantiate (graphManObj, graphRecTra) as GameObject;
			graphManList.Add (obj.GetComponent<GraphManager> ());
			//Simulation.Register (graphManList [graphManList.Count - 1]);

			Debug.Log ("MultiVarious " + i + " = " + tmp [1]);
			graphManList [i].Set (tmp [1]);
		}

		Init ();
	}

	public override void Plot (int step) {
		base.Plot (step);

		foreach (GraphManager gm in graphManList) {
			gm.Plot (step);
		}
	}

	public override void Translate(Vector2 start, Vector2 end) {
		if (graphManList.Count == 0)
			return;

		if (allToggle.isOn) {
			foreach (GraphManager gm in graphManList)
				gm.Translate (start, end);

			if (!Simulation.playing) {
				foreach (GraphManager gm in graphManList)
					gm.Plot (Simulation.step);
			}
		} else {
			selected.Translate (start, end);

			if (!Simulation.playing) {
				selected.Plot (Simulation.step);
			}
		}

		base.Translate (start, end);
	}

	public override void Expand(float expand) {
		if (graphManList.Count == 0)
			return;
		
		if (allToggle.isOn) {
			foreach (GraphManager gm in graphManList)
				gm.Expand (expand);

			if (!Simulation.playing) {
				foreach (GraphManager gm in graphManList)
					gm.Plot (Simulation.step);
			}
		} else {
			selected.Expand (expand);

			if (!Simulation.playing) {
				selected.Plot (Simulation.step);
			}
		}

		base.Expand (expand);
	}

	public override void RemoveGraphManager() {
		base.RemoveGraphManager ();

		int num = graphManList.Count;
		for (int i = 0; i < num; i++) {
			GraphManager gm = graphManList [0];
			//Simulation.Remove (gm);
			graphManList.RemoveAt (0);
			Destroy (gm.gameObject);
		}
	}

	public override void SetGrid() {
		if (graphManList.Count == 0)
			return;

		selected.SetGrid ();
	}

	public override void SetCompletely() {
		if (graphManList.Count == 0)
			return;

		graphManList [0].SetCompletely (true);
		graphManList [0].transform.SetAsLastSibling ();
		if (graphManList.Count >= 2) {
			graphManList [1].SetCompletely (false);
			graphManList [1].transform.SetAsLastSibling ();
		}
	}

	public override void ShowAxis() {
		if (graphManList.Count == 0)
			return;

		selected.ShowAxis ();
	}

	public override void ShowGrid() {
		if (graphManList.Count == 0)
			return;

		selected.ShowGrid ();
	}

	public override void ShowAxisCompletely() {
		if (graphManList.Count == 0)
			return;
		
		graphManList [0].ShowAxisCompletely ();
		if (graphManList.Count >= 2)
			graphManList [1].ShowAxisCompletely ();
	}

	public override void ShowGridCompletely() {
		if (graphManList.Count == 0)
			return;

		graphManList [0].ShowGridCompletely ();
		if (graphManList.Count >= 2)
			graphManList [1].ShowAxisCompletely ();
	}

	public override void HideView() {
		if (graphManList.Count == 0)
			return;

		foreach (GraphManager gm in graphManList)
			gm.HideView ();
	}

	public override string GetTitle () {
		return "Various Graphs";
	}

	public override string GetShortTypeText () {
		return base.GetShortTypeText () + DataBase.GetShortTags () [graphManList [0].xType] + "-" + DataBase.GetShortTags () [graphManList [0].yType];
	}

	public override string GetShortTitle () {
		string s = base.GetShortTitle ();
		return s + graphManList [0].GetStepText () + "_" + GetShortTypeText () + "_mv";
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
