using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGraphManager : GraphManager {

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
		xGrids[0] = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
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
			if (yStart + i == yAxisValue)
				yGrids [i].isAxis = true;
			else
				yGrids [i].isAxis = false;
		}

		xGrids [0].Set (true, GraphToLocal (new Vector2 (Simulation.step, (float)yAxisValue * yGridValue)), Simulation.step, TextPos.BelowLeft);
		xGrids [0].isAxis = true;
	}

	public override void SetCompletely (bool first) {
		base.SetGrid ();


	}

	public override void ShowAxis() {
		base.ShowAxis ();

		for (int i = 0; i < gridNum; i++) {
			if (yGrids [i].isAxis)
				yGrids [i].Draw (true, true);
			else
				yGrids [i].Hide ();
		}

		xGrids [0].Draw (true, false);
	}

	public override void ShowGrid() {
		base.ShowGrid ();

		for (int i = 0; i < gridNum; i++) {
			yGrids [i].Draw (true, true);
		}

		xGrids [0].Draw (true, false);
	}

	public override void ShowCompletely () {
		base.ShowCompletely ();
	}

	public override Vector2 GraphToLocal (Vector2 v)
	{
		return base.RectGraphToLocal (v);
	}

	public override Vector2 LocalToGraph (Vector2 v) {
		return base.LocalToRectGraph (v);
	}

	public override string GetTypeText() {
		return "Time-" + DataBase.GetTags () [yType];
	}
}
