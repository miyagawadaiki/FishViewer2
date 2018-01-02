using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphManager : MonoBehaviour {

	[SerializeField]
	protected Transform view = null;
	[SerializeField]
	protected Transform plot = null;
	[SerializeField]
	protected GameObject gridLineObj = null;
	[SerializeField]
	protected int defaultPointNum = 10;
	[SerializeField, Range(0,1)]
	protected float markerRate = 1f;
	[SerializeField]
	protected GameObject pointObj = null;
	[SerializeField]
	protected Color pointColor = Color.white;
	[SerializeField]
	protected int fish = 0;
	[SerializeField]
	protected int yType = 0;

	protected int pointNum, markerIdx;
	protected float yMax, yMin, xExp = 1f, yExp = 1f;
	protected GridLineController xAxis, yAxis;
	protected RectTransform[] points;

	protected RectTransform recTra;

	protected virtual void Awake() {
		recTra = this.GetComponent<RectTransform> ();
		pointNum = defaultPointNum;
		pointObj.GetComponent<Image> ().color = pointColor;
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

		xAxis = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
		yAxis = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
		xAxis.SetAxis ();
		yAxis.SetAxis ();

		points = new RectTransform[pointNum];
		markerIdx = (int)(pointNum * markerRate + 0.5f);
		for (int i = 0; i < pointNum; i++) {
			GameObject obj = Instantiate (pointObj, plot);
			points [i] = obj.GetComponent<RectTransform> ();
		}
	}

	public virtual void Plot(int step) {
		Debug.Log ("In GraphManager : " + step);
	}

	public virtual void ShowAxis() {
		
	}

	public virtual void ShowGrid() {

	}

	public virtual void ShowGridCompletely() {

	}

	public virtual void HideGrid() {

	}
}
