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

	protected int fish, yType, pointNum, markerIdx;
	protected float yMax, yMin, xExp = 1f, yExp = 1f;
	protected RectTransform[] points;

	protected RectTransform recTra;

	void Awake() {
		recTra = this.GetComponent<RectTransform> ();
		pointNum = defaultPointNum;
		pointObj.GetComponent<Image> ().color = pointColor;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void Init() {
		// yType, pointNumが決まっている
		yMax = DataBase.GetMax (yType);
		yMin = DataBase.GetMin (yType);

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
