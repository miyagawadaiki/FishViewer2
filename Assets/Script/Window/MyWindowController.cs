using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyWindowController : MonoBehaviour {

	public MyWindowContent content = null;
	public GameObject menuArea = null;
	[SerializeField]
	private Image menuImg = null;
	[SerializeField]
	private Image frameImg = null;
	[SerializeField]
	private Color normalColor = Color.white;
	[SerializeField]
	private Color selectedColor = Color.white;
	[SerializeField]
	private RectTransform menuRT = null;
	[SerializeField]
	private RectTransform frameRT = null;

	private Vector2 canvas;
	//private Rect rect;//, parRect;
	//private Vector2 expandDir;

	[System.NonSerialized]
	public MyWindowManager mwm;
	//[System.NonSerialized]
	public bool isSelected = false;
	[System.NonSerialized]
	public bool isDestroyed = false;
	[System.NonSerialized]
	public bool canMove = true;
	[System.NonSerialized]
	public bool canExpand = true;
	[System.NonSerialized]
	public RectTransform recTra;
	[System.NonSerialized]
	public RectTransform parRecTra;

	void Awake() {
		mwm = this.GetComponentInParent<MyWindowManager> ();
		recTra = this.GetComponent<RectTransform> ();
		parRecTra = this.transform.parent.gameObject.GetComponent<RectTransform> ();
		//rect = recTra.rect;
		//parRect = parRecTra.rect;
		menuImg.color = new Color (selectedColor.r, selectedColor.g, selectedColor.b, 0.4f);
		SetNormalMode ();
	}

	// Use this for initialization
	void Start () {
		content = this.GetComponentInChildren<MyWindowContent> ();
		content.transform.SetAsFirstSibling ();

		canvas = (Vector2)GameObject.Find ("Canvas").transform.localScale;

		Instantiate (Resources.Load ("Menubar/Destroy") as GameObject, menuArea.transform);

		//Button destroy = this.GetComponentInChildren<Button> ();
		//destroy.onClick.AddListener (() => this.GetComponentInParent<MyWindowManager> ().CallRemove ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSelectMode() {
		if (isSelected)
			return;
		isSelected = true;
		//menuImg.color = new Color (selectedColor.r, selectedColor.g, selectedColor.b, 0.5f);
		frameImg.color = selectedColor;
	}

	public void SetNormalMode() {
		if (!isSelected)
			return;
		isSelected = false;
		//menuImg.color = new Color (normalColor.r, normalColor.g, normalColor.b, 0.5f);
		frameImg.color = normalColor;
		HideMenu ();
	}

	public void AppearMenu() {
		menuImg.gameObject.SetActive (true);
	}

	public void HideMenu() {
		menuImg.gameObject.SetActive (false);
	}

	public int GetSiblingIndex() {
		return recTra.GetSiblingIndex ();
	}
		
	public void SetAsLastSibling() {
		recTra.SetAsLastSibling();
	}

	public void SetSize(Vector2 size) {
		//Vector2 hoge = this.transform.parent.gameObject.GetComponent<RectTransform> ().rect.size;
		//recTra.sizeDelta = new Vector2(size.x - hoge.x, size.y - hoge.y);
		recTra.sizeDelta = size;
	}

	public void MoveTo (Vector2 localPos) {
		recTra.localPosition = localPos;
		TranslateIntoWindowManager ();
	}
	/*
	public void MoveTo(Vector2 ratePos) {
		//Debug.Log ("screen=" + new Vector2 (Screen.width, Screen.height) + " rect.size=" + rect.size);
		Vector2 tmp = (new Vector2 (Screen.width, Screen.height) - rect.size) / 2f;
		//Debug.Log ("tmp=" + tmp + " ratePos=" + ratePos);
		recTra.localPosition = new Vector2 (tmp.x * ratePos.x, tmp.y * ratePos.y);
		//Vector2 v = localPos - (Vector2)recTra.localPosition;
		//recTra.localPosition += (Vector3)v;
	}
	*/

	public void Translate(Vector2 vec) {
		if (!canMove)
			return;
		recTra.position += (Vector3)vec;

		TranslateIntoWindowManager ();
	}

	public void TranslateIntoWindowManager() {
		Vector2 v = (Vector2)recTra.localPosition;
		Vector2 size = recTra.rect.size;
		Vector2 tmp = new Vector2 ();
		if ((v + size / 2f).x > parRecTra.rect.width / 2f)
			tmp += new Vector2 (parRecTra.rect.width / 2f - (v + size / 2f).x, 0f);
		if ((v + size / 2f).y > parRecTra.rect.height / 2f)
			tmp += new Vector2 (0f, parRecTra.rect.height / 2f - (v + size / 2f).y);

		if ((v - size / 2f).x < -parRecTra.rect.width / 2f)
			tmp += new Vector2 (-parRecTra.rect.width / 2f - (v - size / 2f).x, 0f);
		if ((v - size / 2f).y < -parRecTra.rect.height / 2f)
			tmp += new Vector2 (0f, -parRecTra.rect.height / 2f - (v - size / 2f).y);

		recTra.position += (Vector3)tmp;
	}

	public void Expand(Vector2 vec, Vector2 expandDir) {
		if (!canExpand)
			return;
		Vector2 foo = new Vector2 (vec.x * expandDir.x, vec.y * expandDir.y);
		Vector2 hoge = recTra.sizeDelta;
		Vector2 tmp = recTra.sizeDelta + foo / canvas.x;
		//Debug.Log ("tmp=" + (tmp + recTra.rect.size * canvas.x));
		recTra.sizeDelta = tmp;
		if (recTra.rect.width < menuRT.rect.width + 20f || recTra.rect.height < menuRT.rect.width + 20f) {
			recTra.sizeDelta = hoge;
		} else {
			if (expandDir.x == 0f)
				vec.x = 0f;
			if (expandDir.y == 0f)
				vec.y = 0f;
			Translate (vec / 2f);
		}


		TranslateIntoWindowManager ();
	}

	public void SquareExpand(Vector2 vec, Vector2 expandDir) {
		if (!canExpand)
			return;
		Vector2 tmp = recTra.rect.size - new Vector2 (1f, 1f) * Mathf.Sqrt (recTra.rect.size.sqrMagnitude / 2f);
		recTra.sizeDelta -= tmp;

		Vector2 foo = new Vector2 (vec.x > 0 ? 1f : -1f, vec.y > 0 ? 1f : -1f);
		float len = Mathf.Sqrt (vec.sqrMagnitude / 2f);
		Vector2 square = new Vector2(foo.x * expandDir.x, foo.y * expandDir.y) * len;
		recTra.sizeDelta += square;

		if (recTra.rect.width < menuRT.rect.width + 20f) {
			recTra.sizeDelta = new Vector2 (menuRT.rect.width + 20f, menuRT.rect.width + 20f);
		} else {
			if (expandDir.x == 0f)
				foo.x = 0f;
			if (expandDir.y == 0f)
				foo.y = 0f;
			Translate (foo * len / 2f);
		}
		TranslateIntoWindowManager ();
		/*
		Vector2 foo = new Vector2 (vec.x * expandDir.x, vec.y * expandDir.y);
		Vector2 hoge = recTra.sizeDelta;
		//Debug.Log ("tmp=" + (tmp + recTra.rect.size * canvas.x));
		recTra.sizeDelta = tmp;
		if (recTra.rect.width < menuRT.rect.width + 20f || recTra.rect.height < menuRT.rect.width + 20f) {
			recTra.sizeDelta = hoge;
		} else {
			if (expandDir.x == 0f)
				vec.x = 0f;
			if (expandDir.y == 0f)
				vec.y = 0f;
			Translate (vec / 2f);
		*/
	}

	public void Destroy() {
		isDestroyed = true;
		mwm.CallRemove ();
	}

	public bool Contains(Vector2 vec) {
		Vector2 v = ScreenToWindow (vec);
		Vector2 menuV = v - (Vector2)menuRT.localPosition, frameV = v - (Vector2)frameRT.localPosition;
		return menuRT.rect.Contains (menuV) || frameRT.rect.Contains (frameV);
	}

	/*
	public bool Contains(Vector2 vec) {
		rect = recTra.rect;
		rect.size *= canvas.x;
		Vector2 tmp = vec - (Vector2)this.transform.position + rect.size / 2;
		return (tmp.x >= 0 && tmp.x <= rect.width && tmp.y >= 0 && tmp.y <= rect.height - 13.1 * canvas.x) || IsInMenu(vec);
	}
	*/

	public bool IsInMenu(Vector2 vec) {
		Vector2 v = ScreenToWindow (vec);
		Vector2 menuV = v - (Vector2)menuRT.localPosition;
		return menuRT.rect.Contains (menuV);
	}

	/*
	public bool IsInMenu(Vector2 vec) {
		rect = recTra.rect;
		rect.size *= canvas.x;
		Vector2 tmp = vec - (Vector2)this.transform.position + rect.size / 2 - new Vector2(0f, rect.height - 13.1f * canvas.x);
		return tmp.x >= 0 && tmp.x <= 114.7 * canvas.x && tmp.y >= 0 && tmp.y <= 13.1 * canvas.x;
	}
	*/

	public bool IsInWindowManager() {
		Vector2 v = (Vector2)recTra.localPosition;
		Vector2 size = recTra.rect.size;
		return parRecTra.rect.Contains (v - size / 2f) && parRecTra.rect.Contains (v + size / 2f);// && parRect.Contains (v + new Vector2 (size.x, size.y * -1f) / 2f) && parRect.Contains (v + new Vector2 (size.x * -1f, size.y) / 2f);
	}

	/*
	public bool IsOnSide(Vector2 vec) {
		Vector2 expandDir = new Vector2 ();
		//bool a = IsOnLeftSide (vec);
		//bool b = IsOnBottomSide (vec);
		//bool c = IsOnRightSide (vec);
		//bool d = IsOnTopSide (vec);
		//return a || b || c || d;
	}
	*/

	public Vector2 GetExpandDir(Vector2 vec) {
		Vector2 expandDir = new Vector2 ();
		expandDir.x += IsOnLeftSide(vec);
		expandDir.x += IsOnRightSide(vec);
		expandDir.y += IsOnBottomSide(vec);
		expandDir.y += IsOnTopSide(vec);
		return expandDir;
	}

	public float IsOnLeftSide(Vector2 vec) {
		Vector2 v = ScreenToWindow (vec);
		Rect r = new Rect (recTra.rect.x, recTra.rect.y, 10f, recTra.rect.height);
		//if (r.Contains (v)) expandDir.x = -1f;
		//return r.Contains (v);
		return r.Contains(v) ? -1f : 0f;
	}

	public float IsOnBottomSide(Vector2 vec) {
		Vector2 v = ScreenToWindow (vec);
		Rect r = new Rect(recTra.rect.x, recTra.rect.y, recTra.rect.width, 10f);
		//if (r.Contains (v)) expandDir.y = -1f;
		//return r.Contains (v);
		return r.Contains(v) ? -1f : 0f;
	}

	public float IsOnRightSide(Vector2 vec) {
		Vector2 v = ScreenToWindow (vec);
		Rect r = new Rect(recTra.rect.width / 2 - 10f, recTra.rect.y, 10f, recTra.rect.height);
		//if (r.Contains (v)) expandDir.x = 1f;
		//return r.Contains (v);
		return r.Contains(v) ? 1f : 0f;
	}

	public float IsOnTopSide(Vector2 vec) {
		Vector2 v = ScreenToWindow (vec);
		Rect r = new Rect(recTra.rect.x, recTra.rect.height / 2 - 10f, recTra.rect.width, 10f);
		//if (r.Contains (v)) expandDir.y = 1f;
		//return r.Contains (v);
		return r.Contains(v) ? 1f : 0f;
	}

	public Vector2 ScreenToWindow(Vector2 vec) {
		//Debug.Log ("position=" + (Vector2)recTra.position);
		//Debug.Log ("local=" + (vec - (Vector2)this.transform.position) / canvas.x);

		return (vec - (Vector2)this.transform.position) / canvas.x;
	}
}
