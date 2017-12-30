using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLineController : MonoBehaviour {

	private bool isAxis = false;
	
	private RectTransform viewRecTra, lineRecTra, textRecTra;
	private Image image;
	private Text text;

	void Awake() {
		viewRecTra = this.transform.parent.GetComponent<RectTransform> ();
		lineRecTra = this.GetComponent<RectTransform> ();
		textRecTra = this.GetComponentInChildren<RectTransform> ();
		image = this.GetComponent<Image> ();
		text = this.GetComponentInChildren<Text> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Draw(bool isVertical, Vector2 localPos, float bold, float value) {
		if (isVertical) {
			lineRecTra.sizeDelta = new Vector2 (bold, viewRecTra.rect.height);
			lineRecTra.localPosition = new Vector2 (localPos.x, 0f);
			textRecTra.localPosition = new Vector2 (0f, localPos.y);
		} else {
			lineRecTra.sizeDelta = new Vector2 (viewRecTra.rect.width, bold);
			lineRecTra.localPosition = new Vector2 (0f, localPos.y);
			textRecTra.localPosition = new Vector2 (localPos.x, 0f);
		}
		text.text = value + "";
	}

	public void DrawTextOnly(Vector2 localPos, float value) {
		Draw (true, localPos, 0f, value);
	}

	public void SetAxis() {
		image.color = Color.black;
		text.color = Color.black;
	}
}
