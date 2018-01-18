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
		SetText (tp);
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
			if (localPos.x < viewRecTra.rect.width / -2f - 0.01f || localPos.x > viewRecTra.rect.width / 2f + 0.01f ||
				localPos.y < viewRecTra.rect.height / -2f - 0.01f || localPos.y > viewRecTra.rect.height / 2f + 0.01f) {
				Debug.Log ("text out");
				text.gameObject.SetActive (false);
			} else {
				text.gameObject.SetActive (true);
			}
		}
	}

	public virtual void Hide() {
		Draw (false, false);
	}

	private void SetText(TextPos tp) {
		switch (tp) {
		case TextPos.Below:
			textRecTra.pivot = new Vector2 (0.5f, 1f);
			text.alignment = TextAnchor.MiddleCenter;
			break;
		case TextPos.Right:
			textRecTra.pivot = new Vector2 (0f, 0.5f);
			text.alignment = TextAnchor.MiddleCenter;
			break;
		case TextPos.Above:
			textRecTra.pivot = new Vector2 (0.5f, 0f);
			text.alignment = TextAnchor.MiddleCenter;
			break;
		case TextPos.Left:
			textRecTra.pivot = new Vector2 (1f, 0.5f);
			text.alignment = TextAnchor.MiddleCenter;
			break;
		case TextPos.BelowLeft:
			textRecTra.pivot = new Vector2 (1f, 1f);
			text.alignment = TextAnchor.MiddleRight;
			break;
		default:
			break;
		}
	}
}

public enum TextPos {
	Below,
	Right,
	Above,
	Left,
	BelowLeft,
}