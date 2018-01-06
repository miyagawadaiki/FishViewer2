using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectGraphManager : GraphManager {

	protected override void Awake() {
		base.Awake ();
		//type = GraphType.Rect;
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
		 */

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
		*/
}

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
			}
		}
	}

	public override void Translate(Vector2 start, Vector2 end) {
		//Debug.Log ("vec = " + vec);
		Vector2 tmp = LocalToGraph (end) - LocalToGraph(start);
		//Debug.Log ("tmp = " + tmp);
		xMax -= tmp.x;
		xMin -= tmp.x;
		yMax -= tmp.y;
		yMin -= tmp.y;

		ShowAxis ();
	}

	public override void ShowAxis() {
		base.ShowAxis ();
		xAxis.Draw (false, GraphToLocal (new Vector2 (0f, 0f)), 1.5f, 0f);
		yAxis.DrawLineOnly (true, GraphToLocal (new Vector2 (0f, 0f)), 1.5f);
	}

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
