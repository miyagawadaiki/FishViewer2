using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectGraphManager : GraphManager {

	protected int xType;
	protected float xMax, xMin;

	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitGraph(int fish_, int xType_, int yType_) {
		fish = fish_;
		xType = xType_;
		yType = yType_;
	}

	public override void Plot(int step) {
		Debug.Log ("In RectGraphManager : " + step);
		for (int i = 0; i < pointNum; i++) {
			if (step - markerIdx + i < 0) {
				points [i].gameObject.SetActive (false);
			} else {
				points [i].gameObject.SetActive (true);
				points [i].localPosition = GraphToLocal (new Vector2 (DataBase.GetData (step - markerIdx + i, fish, xType), DataBase.GetData (step - markerIdx + i, fish, yType)));
			}
		}
	}

	public Vector2 GraphToLocal(Vector2 v) {
		Vector2 ret = new Vector2 (
			              xExp * recTra.rect.width / (xMax - xMin) * (v.x - (xMax + xMin) / 2f),
			              yExp * recTra.rect.height / (yMax - yMin) * (v.y - (yMax + yMin) / 2f));
		
		return ret;
	}
}
