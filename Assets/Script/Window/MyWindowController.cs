using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyWindowController : MonoBehaviour {

	[System.NonSerialized]
	public MyWindowContent content;

	[SerializeField]
	private Image frameImg = null;
	[SerializeField]
	private Color normalColor = Color.white;
	[SerializeField]
	private Color selectedColor = Color.white;

	private RectTransform recTra;
	private Vector2 canvas;
	private Rect rect;
	private Vector2 expandDir;

	[System.NonSerialized]
	public bool isSelected = false;
	[System.NonSerialized]
	public bool isDestroyed = false;
	[System.NonSerialized]
	public bool canMove = true;
	[System.NonSerialized]
	public bool canExpand = true;

	void Awake() {
		SetNormalMode ();
	}

	// Use this for initialization
	void Start () {
		recTra = this.GetComponent<RectTransform> ();
		canvas = (Vector2)GameObject.Find ("Canvas").transform.localScale;

		Button destroy = this.GetComponentInChildren<Button> ();
		destroy.onClick.AddListener (() => this.GetComponentInParent<MyWindowManager> ().CallRemove ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSelectMode() {
		if (isSelected)
			return;
		isSelected = true;
		frameImg.color = selectedColor;
	}

	public void SetNormalMode() {
		if (!isSelected)
			return;
		isSelected = false;
		frameImg.color = normalColor;
	}

	public int GetSiblingIndex() {
		return recTra.GetSiblingIndex ();
	}
		
	public void SetAsLastSibling() {
		recTra.SetAsLastSibling();
	}

	public void Translate(Vector2 vec) {
		if (!canMove)
			return;
		recTra.position += (Vector3)vec;
	}

	public void Expand(Vector2 vec) {
		if (!canExpand)
			return;
		Vector2 foo = new Vector2 (vec.x * expandDir.x, vec.y * expandDir.y);
		Vector2 hoge = recTra.sizeDelta;
		Vector2 tmp = recTra.sizeDelta + foo / canvas.x;
		//Debug.Log ("tmp=" + (tmp + recTra.rect.size * canvas.x));
		recTra.sizeDelta = tmp;
		if (recTra.rect.width < 140f || recTra.rect.height < 70f) {
			recTra.sizeDelta = hoge;
		} else {
			Translate (vec / 2);
		}
	}

	public void Destroy() {
		isDestroyed = true;
	}

	public bool Contains(Vector2 vec) {
		rect = recTra.rect;
		rect.size *= canvas.x;
		Vector2 tmp = vec - (Vector2)this.transform.position + rect.size / 2;
		return (tmp.x >= 0 && tmp.x <= rect.width && tmp.y >= 0 && tmp.y <= rect.height - 13.1 * canvas.x) || IsInMenu(vec);
	}

	public bool IsInMenu(Vector2 vec) {
		rect = recTra.rect;
		rect.size *= canvas.x;
		Vector2 tmp = vec - (Vector2)this.transform.position + rect.size / 2 - new Vector2(0f, rect.height - 13.1f * canvas.x);
		return tmp.x >= 0 && tmp.x <= 114.7 * canvas.x && tmp.y >= 0 && tmp.y <= 13.1 * canvas.x;
	}

	public bool IsOnCorner(Vector2 vec) {
		return IsOnBottomRight (vec) || IsOnBottomLeft (vec) || IsOnTopRight (vec) || IsOnTopLeft (vec);
	}

	public bool IsOnBottomRight(Vector2 vec) {
		rect = recTra.rect;
		rect.size *= canvas.x;
		float rad = 20f;
		Vector2 tmp = vec - (Vector2)this.transform.position + rect.size / 2 + new Vector2(-rect.width, 0f);
		if (tmp.sqrMagnitude <= rad * rad)
			expandDir = new Vector2 (1f, -1f);
		return tmp.sqrMagnitude <= rad * rad;
	}

	public bool IsOnBottomLeft(Vector2 vec) {
		rect = recTra.rect;
		rect.size *= canvas.x;
		float rad = 20f;
		Vector2 tmp = vec - (Vector2)this.transform.position + rect.size / 2 + new Vector2(0f, 0f);
		if (tmp.sqrMagnitude <= rad * rad)
			expandDir = new Vector2 (-1f, -1f);
		return tmp.sqrMagnitude <= rad * rad;
	}

	public bool IsOnTopRight(Vector2 vec) {
		rect = recTra.rect;
		rect.size *= canvas.x;
		float rad = 20f;
		Vector2 tmp = vec - (Vector2)this.transform.position + rect.size / 2 + new Vector2(-rect.width, -rect.height);
		if (tmp.sqrMagnitude <= rad * rad)
			expandDir = new Vector2 (1f, 1f);
		return tmp.sqrMagnitude <= rad * rad;
	}

	public bool IsOnTopLeft(Vector2 vec) {
		rect = recTra.rect;
		rect.size *= canvas.x;
		float rad = 20f;
		Vector2 tmp = vec - (Vector2)this.transform.position + rect.size / 2 + new Vector2(0f, -rect.height);
		if (tmp.sqrMagnitude <= rad * rad)
			expandDir = new Vector2 (-1f, 1f);
		return tmp.sqrMagnitude <= rad * rad;
	}
}
