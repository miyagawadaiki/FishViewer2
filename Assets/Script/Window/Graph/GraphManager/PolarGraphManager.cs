using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarGraphManager : GraphManager {

	[SerializeField]
	private GameObject circleLineObj = null;
	[SerializeField]
	private GameObject angleLineObj = null;
	[SerializeField]
	private RectTransform polarView = null;

	private GridLineController[] rGrids;
	private AngleLineController alc;

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

		yGrids = new GridLineController[gridNum * 2];
		rGrids = new GridLineController[gridNum];
		for (int i = 0; i < gridNum; i++) {
			yGrids[i] = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
			yGrids[gridNum + i] = Instantiate (gridLineObj, view).GetComponent<GridLineController>();
			rGrids[i] = Instantiate (circleLineObj, polarView).GetComponent<GridLineController>();
		}

		alc = Instantiate (angleLineObj, polarView).GetComponent<AngleLineController> ();

		HideView ();
	}

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
				if ((int)pointType > 0 && i > 0)
					points [i].rotation = Quaternion.FromToRotation (Vector3.left, points [i - 1].position - points [i].position);
			}
		}
	}

	public override void SetGrid() {
		Vector2 minVec = LocalToRectGraph (recTra.rect.size / -2f), maxVec = LocalToRectGraph(recTra.rect.size / 2f);
		//float range = (maxVec - minVec).magnitude;
		float range = maxVec.x - minVec.x;
		for (int i = 0; i < gridDiv.Length; i++) {
			xGridValue = gridDiv [i];
			if (range / gridDiv [i] <= (float)gridNum - 1)
				break;
		}

		Vector2 localZero = RectGraphToLocal (new Vector2 ());
		int startIdx = 0;
		if (localZero.x < recTra.rect.width / -2f || localZero.x > recTra.rect.width / 2f ||
		    localZero.y < recTra.rect.height / -2f || localZero.y > recTra.rect.height / 2f) {
			float dist;
			if (localZero.x < 0 && localZero.y < 0)
				dist = LocalToRectGraph (recTra.rect.size / -2f).magnitude;
			else if (localZero.x > 0 && localZero.y > 0)
				dist = LocalToRectGraph (recTra.rect.size / 2f).magnitude;
			else if (localZero.x < 0 && localZero.y > 0)
				dist = LocalToRectGraph (new Vector2 (recTra.rect.width / -2f, recTra.rect.height / 2f)).magnitude;
			else
				dist = LocalToRectGraph (new Vector2 (recTra.rect.width / 2f, recTra.rect.height / -2f)).magnitude;
			startIdx = (int)(dist / xGridValue);
		}

		for (int i = 0; i < gridNum; i++) {
			float rad = RectGraphToLocal (new Vector2 ((i + 1 + startIdx) * xGridValue, 0f)).x - RectGraphToLocal (new Vector2 ()).x;
			float angle;
			if (localZero.Equals (new Vector2 ()))
				angle = 0f;
			else {
				angle = Mathf.Acos (localZero.x * -1f / localZero.magnitude) * (localZero.y * -1f > 0f ? 1f : -1f);
				Debug.Log ("angle = " + angle);
			}
			rGrids [i].SetCircle (localZero, rad, angle, (i + 1 + startIdx) * xGridValue);
		}

		alc.SetAngle (localZero);
	}

	public override void SetCompletely (bool first) {
		Vector2 minVec = LocalToRectGraph (recTra.rect.size / -2f), maxVec = LocalToRectGraph(recTra.rect.size / 2f);
		//float range = (maxVec - minVec).magnitude;
		float range = maxVec.x - minVec.x;
		for (int i = 0; i < gridDiv.Length; i++) {
			xGridValue = gridDiv [i];
			if (range / gridDiv [i] <= (float)gridNum * 2f - 1)
				break;
		}

		Vector2 localZero = RectGraphToLocal (new Vector2 ());
		int startIdx = 0;
		if (localZero.x < recTra.rect.width / -2f || localZero.x > recTra.rect.width / 2f ||
			localZero.y < recTra.rect.height / -2f || localZero.y > recTra.rect.height / 2f) {
			float dist;
			if (localZero.x < 0 && localZero.y < 0)
				dist = LocalToRectGraph (recTra.rect.size / -2f).magnitude;
			else if (localZero.x > 0 && localZero.y > 0)
				dist = LocalToRectGraph (recTra.rect.size / 2f).magnitude;
			else if (localZero.x < 0 && localZero.y > 0)
				dist = LocalToRectGraph (new Vector2 (recTra.rect.width / -2f, recTra.rect.height / 2f)).magnitude;
			else
				dist = LocalToRectGraph (new Vector2 (recTra.rect.width / 2f, recTra.rect.height / -2f)).magnitude;
			startIdx = (int)(dist / xGridValue);
		}

		for (int i = 0; i < gridNum; i++) {
			float rad = RectGraphToLocal (new Vector2 ((i + 1 + startIdx) * xGridValue, 0f)).x - RectGraphToLocal (new Vector2 ()).x;
			float angle;
			if (localZero.Equals (new Vector2 ()))
				angle = 0f;
			else {
				angle = Mathf.Acos (localZero.x * -1f / localZero.magnitude) * (localZero.y * -1f > 0f ? 1f : -1f);
				Debug.Log ("angle = " + angle);
			}

			if (first) {
				yGrids [i].Set (false, RectGraphToLocal (new Vector2(minVec.x, (i + 1 + startIdx) * xGridValue)), (i + 1 + startIdx) * xGridValue, TextPos.Left);
				yGrids [gridNum + i].Set (false, RectGraphToLocal (new Vector2(minVec.x, (i + 1 + startIdx) * xGridValue * -1f)), (i + 1 + startIdx) * xGridValue, TextPos.Left);
				rGrids [i].SetCircle (localZero, rad, angle, (i + 1 + startIdx) * xGridValue);
			} else {
				yGrids [i].Set (false, RectGraphToLocal (new Vector2(maxVec.x, (i + 1 + startIdx) * xGridValue)), (i + 1 + startIdx) * xGridValue, TextPos.Right);
				yGrids [gridNum + i].Set (false, RectGraphToLocal (new Vector2(maxVec.x, (i + 1 + startIdx) * xGridValue * -1f)), (i + 1 + startIdx) * xGridValue, TextPos.Right);
				rGrids [i].SetCircle (localZero, 0f, angle, (i + 1 + startIdx) * xGridValue);
			}
		}

		alc.SetAngle (localZero);

		if (first)
			xAxis.Set (true, view.rect.size / -2f, 0f, TextPos.Below);
		else
			xAxis.Set (true, view.rect.size / 2f, 0f, TextPos.Below);
	}

	public override void ShowAxis () {
		base.ShowAxis ();

		for (int i = 0; i < gridNum; i++) {
			yGrids [i].Hide ();
			yGrids [gridNum + i].Hide ();
			rGrids [i].Hide ();
		}

		alc.isAxis = true;
		alc.Draw (false, false);
	}

	public override void ShowGrid () {
		base.ShowGrid ();

		for (int i = 0; i < gridNum; i++) {
			yGrids [i].Hide ();
			yGrids [gridNum + i].Hide ();
			rGrids [i].Draw (true, true);
		}

		alc.isAxis = false;
		alc.Draw (false, false);
	}

	public override void ShowAxisCompletely () {
		base.ShowAxisCompletely ();

		for (int i = 0; i < gridNum; i++) {
			yGrids [i].DrawShort ();
			yGrids [gridNum + i].DrawShort ();
			rGrids [i].Draw (true, false);
		}

		alc.isAxis = true;
		alc.Draw (false, false);

		xAxis.Draw (true, false);
	}

	public override void ShowGridCompletely () {
		base.ShowGridCompletely ();

		for (int i = 0; i < gridNum; i++) {
			yGrids [i].DrawShort ();
			yGrids [gridNum + i].DrawShort ();
			rGrids [i].Draw (true, false);
		}

		alc.isAxis = false;
		alc.Draw (false, false);

		xAxis.Draw (true, false);
	}

	public override void HideView () {
		base.HideView ();

		foreach (Transform t in polarView)
			t.gameObject.GetComponent<GridLineController> ().Hide ();
	}

	public override Vector2 GraphToLocal(Vector2 v) {
		Vector2 vv = new Vector2 (Mathf.Cos (v.y), Mathf.Sin (v.y)) * v.x;
		return base.RectGraphToLocal (vv);
	}

	public override Vector2 LocalToGraph (Vector2 v) {
		Vector2 vv = base.LocalToRectGraph (v);
		float r = vv.magnitude;
		return new Vector2 (r, Mathf.Acos (vv.x / r));
	}
}
