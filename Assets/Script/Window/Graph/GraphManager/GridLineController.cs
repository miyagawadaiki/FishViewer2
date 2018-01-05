using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLineController : MonoBehaviour {

	[SerializeField]
	private GameObject textObj = null;
	
	private RectTransform viewRecTra, lineRecTra, textRecTra;
	private Image image;
	private Text text;

	void Awake() {
		viewRecTra = this.transform.parent.GetComponent<RectTransform> ();
		lineRecTra = this.GetComponent<RectTransform> ();
		textRecTra = textObj.GetComponent<RectTransform> ();
		Debug.Log ("textRectra.gameObject = " + textRecTra.gameObject.name);
		image = this.GetComponent<Image> ();
		text = textObj.GetComponent<Text> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Draw(bool isVertical, Vector2 localPos, float bold, float value) {
		Debug.Log ("localPos = " + localPos);
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
	}

	public void DrawLineOnly(bool isVertical, Vector2 localPos, float bold) {
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
	}

	public void DrawTextOnly(Vector2 localPos, float value) {
		Draw (true, localPos, 0f, value);
	}

	public void Hide() {
		DrawLineOnly (true, new Vector2 (), 0f);
	}

	public void SetAxis() {
		image.color = Color.black;
		text.color = Color.black;
	}
}
