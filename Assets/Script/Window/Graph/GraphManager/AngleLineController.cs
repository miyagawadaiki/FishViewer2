using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleLineController : GridLineController {

	[SerializeField]
	private Sprite axisSprite = null;
	[SerializeField]
	private Sprite angleSprite = null;

	protected override void Awake() {
		viewRecTra = this.transform.parent.GetComponent<RectTransform> ();
		lineRecTra = this.GetComponent<RectTransform> ();
		//textRecTra = textObj.GetComponent<RectTransform> ();
		image = this.GetComponent<Image> ();
		//text = textObj.GetComponent<Text> ();
		defColor = image.color;

	}

	// Use this for initialization
	void Start () {
		lineRecTra.anchorMin = new Vector2 (0.5f, 0.5f);
		lineRecTra.anchorMax = new Vector2 (0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void SetAngle(Vector2 localPos) {
		this.localPos = localPos;
	}

	public override void Draw(bool drawLine, bool showText) {
		this.gameObject.SetActive (true);

		if (isAxis) {
			image.sprite = axisSprite;
		} else {
			image.sprite = angleSprite;
		}
			
		lineRecTra.localPosition = localPos;
		if(Mathf.Abs(localPos.x) > Mathf.Abs(localPos.y))
			lineRecTra.sizeDelta = new Vector2 (1f, 1f) * (localPos.x > 0f ? localPos.x - viewRecTra.rect.width / -2f : viewRecTra.rect.width / 2f - localPos.x) * 2f;
		else
			lineRecTra.sizeDelta = new Vector2 (1f, 1f) * (localPos.y > 0f ? localPos.y - viewRecTra.rect.height / -2f : viewRecTra.rect.height / 2f - localPos.y) * 2f;
	}

	public override void Hide() {
		this.gameObject.SetActive (false);
	}
}
