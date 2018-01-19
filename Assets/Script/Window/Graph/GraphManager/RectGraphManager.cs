using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectGraphManager : GraphManager {

	protected override void Awake() {
		base.Awake ();
		graphType = GraphType.Rect;
	}

	// Use this for initialization
	protected override void Start () {
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		
	}

	public override void Init() {
		Debug.Log ("<color=green>RectGraphManager.Init()</color>");
		base.Init ();

		xMax = DataBase.GetMax (xType);
		xMin = DataBase.GetMin (xType);

		xGrids = new GridLineController[gridNum];
		yGrids = new GridLineController[gridNum];
		for (int i = 0; i < gridNum; i++) {
			xGrids[i] = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
			yGrids[i] = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
		}

		HideView ();
	}
		
	public override void Plot(int step) {
		//Debug.Log ("In RectGraphManager : " + step);
		for (int i = 0; i < pointNum; i++) {
			if (step - markerIdx + i < 0) {
				points [i].gameObject.SetActive (false);
			} else {
				points [i].gameObject.SetActive (true);
				points [i].localPosition = RectGraphToLocal (new Vector2 (DataBase.GetData (step - markerIdx + i, fish, xType), DataBase.GetData (step - markerIdx + i, fish, yType)));
				if (!recTra.rect.Contains (points [i].localPosition))
					points [i].gameObject.SetActive (false);
			}
		}
	}

	public override void SetGrid() {
		base.SetGrid ();

		float xMax_ = LocalToRectGraph(new Vector2(view.rect.width / 2f, 0f)).x, xMin_ = LocalToGraph(new Vector2(view.rect.width / -2f, 0f)).x;
		float yMax_ = LocalToRectGraph(new Vector2(0f, view.rect.height / 2f)).y, yMin_ = LocalToGraph(new Vector2(0f, view.rect.height / -2f)).y;

		int xStart = (int)(xMin_ / xGridValue) + (xMin_ > 0 ? 1 : 0), yStart = (int)(yMin_ / yGridValue) + (yMin_ > 0 ? 1 : 0);
		for (int i = 0; i < gridNum; i++) {
			xGrids [i].Set (true, RectGraphToLocal(new Vector2 ((xStart + i) * xGridValue, (float)yAxisValue * yGridValue)), (xStart + i) * xGridValue, TextPos.BelowLeft);
			//if (xStart + i == xAxisValue)
			//	xGrids [i].isAxis = true;
			//else
				xGrids [i].isAxis = false;

			yGrids [i].Set (false, RectGraphToLocal(new Vector2 ((float)xAxisValue * xGridValue, (yStart + i) * yGridValue)), (yStart + i) * yGridValue, TextPos.BelowLeft);
			//if (yStart + i == yAxisValue)
			//	yGrids [i].isAxis = true;
			//else
				yGrids [i].isAxis = false;
		}

		xGrids [xAxisValue - xStart].isAxis = true;
		yGrids [yAxisValue - yStart].isAxis = true;
	}

	public override void SetCompletely(bool first) {
		base.SetGrid ();

		float xMax_ = LocalToRectGraph(new Vector2(view.rect.width / 2f, 0f)).x, xMin_ = LocalToGraph(new Vector2(view.rect.width / -2f, 0f)).x;
		float yMax_ = LocalToRectGraph(new Vector2(0f, view.rect.height / 2f)).y, yMin_ = LocalToGraph(new Vector2(0f, view.rect.height / -2f)).y;

		int xStart = (int)(xMin_ / xGridValue) + (xMin_ > 0 ? 1 : 0), yStart = (int)(yMin_ / yGridValue) + (yMin_ > 0 ? 1 : 0);
		for (int i = 0; i < gridNum; i++) {
			if(first)
				xGrids [i].Set (true, RectGraphToLocal(new Vector2 ((xStart + i) * xGridValue, yMin_)), (xStart + i) * xGridValue, TextPos.Below);
			else
				xGrids [i].Set (true, RectGraphToLocal(new Vector2 ((xStart + i) * xGridValue, yMax_)), (xStart + i) * xGridValue, TextPos.Above);
			xGrids [i].isAxis = false;

			if(first)
				yGrids [i].Set (false, RectGraphToLocal(new Vector2 (xMin_, (yStart + i) * yGridValue)), (yStart + i) * yGridValue, TextPos.Left);
			else
				yGrids [i].Set (false, RectGraphToLocal(new Vector2 (xMax_, (yStart + i) * yGridValue)), (yStart + i) * yGridValue, TextPos.Right);
			yGrids [i].isAxis = false;
		}

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
			if (xGrids [i].isAxis)
				xGrids [i].Draw (true, true);
			else
				xGrids [i].Hide ();
			
			if (yGrids [i].isAxis)
				yGrids [i].Draw (true, false);
			else
				yGrids [i].Hide ();
		}
	}

	public override void ShowGrid() {
		base.ShowGrid ();

		for (int i = 0; i < gridNum; i++) {
			xGrids [i].Draw (true, true);
			if (yGrids [i].isAxis)
				yGrids [i].Draw (true, false);
			else
				yGrids [i].Draw (true, true);
		}
	}

	public override void ShowAxisCompletely () {
		base.ShowAxisCompletely ();

		for (int i = 0; i < gridNum; i++) {
			xGrids [i].DrawShort ();
			yGrids [i].DrawShort ();
		}

		xAxis.Draw (true, true);
		yAxis.Draw (true, true);
	}

	public override void ShowGridCompletely () {
		base.ShowGridCompletely ();

		for (int i = 0; i < gridNum; i++) {
			xGrids [i].Draw (true, true);
			yGrids [i].Draw (true, true);
		}

		xAxis.Draw (true, true);
		yAxis.Draw (true, true);
	}

	public override Vector2 GraphToLocal (Vector2 v) {
		return base.RectGraphToLocal (v);
	}

	public override Vector2 LocalToGraph (Vector2 v) {
		return base.LocalToRectGraph (v);
	}
}
