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
	[SerializeField]
	protected GameObject pointObj = null;
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

	//public bool showAxis, showGrid;

	protected GraphContent gc;

	protected int markerIdx;
	protected float yMax, yMin, xExp = 1f, yExp = 1f;
	protected float xMax, xMin;
	//protected GridLineController xAxis, yAxis;
	protected RectTransform[] points;
	//protected Color pointColor;
	protected float markerRate = 0.5f;

	protected RectTransform recTra;

	protected float[] gridDiv = { 0.1f, 0.2f, 1f, 2f, 5f, 10f, 20f, 50f, 100f, 200f, 500f };
	protected float xGridValue = 0.1f, yGridValue = 0.1f;
	protected GridLineController[] xGrids, yGrids;
	protected int gridNum = 11, xPosSt, xNegSt, yPosSt, yNegSt, xAxisIdx, yAxisIdx;
	protected int[] xGridIdxs, yGridIdxs;




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

		/*
		xAxis = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
		yAxis = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
		xAxis.gameObject.name = "x-axis";
		yAxis.gameObject.name = "y-axis";
		xAxis.SetAxis ();
		yAxis.SetAxis ();
		*/

		foreach (Transform t in plot)
			Destroy (t.gameObject);
		
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

		xGrids = new GridLineController[gridNum];
		yGrids = new GridLineController[gridNum];
		for (int i = 0; i < gridNum; i++) {
			xGrids[i] = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
			yGrids[i] = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
		}
		xGridIdxs = new int[gridNum];
		yGridIdxs = new int[gridNum];
		HideView ();
	}

	public virtual void Set(string values) {
		char[] separator = { ',' };
		string[] tmp = values.Split (separator);

		fish = int.Parse (tmp [0]);
		xType = int.Parse (tmp [1]);
		yType = int.Parse (tmp [2]);
		pointNum = int.Parse (tmp [3]);
		//markerRate = float.Parse (tmp [4]);
		pointColorNum = int.Parse (tmp [4]);
		useColorGrad = int.Parse (tmp [5]) > 0 ? true : false;
		useSizeGrad = int.Parse (tmp [6]) > 0 ? true : false;
		plotSize = float.Parse (tmp [7]);
		useAutoSize = int.Parse (tmp [8]) > 0 ? true : false;

		//Init ();
	}

	public virtual void Plot(int step) {
		Debug.Log ("In GraphManager : " + step);
	}

	public virtual void Translate(Vector2 start, Vector2 end) {

	}

	public virtual void Expand(float expand) {
		xExp += expand;
		if (xExp < 1f)
			xExp = 1f;
				
		yExp += expand;
		if (yExp < 1f)
			yExp = 1f;
	}

	/*
	public virtual void ShowView() {
		if (!showAxis && !showGrid) {
			HideGrid ();
			return;
		}

		if (showAxis)
			SetAxis ();
		if (showGrid)
			SetGrid ();
	*/

	public virtual void SetGrid() {
		
	}

	public virtual void ShowAxis() {
		
	}

	public virtual void ShowGrid() {
		
	}

	public virtual void HideView() {
		foreach (Transform t in view) {
			t.gameObject.GetComponent<GridLineController> ().Hide ();
		}
	}
}

public enum GraphType {
	Rect,
	Time,
	Polar,
}