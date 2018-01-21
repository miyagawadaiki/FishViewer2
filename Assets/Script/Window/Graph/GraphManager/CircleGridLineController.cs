using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleGridLineController : GridLineController {

	public float radius, angle;

	protected override void Awake () {
		base.Awake ();

		lineRecTra.anchorMin = new Vector2 (0.5f, 0.5f);
		lineRecTra.anchorMax = new Vector2 (0.5f, 0.5f);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void SetCircle(Vector2 localZero, float localRadius, float angle, float value) {
		localPos = localZero;
		radius = localRadius;
		this.angle = angle;
		this.value = value;
	}

	public override void Draw(bool drawLine, bool showText) {
		lineRecTra.sizeDelta = new Vector2 (1f, 1f) * radius * 2;
		lineRecTra.localPosition = localPos;
		textRecTra.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;

		lineRecTra.gameObject.SetActive (false);
		textRecTra.gameObject.SetActive (false);

		if (drawLine) {
			lineRecTra.gameObject.SetActive (true);
		}

		if (showText) {
			textRecTra.gameObject.SetActive (true);
			text.text = value + "";
		}
	}
}
