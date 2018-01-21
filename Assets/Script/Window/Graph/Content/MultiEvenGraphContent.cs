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
		gcType = GraphContentType.MultiEven;
	}

	public override void Init() {

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
		foreach (Transform t in plotViewTra)
			Destroy (t.gameObject);

		char[] groupSeparator = { ':' }, typeSeparator = { ' ' }, allSeparator = { ':', ' ', ',' };
		string[] groupTexts = parameters.Split (groupSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		string[] typeText = groupTexts [0].Split (typeSeparator);
		GraphType gt = (GraphType)(int.Parse (typeText[0]));
		GameObject graphManObj = Resources.Load ("GraphManager/" + System.Enum.GetName (typeof(GraphType), gt) + "GraphManager") as GameObject;

		string[] fishTexts = groupTexts [1].Split (allSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		string[] sourceTexts = groupTexts [2].Split (allSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		string[] pointerTexts = groupTexts [3].Split (allSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		List<int> fishNumList = new List<int> ();
		for (int i = 0; i < fishTexts.Length; i++)
			if (int.Parse (fishTexts [i]) > 0)
				fishNumList.Add (i);

		if (fishNumList.Count <= 0)
			return;

		for (int i = 0; i < fishNumList.Count; i++) {
			GameObject obj = Instantiate (graphManObj, graphRecTra) as GameObject;
			graphManList.Add (obj.GetComponent<GraphManager> ());
			Simulation.Register (graphManList [graphManList.Count - 1]);

			string p = fishNumList [i] + "," + sourceTexts[1] + "," + sourceTexts[2] + ",";
			p += pointerTexts [0] + "," + fishNumList [i] + "," + pointerTexts[2] + "," + pointerTexts[3] + "," + pointerTexts[4] + "," + pointerTexts[5] + ",";

			//Debug.Log (p);
			graphManList [i].Set (p);
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

		return fishText + "\n" + graphManList [0].GetTypeText ();
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
		Debug.Log ("ret = " + ret);

		return ret;
	}

	void OnDestroy() {
		RemoveGraphManager ();
	}
}
