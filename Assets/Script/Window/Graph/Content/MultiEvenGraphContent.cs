using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEvenGraphContent : GraphContent {

	[SerializeField]
	private GameObject nodeObj = null;
	[SerializeField]
	private Transform plotViewTra = null;

	public List<GraphManager> graphManList;

	public override void Awake() {
		gcType = GraphContentType.MultiEven;
		base.Awake ();
		//graphManList = new List<GraphManager> ();

	}

	public override bool IsReady () {
		return graphManList != null && graphManList.Count > 0;
	}

	public override bool Equals (string parameter) {
		char[] groupSeparator = { ':' };//, typeSeparator = { ' ' }, allSeparator = { ':', ' ', ',' };
		string[] tmp = parameter.Split (groupSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		string[] thistmp = this.GetParameterText ().Split (groupSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		return thistmp [0].Equals (tmp [0]) && thistmp [2].Equals (tmp [2]);
	}

	public override void Init() {
		
		foreach (Transform t in plotViewTra)
			Destroy (t.gameObject);
		
		foreach (GraphManager gm in graphManList) {
			gm.Init ();
			GameObject obj = Instantiate (nodeObj, plotViewTra) as GameObject;
			obj.GetComponent<MultiEvenNodeController> ().Set (gm.pointColorNum, gm.fish);
			obj.GetComponent<MultiEvenNodeController> ().RegisterTransform (gm.transform);

			gm.HideView ();
		}

		graphTitleText.text = GetTitle ();

		/*
		if (graphManList [0].graphType.Equals (GraphType.Polar)) {
			graphRecTra.offsetMin = new Vector2 (graphRecTra.offsetMin.x, graphRecTra.offsetMax.x * -1f);
		}
		*/

		ShowView ();

		base.Init ();
	}

	/*
	public override void OpenSettingWindow() {
		mwc.mwm.AddWindow ("MultiEvenGraphSetting");
		mwc.mwm.GetLastWindowController ().gameObject.GetComponentInChildren<MultiEvenGraphSettingContent> ().RegisterGraphContent (this);
	}
	*/

	public override void Set(string parameters) {
		//Debug.Log (parameters);
		RemoveGraphManager ();

		char[] groupSeparator = { ':' }, typeSeparator = { ' ' }, allSeparator = { ':', ' ', ',' };
		string[] groupTexts = parameters.Split (groupSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		string[] typeText = groupTexts [0].Split (typeSeparator);
		GraphType gt = (GraphType)(int.Parse (typeText[0]));
		GameObject graphManObj = Resources.Load ("GraphManager/" + System.Enum.GetName (typeof(GraphType), gt) + "GraphManager") as GameObject;

		string[] fishTexts = groupTexts [1].Split (allSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		string[] sourceTexts = groupTexts [2].Split (allSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		string[] pointerTexts = groupTexts [3].Split (allSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		List<int> fishNumList = new List<int> ();
		if (fishTexts [0][0] == 'a') {
			for (int i = 0; i < DataBase.fish; i++)
				fishNumList.Add (i);
		} else {
			for (int i = 0; i < fishTexts.Length; i++)
				if (int.Parse (fishTexts [i]) > 0)
					fishNumList.Add (i);
		}

		if (fishNumList.Count <= 0)
			return;

		Simulation.Register (this);

		graphManList = new List<GraphManager> ();
		for (int i = 0; i < fishNumList.Count; i++) {
			GameObject obj = Instantiate (graphManObj, graphRecTra) as GameObject;
			graphManList.Add (obj.GetComponent<GraphManager> ());
			//Simulation.Register (graphManList [graphManList.Count - 1]);

			string p = fishNumList [i] + "," + sourceTexts [1] + "," + sourceTexts [2] + "," + sourceTexts [3] + ",";
			p += pointerTexts [0] + "," + pointerTexts [1] + "," + fishNumList [i] + "," + pointerTexts [3] + "," + pointerTexts [4] + "," + pointerTexts [5] + "," + pointerTexts [6] + ",";

			//Debug.Log (p);
			graphManList [i].Set (p);
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

		foreach (GraphManager gm in graphManList)
			gm.Translate (start, end);

		if (!Simulation.playing) {
			foreach (GraphManager gm in graphManList)
				gm.Plot (Simulation.step);
		}

		base.Translate (start, end);
	}

	public override void Expand(float expand) {
		if (graphManList.Count == 0)
			return;

		foreach (GraphManager gm in graphManList)
			gm.Expand(expand);

		if (!Simulation.playing) {
			foreach (GraphManager gm in graphManList)
				gm.Plot (Simulation.step);
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

		graphManList [0].SetGrid ();
	}

	public override void SetCompletely () {
		if (graphManList.Count == 0)
			return;

		graphManList [0].SetCompletely (true);
	}

	public override void ShowAxis() {
		if (graphManList.Count == 0)
			return;

		graphManList [0].ShowAxis ();
	}

	public override void ShowGrid() {
		if (graphManList.Count == 0)
			return;

		graphManList [0].ShowGrid ();
	}

	public override void ShowAxisCompletely() {
		if (graphManList.Count == 0)
			return;

		graphManList [0].ShowAxisCompletely ();
	}

	public override void ShowGridCompletely() {
		if (graphManList.Count == 0)
			return;

		graphManList [0].ShowGridCompletely ();
	}

	public override void HideView() {
		if (graphManList.Count == 0)
			return;

		graphManList [0].HideView ();
	}

	public override string GetTitle () {
		string fishText = "fish";
		foreach (GraphManager gm in graphManList)
			fishText += (gm.fish + 1) + ",";

		return graphManList[0].GetStepText () + ", " + fishText + "\n" + graphManList [0].GetTypeText ();
	}

	public override string GetShortTypeText () {
		return base.GetShortTypeText () + DataBase.GetShortTags () [graphManList [0].xType] + "-" + DataBase.GetShortTags () [graphManList [0].yType];
	}

	public override string GetShortTitle () {
		string s = base.GetShortTitle ();
		return s + graphManList [0].GetStepText () + "_" + GetShortTypeText () + "_me";
	}

	public override string GetParameterText () {
		string ret = "";

		if (graphManList.Count == 0)
			return ret;

		char[] typeSeparator = { ':', ' ' }, paramSeparator = { ':', ' ', ',' };
		string[] typeTexts = graphManList [0].GetParameterText ().Split(typeSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		ret += typeTexts [0] + " :";

		int[] fishMemo = new int[DataBase.fish];
		foreach (GraphManager gm in graphManList) {
			string[] tmp = gm.GetParameterText ().Split (paramSeparator, System.StringSplitOptions.RemoveEmptyEntries);
			fishMemo [int.Parse (tmp [1])] = 1;
		}
		foreach (int i in fishMemo)
			ret += i + ",";
		ret += ":";

		ret += typeTexts [1] + ":" + typeTexts [2] + ":";
		//Debug.Log ("ret = " + ret);

		return ret;
	}

	void OnDestroy() {
		RemoveGraphManager ();
	}
}
