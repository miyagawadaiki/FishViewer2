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
	protected Color lineDefColor, textDefColor;

	public bool isAxis, isVertical;
	public Vector2 localPos;
	public float value;

	private Vector2 pivotMemo;
	private Vector2[] textPivotMemo = {
		new Vector2 (0.5f, 1.2f),
		new Vector2 (0f, 0.5f),
		new Vector2 (0.5f, -0.2f),
		new Vector2 (1f, 0.5f),
		new Vector2 (1f, 1.2f),
		new Vector2 (-0.1f, 1.4f),
	};
	private TextAnchor[] textAnchorMemo = {
		TextAnchor.LowerCenter,
		TextAnchor.MiddleCenter,
		TextAnchor.UpperCenter,
		TextAnchor.MiddleCenter,
		TextAnchor.MiddleRight,
		TextAnchor.MiddleLeft,
	};
	private Vector2[] linePivotMemo = {
		new Vector2 (0.5f, 0f),
		new Vector2 (1f, 0.5f),
		new Vector2 (0.5f, 1f),
		new Vector2 (0f, 0.5f),
		new Vector2 (0.5f, 0.5f),
		new Vector2 (0.5f, 0.5f)
	};

	private Vector2 zero = new Vector2 ();
	//private Vector2 leftBottom = new Vector2 ();
	private Vector2 centerBottom = new Vector2 (0.5f, 0f);
	private Vector2 rightBottom = new Vector2 (1f, 0f);
	private Vector2 leftMiddle = new Vector2 (0f, 0.5f);
	private Vector2 centerMiddle = new Vector2 (0.5f, 0.5f);
	private Vector2 rightMiddle = new Vector2 (1f, 0.5f);
	private Vector2 leftTop = new Vector2 (0f, 1f);
	private Vector2 centerTop = new Vector2 (0.5f, 1f);
	private Vector2 rightTop = new Vector2 (1f, 1f);

	protected virtual void Awake() {
		viewRecTra = this.transform.parent.GetComponent<RectTransform> ();
		lineRecTra = this.GetComponent<RectTransform> ();
		textRecTra = textObj.GetComponent<RectTransform> ();
		image = this.GetComponent<Image> ();
		text = textObj.GetComponent<Text> ();
		lineDefColor = image.color;
		textDefColor = text.color;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void Set(bool isVertical, Vector2 localPos, float value, TextPos tp) {
		this.isVertical = isVertical;
		this.localPos = localPos;
		this.value = value;
		textRecTra.pivot = textPivotMemo[(int)tp];
		text.alignment = textAnchorMemo[(int)tp];
		pivotMemo = linePivotMemo[(int)tp];
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
			image.color = lineDefColor;
			text.color = textDefColor;
		}

		lineRecTra.pivot = centerMiddle;

		if (isVertical) {
			lineRecTra.anchorMin = centerBottom;
			lineRecTra.anchorMax = centerTop;
			lineRecTra.sizeDelta = zero;
			lineRecTra.localPosition = rightBottom * localPos.x;
			textRecTra.localPosition = leftTop * localPos.y;
		} else {
			lineRecTra.anchorMin = leftMiddle;
			lineRecTra.anchorMax = rightMiddle;
			lineRecTra.sizeDelta = zero;
			lineRecTra.localPosition = leftTop * localPos.y;
			textRecTra.localPosition = rightBottom * localPos.x;
		}

		text.text = value + "";
		text.gameObject.SetActive (false);

		if (drawLine) {
			if ((isVertical && (localPos.x < viewRecTra.rect.width / -2f || localPos.x > viewRecTra.rect.width / 2f)) ||
				(!isVertical && (localPos.y < viewRecTra.rect.height / -2f || localPos.y > viewRecTra.rect.height / 2f))) {
				//Debug.Log ("Hide : " + localPos);
			} else {
				if (isVertical) {
					lineRecTra.sizeDelta = rightBottom * (isAxis ? 1.5f : 1f);
				} else {
					lineRecTra.sizeDelta = leftTop * (isAxis ? 1.5f : 1f);
				}
			}
		}

		if (showText) {
			if (localPos.x < viewRecTra.rect.width / -2f - 0.01f || localPos.x > viewRecTra.rect.width / 2f + 0.01f ||
				localPos.y < viewRecTra.rect.height / -2f - 0.01f || localPos.y > viewRecTra.rect.height / 2f + 0.01f) {
				//Debug.Log ("text out");
				text.gameObject.SetActive (false);
			} else {
				text.gameObject.SetActive (true);
			}
		}
	}

	public virtual void DrawShort() {
		if (isAxis) {
			image.color = Color.black;
			text.color = Color.black;
		} else {
			image.color = lineDefColor;
			text.color = textDefColor;
		}

		lineRecTra.anchorMin = centerMiddle;
		lineRecTra.anchorMax = centerMiddle;
		lineRecTra.pivot = pivotMemo;
		textRecTra.localPosition = (pivotMemo - centerMiddle) * 10f;
		lineRecTra.localPosition = localPos;

		if (isVertical) {
			lineRecTra.sizeDelta = new Vector2 (1f, 10f);
		} else {
			lineRecTra.sizeDelta = new Vector2 (10f, 1f);
		}

		text.text = value + "";
		text.gameObject.SetActive (false);

		if ((isVertical && (localPos.x < viewRecTra.rect.width / -2f || localPos.x > viewRecTra.rect.width / 2f)) ||
			(!isVertical && (localPos.y < viewRecTra.rect.height / -2f || localPos.y > viewRecTra.rect.height / 2f))) {
			//Debug.Log ("Hide : " + localPos);
			lineRecTra.sizeDelta = new Vector2 ();
		}

		if (localPos.x < viewRecTra.rect.width / -2f - 0.01f || localPos.x > viewRecTra.rect.width / 2f + 0.01f ||
			localPos.y < viewRecTra.rect.height / -2f - 0.01f || localPos.y > viewRecTra.rect.height / 2f + 0.01f) {
			//Debug.Log ("text out");
			text.gameObject.SetActive (false);
		} else {
			text.gameObject.SetActive (true);
		}
	}

	public virtual void Hide() {
		Draw (false, false);
	}
}

public enum TextPos {
	Below,
	Right,
	Above,
	Left,
	BelowLeft,
	BelowRight,
}