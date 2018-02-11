using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphManager : MonoBehaviour {

	public GraphType graphType;

	[SerializeField]
	protected RectTransform view = null;
	[SerializeField]
	protected RectTransform plot = null;
	[SerializeField]
	protected GameObject gridLineObj = null;
	//[SerializeField]
	public int pointNum = 50;
	//[SerializeField, Range(0,1)]
	//public float markerRate = 1f;
	//[SerializeField]
	protected GameObject pointObj;// = null;
	//[SerializeField]
	//protected Color pointColor = Color.white;
	public int pointColorNum = 0;
	//[SerializeField]
	public bool useColorGrad = true;
	//[SerializeField]
	public bool useSizeGrad = false;
	//[SerializeField]
	public float plotSize = 10f;
	//[SerializeField]
	public bool useAutoSize = false;
	//[SerializeField]
	public int fish = 0;
	//[SerializeField]
	public int xType = 0;

	public int yType = 1;

	public int dType = 0;

	public PointType pointType = PointType.Circle;

	//public bool showAxis, showGrid;

	protected GraphContent gc;

	protected int markerIdx;
	protected float yMax, yMin, xExp = 1f, yExp = 1f;
	protected float xMax, xMin;
	protected GridLineController xAxis, yAxis;
	protected RectTransform[] points;
	//protected Color pointColor;
	protected float markerRate = 0.5f;

	protected RectTransform recTra;

	protected float[] gridDiv = { 0.1f, 0.2f, 0.5f, 1f, 2f, 5f, 10f, 20f, 50f, 100f, 200f, 500f };
	protected float xGridValue = 0.1f, yGridValue = 0.1f;
	protected GridLineController[] xGrids, yGrids;
	protected int gridNum = 11, xAxisValue, yAxisValue;//xPosSt, xNegSt, yPosSt, yNegSt, xAxisIdx, yAxisIdx;
	//protected int[] xGridIdxs, yGridIdxs;




	protected virtual void Awake() {
		recTra = this.GetComponent<RectTransform> ();
		//pointNum = defaultPointNum;
		//pointObj.GetComponent<Image> ().color = pointColor;
		gc = this.GetComponentInParent<GraphContent>();
	}

	// Use this for initialization
	protected virtual void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

	public virtual void Init() {
		Debug.Log ("<color=green>GraphManager.Init() : " + pointNum + "</color>");

		// yType, pointNumが決まっている
		yMax = DataBase.GetMax (yType);
		yMin = DataBase.GetMin (yType);

		foreach (Transform t in view)
			Destroy (t.gameObject);

		xExp = 1f;
		yExp = 1f;

		foreach (Transform t in plot)
			Destroy (t.gameObject);

		pointObj = Resources.Load (System.Enum.GetName (typeof(PointType), pointType) + "Point") as GameObject;
		
		points = new RectTransform[pointNum];
		markerIdx = (int)((pointNum - 1) * markerRate + 0.5f);
		//pointColor = ProjectData.ColorList.colors[pointColorNum];
		for (int i = 0; i < pointNum; i++) {
			GameObject obj = Instantiate (pointObj, plot);
			points [i] = obj.GetComponent<RectTransform> ();
			points [i].GetComponent<Image> ().color = ProjectData.ColorList.colors[pointColorNum];
			points [i].gameObject.SetActive (false);
		}

		if (useColorGrad) {
			Gradient g = new Gradient ();
			GradientColorKey[] gck = new GradientColorKey[2];
			gck [0].color = Color.white;	gck [0].time = 0f;
			gck [1].color = ProjectData.ColorList.colors[pointColorNum];		gck [1].time = 1f;
			GradientAlphaKey[] gak = new GradientAlphaKey[2];
			gak [0].alpha = 0f;		gak [0].time = 0f;
			gak [1].alpha = 0xfff;	gak [1].time = 1f;
			g.SetKeys (gck, gak);

			for (int i = 0; i < pointNum; i++) {
				points [i].GetComponent<Image> ().color = g.Evaluate (0.1f + (i / (float)(pointNum - 1) * 0.9f));
			}
		}
			
		if (useAutoSize) {
			float size = recTra.rect.width / (pointNum * 1.1f);
			for (int i = 0; i < pointNum; i++) {
				points [i].GetComponent<RectTransform> ().sizeDelta = new Vector2(1f, 1f) * size;
			}
		} else if (useSizeGrad) {
			for (int i = 0; i < pointNum; i++) {
				points [i].GetComponent<RectTransform> ().sizeDelta = new Vector2(1f, 1f) * ((0.4f + i / (float)(pointNum - 1) * 0.6f) * plotSize);
			}
		} else {
			for (int i = 0; i < pointNum; i++) {
				points [i].GetComponent<RectTransform> ().sizeDelta = new Vector2(1f, 1f) * plotSize;
			}
		}

		xAxis = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
		yAxis = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
		xAxis.isAxis = true;
		yAxis.isAxis = true;
	}

	public virtual void Set(string values) {
		char[] separator = { ':', ',' };
		string[] tmp = values.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		fish = int.Parse (tmp [0]);
		xType = int.Parse (tmp [1]);
		yType = int.Parse (tmp [2]);
		dType = int.Parse (tmp [3]);
		pointType = (PointType)int.Parse (tmp [4]);
		pointNum = int.Parse (tmp [5]);
		pointColorNum = int.Parse (tmp [6]);
		useColorGrad = int.Parse (tmp [7]) > 0 ? true : false;
		useSizeGrad = int.Parse (tmp [8]) > 0 ? true : false;
		plotSize = float.Parse (tmp [9]);
		useAutoSize = int.Parse (tmp [10]) > 0 ? true : false;
	}

	public virtual void Plot(int step) {
		Debug.Log ("In GraphManager : " + step);
	}

	public virtual void Translate(Vector2 start, Vector2 end) {
		Vector2 tmp = LocalToRectGraph (end) - LocalToRectGraph(start);
		//Debug.Log ("tmp = " + tmp);
		xMax -= tmp.x;
		xMin -= tmp.x;
		yMax -= tmp.y;
		yMin -= tmp.y;
	}

	public virtual void Expand(float expand) {
		xExp += expand;
		if (xExp < 1f)
			xExp = 1f;
				
		yExp += expand;
		if (yExp < 1f)
			yExp = 1f;
	}

	public virtual void SetGrid() {
		// 拡大も考慮したxの範囲を求める
		float xMax_ = LocalToRectGraph(new Vector2(view.rect.width / 2f, 0f)).x, xMin_ = LocalToGraph(new Vector2(view.rect.width / -2f, 0f)).x;
		float xRange = (xMax_ - xMin_);

		// gridNumを超えないように最大のグリッド幅を決める
		for (int i = 0; i < gridDiv.Length; i++) {
			xGridValue = gridDiv [i];
			if (xRange / gridDiv [i] <= (float)gridNum - 1)
				break;
		}

		// yについても同様に
		//float yMax_ = yMax / yExp, yMin_ = yMin / yExp;
		float yMax_ = LocalToRectGraph(new Vector2(0f, view.rect.height / 2f)).y, yMin_ = LocalToGraph(new Vector2(0f, view.rect.height / -2f)).y;
		float yRange = (yMax_ - yMin_);

		for (int i = 0; i < gridDiv.Length; i++) {
			yGridValue = gridDiv [i];
			if (yRange / gridDiv [i] <= (float)gridNum - 1)
				break;
		}

		//Debug.Log ("GridValue = " + new Vector2 (xGridValue, yGridValue));

		// xのレンジが0をまたいでいたら基準グリッド線は0を通るとする
		if (xMax_ * xMin_ <= 0f)
			xAxisValue = 0;
		//else if (xMax_ * xMax_ < xMin_ * xMin_)
		// レンジが負の領域にあればxMax_に最も近いグリッド線を基準とする
		// この値の次元はグリッド幅を決めた上での原点から何番目かの値となる
		else if (xMax_ < 0)
			//xAxisValue = (int)(xMax_ / xGridValue) + (xMax_ < 0 ? -1 : 0);
			xAxisValue = (int)(xMax_ / xGridValue) - 1;
		else
			xAxisValue = (int)(xMin_ / xGridValue) + 1;//+ (xMin_ > 0 ? 1 : 0);

		// yについても同様にAxisValueを決める
		//int yAxisValue;
		if (yMax_ * yMin_ <= 0f)
			yAxisValue = 0;
		else if (yMax_ < 0)
			yAxisValue = (int)(yMax_ / yGridValue) - 1;
		else
			yAxisValue = (int)(yMin_ / yGridValue) + 1;
		
		/*
		else if (yMax_ * yMax_ < yMin_ * yMin_)
			yAxisValue = (int)(yMax_ / yGridValue) + (yMax_ < 0 ? -1 : 0);
		else
			yAxisValue = (int)(yMin_ / yGridValue) + (yMin_ > 0 ? 1 : 0);
		*/

	}

	public virtual void SetCompletely(bool first) {
		
	}

	public virtual void ShowAxis() {
		xAxis.Hide ();
		yAxis.Hide ();
	}

	public virtual void ShowGrid() {
		xAxis.Hide ();
		yAxis.Hide ();
	}

	public virtual void ShowAxisCompletely() {

	}

	public virtual void ShowGridCompletely() {

	}

	public virtual void HideView() {
		foreach (Transform t in view) {
			t.gameObject.GetComponent<GridLineController> ().Hide ();
		}
	}

	public virtual Vector2 RectGraphToLocal(Vector2 v) {
		Vector2 ret = new Vector2 (
			xExp * recTra.rect.width / (xMax - xMin) * (v.x - (xMax + xMin) / 2f),
			yExp * recTra.rect.height / (yMax - yMin) * (v.y - (yMax + yMin) / 2f));

		return ret;
	}

	public virtual Vector2 GraphToLocal(Vector2 v) {
		return new Vector2 ();
	}

	public virtual Vector2 LocalToRectGraph(Vector2 v) {
		Vector2 ret = new Vector2 (
			(xMax - xMin) / (xExp * recTra.rect.width) * v.x,
			(yMax - yMin) / (yExp * recTra.rect.height) * v.y);
		ret += new Vector2 ((xMax + xMin) / 2f, (yMax + yMin) / 2f);

		return ret;
	}

	public virtual Vector2 LocalToGraph(Vector2 v) {
		return new Vector2 ();
	}

	public string GetStepText () {
		int st = Simulation.step - markerIdx;
		st = st < 0 ? 0 : st;
		st = st >= DataBase.step ? DataBase.step - 1 : st;
		int ed = Simulation.step - markerIdx + pointNum - 1;
		ed = ed < 0 ? 0 : ed;
		ed = ed >= DataBase.step ? DataBase.step - 1 : ed;
		string s = st + "-" + ed;
		return s;
	}

	public string GetFishText() {
		return "fish" + (fish + 1);
	}

	public virtual string GetTypeText() {
		return DataBase.GetTags () [xType] + "-" + DataBase.GetTags () [yType];
	}

	public string GetLabelText () {
		return fish + ", " + DataBase.GetShortTags () [xType] + "-" + DataBase.GetShortTags () [yType];
	}

	public string GetParameterText() {
		string s = 
			(int)graphType + " :" +
			fish + "," +
			xType + "," +
			yType + "," +
			dType + ",:" +
			(int)pointType + "," + 
			pointNum + "," +
			pointColorNum + "," +
			(useColorGrad ? 1 : 0) + "," +
			(useSizeGrad ? 1 : 0) + "," +
			plotSize + "," +
			(useAutoSize ? 1 : 0) + ",:";

		return s;
	}
}

public enum GraphType {
	Rect,
	Time,
	Polar,
}

public enum PointType {
	Circle,
	Triangle,
}