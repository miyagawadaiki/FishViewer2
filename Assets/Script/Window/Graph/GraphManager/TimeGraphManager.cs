using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGraphManager : GraphManager {

	private UnityEngine.UI.Text xText;

	protected override void Awake() {
		base.Awake ();
		graphType = GraphType.Time;
	}

	// Use this for initialization
	protected override void Start () {

	}

	// Update is called once per frame
	protected override void Update () {

	}

	public override void Init() {
		Debug.Log ("<color=green>TimeGraphManager.Init()</color>");
		base.Init ();

		points [markerIdx].sizeDelta *= 2f;
		xMin = (float)(Simulation.step - markerIdx);
		xMax = (float)(Simulation.step + (pointNum - markerIdx));

		xGrids = new GridLineController[1];
		GameObject obj = Instantiate (gridLineObj, view) as GameObject;
		xGrids[0] = obj.GetComponent<GridLineController>();
		xText = obj.GetComponentInChildren<UnityEngine.UI.Text> ();

		yGrids = new GridLineController[gridNum];
		for (int i = 0; i < gridNum; i++) {
			yGrids[i] = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
		}

		HideView ();
	}
		
	public override void Plot(int step) {
		xMin = (float)(step - markerIdx);
		xMax = (float)(step + (pointNum - markerIdx));
		//Debug.Log ("In RectGraphManager : " + step);
		for (int i = 0; i < pointNum; i++) {
			if (step - markerIdx + i < 0 || step - markerIdx + i >= DataBase.step) {
				points [i].gameObject.SetActive (false);
			} else {
				points [i].gameObject.SetActive (true);
				points [i].localPosition = GraphToLocal (new Vector2 (step - markerIdx + i, DataBase.GetData (step - markerIdx + i, fish, yType)));
				if (!recTra.rect.Contains (points [i].localPosition))
					points [i].gameObject.SetActive (false);
			}
		}

		xText.text = step + "";
	}
		
	public override void Translate(Vector2 start, Vector2 end) {
		Vector2 tmp = LocalToGraph (end) - LocalToGraph (start);
		yMax -= tmp.y;
		yMin -= tmp.y;

		//SetAxis ();
	}

	public override void Expand (float expand) {
		yExp += expand;
		if (yExp < 1f)
			yExp = 1f;
	}

	public override void SetGrid() {
		base.SetGrid ();

		float yMax_ = LocalToRectGraph(new Vector2(0f, view.rect.height / 2f)).y, yMin_ = LocalToGraph(new Vector2(0f, view.rect.height / -2f)).y;

		int yStart = (int)(yMin_ / yGridValue) + (yMin_ > 0 ? 1 : 0);
		for (int i = 0; i < gridNum; i++) {
			yGrids [i].Set (false, GraphToLocal(new Vector2 (Simulation.step, (yStart + i) * yGridValue)), (yStart + i) * yGridValue, TextPos.BelowLeft);
			yGrids [i].isAxis = false;
		}
		yGrids [yAxisValue - yStart].isAxis = true;

		xGrids [0].Set (true, GraphToLocal (new Vector2 (Simulation.step, (float)yAxisValue * yGridValue)), Simulation.step, TextPos.BelowRight);
		xGrids [0].isAxis = false;
	}

	public override void SetCompletely (bool first) {
		base.SetGrid ();

		float xMax_ = LocalToRectGraph(new Vector2(view.rect.width / 2f, 0f)).x, xMin_ = LocalToGraph(new Vector2(view.rect.width / -2f, 0f)).x;
		float yMax_ = LocalToRectGraph(new Vector2(0f, view.rect.height / 2f)).y, yMin_ = LocalToGraph(new Vector2(0f, view.rect.height / -2f)).y;

		int yStart = (int)(yMin_ / yGridValue) + (yMin_ > 0 ? 1 : 0);
		for (int i = 0; i < gridNum; i++) {
			if(first)
				yGrids [i].Set (false, GraphToLocal(new Vector2 (Simulation.step - pointNum / 2, (yStart + i) * yGridValue)), (yStart + i) * yGridValue, TextPos.Left);
			else
				yGrids [i].Set (false, GraphToLocal(new Vector2 (Simulation.step + pointNum / 2, (yStart + i) * yGridValue)), (yStart + i) * yGridValue, TextPos.Right);
			yGrids [i].isAxis = false;
		}

		if (first)
			xGrids [0].Set (true, GraphToLocal (new Vector2 (Simulation.step, yMin_)), Simulation.step, TextPos.Below);
		else
			xGrids [0].Set (true, GraphToLocal (new Vector2 (Simulation.step + pointNum, yMax_)), Simulation.step, TextPos.Below);
		xGrids [0].isAxis = false;

		if (first)
			xAxis.Set (true, view.rect.size / -2f, xMin_, TextPos.Below);
		else
			xAxis.Set (true, view.rect.size / 2f, xMax_, TextPos.Above);

		if (first)
			yAxis.Set (false, view.rect.size / -2f, yMin_, TextPos.Left);
		else
			yAxis.Set (false, view.rect.size / 2f, yMax_, TextPos.Right);
	}

	public override void ShowAxis() {
		base.ShowAxis ();

		for (int i = 0; i < gridNum; i++) {
			if (yGrids [i].isAxis)
				yGrids [i].Draw (true, true);
			else
				yGrids [i].Hide ();
		}

		xGrids [0].Draw (true, true);
	}

	public override void ShowGrid() {
		base.ShowGrid ();

		for (int i = 0; i < gridNum; i++) {
			yGrids [i].Draw (true, true);
		}

		xGrids [0].Draw (true, true);
	}

	public override void ShowAxisCompletely () {
		base.ShowAxisCompletely ();

		for (int i = 0; i < gridNum; i++) {
			yGrids [i].DrawShort ();
		}

		xGrids [0].Draw (true, true);

		xAxis.Draw (true, false);
		yAxis.Draw (true, false);
	}

	public override void ShowGridCompletely () {
		base.ShowGridCompletely ();

		for (int i = 0; i < gridNum; i++) {
			yGrids [i].Draw (true, true);
		}

		xGrids [0].Draw (true, true);

		xAxis.Draw (true, false);
		yAxis.Draw (true, false);
	}

	public override Vector2 GraphToLocal (Vector2 v) {
		return base.RectGraphToLocal (v);
	}

	public override Vector2 LocalToGraph (Vector2 v) {
		return base.LocalToRectGraph (v);
	}

	public override string GetTypeText() {
		return "Time-" + DataBase.GetTags () [yType];
	}
}
