﻿using System.Collections;
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

		//ShowAxis ();
	}

	/*
	public override void Set(string values) {
		char[] separator = { ',' };
		string[] tmp = values.Split (separator);

		/* 
		 * fish(0)
		 * xType(1)
		 * yType(2)
		 * pointNum(3)
		 * markerRate(4)
		 * pointColor(5)
		 * useColorGrad(6)
		 * useSizeGrad(7)
		 * sizeValue(8)
		 * useAutoSize(9)
		 

		fish = int.Parse (tmp [0]);
		xType = int.Parse (tmp [1]);
		yType = int.Parse (tmp [2]);
		pointNum = int.Parse (tmp [3]);
		markerRate = float.Parse (tmp [4]);
		pointColorNum = int.Parse (tmp [5]);
		useColorGrad = int.Parse (tmp [6]) > 0 ? true : false;
		useSizeGrad = int.Parse (tmp [7]) > 0 ? true : false;
		plotSize = float.Parse (tmp [8]);
		useAutoSize = int.Parse (tmp [9]) > 0 ? true : false;

		Init ();

		/*
		gc.mFish = int.Parse (tmp [0]);
		gc.mXType = int.Parse (tmp [1]);
		gc.mYType = int.Parse (tmp [2]);
		gc.mPointNum = int.Parse (tmp [3]);
		gc.mMarkerRate = float.Parse (tmp [4]);
		gc.mPointColorNum = int.Parse (tmp [5]);
		gc.mUseColorGrad = int.Parse (tmp [6]) > 0 ? true : false;
		gc.mUseSizeGrad = int.Parse (tmp [7]) > 0 ? true : false;
		gc.mPlotSize = float.Parse (tmp [8]);
		gc.mUseAutoSize = int.Parse (tmp [9]) > 0 ? true : false;

	}
	*/

	/*
	public void SetGraph(int fish_, int xType_, int yType_) {
		fish = fish_;
		xType = xType_;
		yType = yType_;
	}
	*/

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

	/*
	public override void Translate(Vector2 start, Vector2 end) {
		//Debug.Log ("vec = " + vec);


		//SetAxis ();
	}
	*/

	/*
	public override void SetAxis() {
		base.SetAxis ();
		Vector2 origin = new Vector2 ();
		//if (xMin <= origin.x && xMax >= origin.x)
			xAxis.DrawLineOnly (true, GraphToLocal (origin), 1.5f);
		//else
		//	xAxis.Hide ();
		//if (yMin <= origin.y && yMax >= origin.y)
			yAxis.Draw (false, GraphToLocal (origin), 1.5f, 0f);
		//else
		//	yAxis.Hide ();
	}
	*/

	public override void SetGrid() {
		base.SetGrid ();

		float xMax_ = LocalToRectGraph(new Vector2(view.rect.width / 2f, 0f)).x, xMin_ = LocalToGraph(new Vector2(view.rect.width / -2f, 0f)).x;
		float yMax_ = LocalToRectGraph(new Vector2(0f, view.rect.height / 2f)).y, yMin_ = LocalToGraph(new Vector2(0f, view.rect.height / -2f)).y;

		int xStart = (int)(xMin_ / xGridValue) + (xMin_ > 0 ? 1 : 0), yStart = (int)(yMin_ / yGridValue) + (yMin_ > 0 ? 1 : 0);
		for (int i = 0; i < gridNum; i++) {
			xGrids [i].Set (true, RectGraphToLocal(new Vector2 ((xStart + i) * xGridValue, (float)yAxisValue * yGridValue)), (xStart + i) * xGridValue);
			if (xStart + i == xAxisValue)
				xGrids [i].isAxis = true;
			else
				xGrids [i].isAxis = false;

			yGrids [i].Set (false, RectGraphToLocal(new Vector2 ((float)xAxisValue * xGridValue, (yStart + i) * yGridValue)), (yStart + i) * yGridValue);
			if (yStart + i == yAxisValue)
				yGrids [i].isAxis = true;
			else
				yGrids [i].isAxis = false;
		}
	}

	public override void ShowAxis() {
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
		/*
		Vector2 v = new Vector2 (xGridIdxs [xAxisIdx] * xGridValue, yGridIdxs [yAxisIdx] * yGridValue);
		xGrids [xAxisIdx].Draw (true, GraphToLocal (v), 1.5f, v.x);
		yGrids [yAxisIdx].DrawLineOnly (false, GraphToLocal (v), 1.5f);
		*/
	}

	public override void ShowGrid() {
		for (int i = 0; i < gridNum; i++) {
			xGrids [i].Draw (true, true);
			if (yGrids [i].isAxis)
				yGrids [i].Draw (true, false);
			else
				yGrids [i].Draw (true, true);
		}
		/*
		Vector2 v = new Vector2 (xGridIdxs [xAxisIdx] * xGridValue, yGridIdxs [yAxisIdx] * yGridValue);
		for(int i=0;i<gridNum;i++) {
			if (xGridIdxs [i] < 10000)
				xGrids [i].Draw (true, GraphToLocal (new Vector2 (xGridIdxs [i] * xGridValue, v.y)), 1f, xGridIdxs [i] * xGridValue);
			if (yGridIdxs [i] < 10000)
				yGrids [i].Draw (false, GraphToLocal (new Vector2 (v.x, yGridIdxs [i] * yGridValue)), 1f, yGridIdxs [i] * yGridValue);
		}

		ShowAxis ();
		*/
	}

	/*
	public override void SetGrid() {
		base.SetGrid ();
		//SetAxis ();




		float xMax_ = xMax / xExp, xMin_ = xMin / xExp;
		int i;
		for (i = 0; i < gridNum; i++) {
			Vector2 v = new Vector2 ((i + xPosSt + 1) * xGridValue, 0f);
			if (xMin <= v.x && xMax >= v.x)
				xGrids [i].Draw (true, GraphToLocal (v), 1f, v.x);
			else
				xGrids [i].Hide ();
		}
		for (; i < gridNum * 2; i++) {
			Vector2 v = new Vector2 ((i + xNegSt - gridNum + 1) * xGridValue * -1f, 0f);
			if (xMin <= v.x && xMax >= v.x)
				xGrids [i].Draw (true, GraphToLocal (v), 1f, v.x);
			else
				xGrids [i].Hide ();
		}

		for (i = 0; i < gridNum; i++) {
			Vector2 v = new Vector2 (0f, (i + yPosSt + 1) * yGridValue);
			if (yMin <= v.y || yMax >= v.y)
				yGrids [i].Draw (false, GraphToLocal (v), 1f, v.y);
			else
				yGrids [i].Hide ();
		}
		for (; i < gridNum * 2; i++) {
			Vector2 v = new Vector2 (0f, (i + yNegSt - gridNum + 1) * yGridValue * -1f);
			if (yMin <= v.y || yMax >= v.y)
				yGrids [i].Draw (false, GraphToLocal (v), 1f, v.y);
			else
				yGrids [i].Hide ();
		}

	}
	*/

	public override Vector2 GraphToLocal (Vector2 v) {
		return base.RectGraphToLocal (v);
	}

	public override Vector2 LocalToGraph (Vector2 v) {
		return base.LocalToRectGraph (v);
	}

	/*
	public override Vector2 RectGraphToLocal(Vector2 v) {
		
	}
	*/

	/*
	public override Vector2 LocalToRectGraph(Vector2 v) {
		
	}
	*/
}
