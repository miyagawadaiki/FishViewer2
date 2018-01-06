using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarGraphManager : GraphManager {

	protected override void Awake() {
		base.Awake ();
		graphType = GraphType.Polar;
	}

	// Use this for initialization
	protected override void Start () {
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		
	}

	public override void Init() {
		Debug.Log ("<color=green>PolarGraphManager.Init()</color>");
		base.Init ();

		xMax = DataBase.GetMax (xType);
		xMin = -xMax;
		yMax = xMax;
		yMin = -xMax;

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
				points [i].localPosition = GraphToLocal (new Vector2 (DataBase.GetData (step - markerIdx + i, fish, xType), DataBase.GetData (step - markerIdx + i, fish, yType)));
				if (!recTra.rect.Contains (points [i].localPosition))
					points [i].gameObject.SetActive (false);
			}
		}
	}

	public override void Translate(Vector2 start, Vector2 end) {
		Vector2 tmp = LocalToRectGraph (end) - LocalToRectGraph(start);
		//Debug.Log ("tmp = " + tmp);
		xMax -= tmp.x;
		xMin -= tmp.x;
		yMax -= tmp.y;
		yMin -= tmp.y;

		ShowAxis ();
	}

	public override void ShowAxis() {
		base.ShowAxis ();
		Vector2 origin = new Vector2 ();
		if(yMin <= origin.y && yMax >= origin.y)
			xAxis.Draw (false, GraphToLocal (origin), 1.5f, 0f);
		else
			xAxis.Hide ();
		if(xMin <= origin.x && xMax >= origin.x)
			yAxis.DrawLineOnly (true, GraphToLocal (origin), 1.5f);
		else
			yAxis.Hide ();
	}

	public Vector2 GraphToLocal(Vector2 v) {
		Vector2 vv = new Vector2 (Mathf.Cos (v.y), Mathf.Sin (v.y)) * v.x;
		Vector2 ret = new Vector2 (
			xExp * recTra.rect.width / (xMax - xMin) * (vv.x - (xMax + xMin) / 2f),
			yExp * recTra.rect.height / (yMax - yMin) * (vv.y - (yMax + yMin) / 2f));

		return ret;
	}

	public Vector2 LocalToRectGraph(Vector2 v) {
		Vector2 ret = new Vector2 (
			(xMax - xMin) / (xExp * recTra.rect.width) * v.x,
			(yMax - yMin) / (yExp * recTra.rect.height) * v.y);
		ret += new Vector2 ((xMax + xMin) / 2f, (yMax + yMin) / 2f);

		return ret;
	}
}
