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

	// Use this for initialization
	public override void Start () {
		base.Start ();
		OpenSettingWindow ();
	}
	
	// Update is called once per frame
	void Update () {
		
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

	public override void OpenSettingWindow() {
		mwc.mwm.AddWindow ("MultiEvenGraphSetting");
		mwc.mwm.GetLastWindowController ().gameObject.GetComponentInChildren<MultiEvenGraphSettingContent> ().RegisterGraphContent (this);
	}

	public void Set(GraphType gt, string[] parameters) {
		//if (graphManList.Count == 0 || graphManList [0].graphType != gt) {
		RemoveGraphManager ();
		foreach (Transform t in plotViewTra)
			Destroy (t.gameObject);

		if (parameters == null)
			return;

		GameObject graphManObj = Resources.Load ("GraphManager/" + System.Enum.GetName (typeof(GraphType), gt) + "GraphManager") as GameObject;
		for (int i = 0; i < parameters.Length; i++) {
			GameObject obj = Instantiate (graphManObj, graphTra) as GameObject;
			graphManList.Add (obj.GetComponent<GraphManager> ());
			Simulation.Register (graphManList [graphManList.Count - 1]);
		}

			memo = graphManList [0];
		//}

		for (int i = 0; i < parameters.Length; i++) {
			graphManList[i].Set (parameters [i]);
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

	void OnDestroy() {
		RemoveGraphManager ();
	}
}
