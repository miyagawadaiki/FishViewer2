using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLineController : MonoBehaviour {

	[SerializeField]
	protected GameObject textObj = null;

	protected RectTransform viewRecTra, lineRecTra, textRecTra;
	protected Image image;
	protected Text text;
	protected Color defColor;

	public bool isAxis, isVertical;
	public Vector2 localPos;
	public float value;


	protected virtual void Awake() {
		viewRecTra = this.transform.parent.GetComponent<RectTransform> ();
		lineRecTra = this.GetComponent<RectTransform> ();
		textRecTra = textObj.GetComponent<RectTransform> ();
		image = this.GetComponent<Image> ();
		text = textObj.GetComponent<Text> ();
		defColor = image.color;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void Set(bool isVertical, Vector2 localPos, float value) {
		this.isVertical = isVertical;
		this.localPos = localPos;
		this.value = value;
	}

	public virtual void SetCircle(Vector2 localZero, float localRadius, float angle, float value) {

	}

	public virtual void SetAngle(Vector2 localZero) {

	}

	public virtual void Draw(bool drawLine, bool showText) {
		if (isAxis) {
			image.color = Color.black;
			text.color = Color.black;
		} else {
			image.color = defColor;
			text.color = defColor;
		}

		if (isVertical) {
			lineRecTra.anchorMin = new Vector2 (0.5f, 0f);
			lineRecTra.anchorMax = new Vector2 (0.5f, 1f);
			lineRecTra.sizeDelta = new Vector2 (0f, 0f);
			lineRecTra.localPosition = new Vector2 (localPos.x, 0f);
			textRecTra.localPosition = new Vector2 (0f, localPos.y);
		} else {
			lineRecTra.anchorMin = new Vector2 (0f, 0.5f);
			lineRecTra.anchorMax = new Vector2 (1f, 0.5f);
			lineRecTra.sizeDelta = new Vector2 (0f, 0f);
			lineRecTra.localPosition = new Vector2 (0f, localPos.y);
			textRecTra.localPosition = new Vector2 (localPos.x, 0f);
		}

		text.text = value + "";
		text.gameObject.SetActive (false);

		if (drawLine) {
			if ((isVertical && (localPos.x < viewRecTra.rect.width / -2f || localPos.x > viewRecTra.rect.width / 2f)) ||
				(!isVertical && (localPos.y < viewRecTra.rect.height / -2f || localPos.y > viewRecTra.rect.height / 2f))) {
				Debug.Log ("Hide : " + localPos);
			} else {
				if (isVertical) {
					lineRecTra.sizeDelta = new Vector2 (isAxis ? 1.5f : 1f, 0f);
				} else {
					lineRecTra.sizeDelta = new Vector2 (0f, isAxis ? 1.5f : 1f);
				}
			}
		}

		if (showText) {
			if (localPos.x < viewRecTra.rect.width / -2f || localPos.x > viewRecTra.rect.width / 2f ||
				localPos.y < viewRecTra.rect.height / -2f || localPos.y > viewRecTra.rect.height / 2f)
				text.gameObject.SetActive (false);
			else
				text.gameObject.SetActive (true);
		}
	}

	public virtual void Hide() {
		Draw (false, false);
	}




	/*
	public virtual void Draw(bool isVertical, Vector2 localPos, float bold, float value) {
		//Debug.Log ("localPos = " + localPos);
		if (isVertical && (localPos.x < viewRecTra.rect.width / -2f || localPos.x > viewRecTra.rect.width / 2f)) {
			Hide ();
			return;
		}
		if (!isVertical && (localPos.y < viewRecTra.rect.height / -2f || localPos.y > viewRecTra.rect.height / 2f)) {
			Hide ();
			return;
		}

		if (localPos.x < viewRecTra.rect.width / -2f || localPos.x > viewRecTra.rect.width / 2f ||
		    localPos.y < viewRecTra.rect.height / -2f || localPos.y > viewRecTra.rect.height / 2f)
			text.gameObject.SetActive (false);
		else
			text.gameObject.SetActive (true);
		
		if (isVertical) {
			lineRecTra.anchorMin = new Vector2 (0.5f, 0f);
			lineRecTra.anchorMax = new Vector2 (0.5f, 1f);
			lineRecTra.sizeDelta = new Vector2 (bold, 0f);
			//lineRecTra.sizeDelta = new Vector2 (bold, viewRecTra.rect.height);
			lineRecTra.localPosition = new Vector2 (localPos.x, 0f);
			textRecTra.localPosition = new Vector2 (0f, localPos.y);
		} else {
			lineRecTra.anchorMin = new Vector2 (0f, 0.5f);
			lineRecTra.anchorMax = new Vector2 (1f, 0.5f);
			lineRecTra.sizeDelta = new Vector2 (0f, bold);
			//lineRecTra.sizeDelta = new Vector2 (viewRecTra.rect.width, bold);
			lineRecTra.localPosition = new Vector2 (0f, localPos.y);
			textRecTra.localPosition = new Vector2 (localPos.x, 0f);
		}
		text.text = value + "";

		//if (!viewRecTra.rect.Contains (lineRecTra.localPosition))
			//Hide ();
	}

	public virtual void DrawLineOnly(bool isVertical, Vector2 localPos, float bold) {
		if (isVertical && (localPos.x < viewRecTra.rect.width / -2f || localPos.x > viewRecTra.rect.width / 2f)) {
			Hide ();
			return;
		}
		if (!isVertical && (localPos.y < viewRecTra.rect.height / -2f || localPos.y > viewRecTra.rect.height / 2f)) {
			Hide ();
			return;
		}

		text.gameObject.SetActive (false);
		if (isVertical) {
			lineRecTra.anchorMin = new Vector2 (0.5f, 0f);
			lineRecTra.anchorMax = new Vector2 (0.5f, 1f);
			lineRecTra.sizeDelta = new Vector2 (bold, 0f);
			//lineRecTra.sizeDelta = new Vector2 (bold, viewRecTra.rect.height);
			lineRecTra.localPosition = new Vector2 (localPos.x, 0f);
			textRecTra.localPosition = new Vector2 (0f, localPos.y);
		} else {
			lineRecTra.anchorMin = new Vector2 (0f, 0.5f);
			lineRecTra.anchorMax = new Vector2 (1f, 0.5f);
			lineRecTra.sizeDelta = new Vector2 (0f, bold);
			//lineRecTra.sizeDelta = new Vector2 (viewRecTra.rect.width, bold);
			lineRecTra.localPosition = new Vector2 (0f, localPos.y);
			textRecTra.localPosition = new Vector2 (localPos.x, 0f);

		}

		//if (!viewRecTra.rect.Contains (lineRecTra.localPosition))
			//Hide ();
	}

	public virtual void DrawTextOnly(Vector2 localPos, float value) {
		Draw (true, localPos, 0f, value);
	}

	/*
	public virtual void Hide() {
		DrawLineOnly (true, new Vector2 (), 0f);
	}


	public virtual void SetAxis() {
		image.color = Color.black;
		text.color = Color.black;
	}

	public virtual void SetGrid() {
		image.color = defColor;
		text.color = defColor;
	}
	*/
}
