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

		points [markerIdx].sizeDelta *= 1.5f;
		xMin = (float)(Simulation.step - markerIdx);
		xMax = (float)(Simulation.step + (pointNum - markerIdx));
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
		//xType = int.Parse (tmp [1]);
		yType = int.Parse (tmp [2]);
		pointNum = int.Parse (tmp [3]);
		markerRate = float.Parse (tmp [4]);
		pointColorNum = int.Parse (tmp [5]);
		useColorGrad = int.Parse (tmp [6]) > 0 ? true : false;
		useSizeGrad = int.Parse (tmp [7]) > 0 ? true : false;
		plotSize = float.Parse (tmp [8]);
		useAutoSize = int.Parse (tmp [9]) > 0 ? true : false;

		Init ();
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

	public override void SetGrid() {
		/*
		//float xMax_ = xMax / xExp, xMin_ = xMin / xExp;
		float xMax_ = LocalToGraph(new Vector2(view.rect.width / 2f, 0f)).x, xMin_ = LocalToGraph(new Vector2(view.rect.width / -2f, 0f)).x;
		float xRange = (xMax_ - xMin_);
		for (int i = 0; i < gridDiv.Length; i++) {
			if (xRange / gridDiv [i] < (float)gridNum - 1)
				break;
			xGridValue = gridDiv [i];
		}
		*/

		//float yMax_ = yMax / yExp, yMin_ = yMin / yExp;
		float yMax_ = LocalToGraph(new Vector2(0f, view.rect.height / 2f)).y, yMin_ = LocalToGraph(new Vector2(0f, view.rect.height / -2f)).y;
		float yRange = (yMax_ - yMin_);
		for (int i = 0; i < gridDiv.Length; i++) {
			if (yRange / gridDiv [i] < (float)gridNum - 1)
				break;
			yGridValue = gridDiv [i];
		}

		/*
		int xAxisValue;
		if (xMax_ * xMin_ <= 0f)
			xAxisValue = 0;
		//else if (xMax_ * xMax_ < xMin_ * xMin_)
		else if (xMax_ < 0)
			//xAxisValue = (int)(xMax_ / xGridValue) + (xMax_ < 0 ? -1 : 0);
			xAxisValue = (int)(xMax_ / xGridValue) - 1;
		else
			xAxisValue = (int)(xMin_ / xGridValue) + 1;//+ (xMin_ > 0 ? 1 : 0);
		*/

		int yAxisValue;
		if (yMax_ * yMin_ <= 0f)
			yAxisValue = 0;
		else if (yMax_ * yMax_ < yMin_ * yMin_)
			yAxisValue = (int)(yMax_ / yGridValue) + (yMax_ < 0 ? -1 : 0);
		else
			yAxisValue = (int)(yMin_ / yGridValue) + (yMin_ > 0 ? 1 : 0);

		//Debug.Log ("AxisVlue = " + new Vector2 (xAxisValue, yAxisValue));


		//int xStart = (int)(xMin_ / xGridValue) + (xMin_ > 0 ? 1 : 0);
		int yStart = (int)(yMin_ / yGridValue) + (yMin_ > 0 ? 1 : 0);
		for (int i = 0; i < gridNum; i++) {
			/*
			xGrids [i].Set (true, GraphToLocal(new Vector2 ((xStart + i) * xGridValue, (float)yAxisValue * yGridValue)), (xStart + i) * xGridValue);
			if (xStart + i == xAxisValue)
				xGrids [i].isAxis = true;
			else
				xGrids [i].isAxis = false;
			*/

			yGrids [i].Set (false, GraphToLocal(new Vector2 (Simulation.step, (yStart + i) * yGridValue)), (yStart + i) * yGridValue);
			if (yStart + i == yAxisValue)
				yGrids [i].isAxis = true;
			else
				yGrids [i].isAxis = false;
		}

		xGrids [0].Set (true, GraphToLocal (new Vector2 (Simulation.step, (float)yAxisValue * yGridValue)), Simulation.step);
		xGrids [0].isAxis = true;
		for (int i = 1; i < gridNum; i++)
			xGrids [i].Hide ();
	}

	public override void ShowAxis() {
		for (int i = 0; i < gridNum; i++) {
			if (yGrids [i].isAxis)
				yGrids [i].Draw (true, true);
			else
				yGrids [i].Hide ();
		}

		xGrids [0].Draw (true, false);
	}

	public override void ShowGrid() {
		for (int i = 0; i < gridNum; i++) {
			yGrids [i].Draw (true, true);
		}

		xGrids [0].Draw (true, false);
	}

	/*
	public override void SetAxis() {
		base.SetAxis ();
		Vector2 origin = new Vector2 (Simulation.step, 0f);
		//if(yMin <= origin.y && yMax >= origin.y)
			xAxis.Draw (false, GraphToLocal (origin), 1.5f, 0f);
		//else
		//	xAxis.Hide ();
		//if(xMin <= origin.x && xMax >= origin.x)
			yAxis.DrawLineOnly (true, GraphToLocal (origin), 1.5f);
		//else
		//	yAxis.Hide ();
	}
	*/

	public Vector2 GraphToLocal(Vector2 v) {
		Vector2 ret = new Vector2 (
			xExp * recTra.rect.width / (xMax - xMin) * (v.x - (xMax + xMin) / 2f),
			yExp * recTra.rect.height / (yMax - yMin) * (v.y - (yMax + yMin) / 2f));

		return ret;
	}

	public Vector2 LocalToGraph(Vector2 v) {
		Vector2 ret = new Vector2 (
			(xMax - xMin) / (xExp * recTra.rect.width) * v.x,
			(yMax - yMin) / (yExp * recTra.rect.height) * v.y);
		ret += new Vector2 ((xMax + xMin) / 2f, (yMax + yMin) / 2f);

		return ret;
	}
}
