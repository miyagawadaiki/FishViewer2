using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyWindowController : MonoBehaviour {
	[SerializeField]
	private Image frameImg = null;
	[SerializeField]
	private Color normalColor = Color.white;
	[SerializeField]
	private Color selectedColor = Color.white;

	private RectTransform recTra;

	private bool isSelected = false;

	void Awake() {
		SetNormalMode ();
	}

	// Use this for initialization
	void Start () {
		recTra = this.GetComponent<RectTransform> ();
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

	public bool Contains(Vector2 vec) {
		Rect rect = recTra.rect;
		Vector2 tmp = vec - (Vector2)this.transform.position;
		Vector2 canvas = (Vector2)GameObject.Find ("Canvas").transform.localScale;
		return tmp.x >= -rect.width * canvas.x / 2 && tmp.x <= rect.width * canvas.x / 2 && tmp.y >= -rect.height * canvas.y / 2 && tmp.y <= rect.height * canvas.y / 2;
	}

	void OnMouseDown() {
		// Debug
		Debug.Log(this.ToString());
	}

	public bool IsSelect {
		get { return isSelected; }
	}
}
