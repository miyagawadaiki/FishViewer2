using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterContent : MyWindowContent {

	[SerializeField]
	private GameObject circleObj = null;
	[SerializeField]
	private Transform circleArea = null;
	[SerializeField]
	private GameObject paramNodeObj = null;
	[SerializeField]
	private Transform content = null;
	[SerializeField]
	private Button doneButton = null;

	private Image[] images;
	private Text[] texts;
	private char[] separator = { ',', ':', ' ' };
	private Vector2 localOrigin;
	private float[] memo;
	private int index;
	private float height;

	private Text paramText;

	public override void Awake () {
		base.Awake ();

		defaultPosition = new Vector2 ();
		mwc.canMove = false;
		mwc.canExpand = false;
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();

		height = this.GetComponent<RectTransform> ().rect.height;
		//Register (typeName);


		doneButton.onClick.AddListener (() => SetText ());
		doneButton.onClick.AddListener (() => mwc.Destroy ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RegisterTextArea (Text t) {
		paramText = t;
		Register (paramText.text);
	}

	public void Register (string text) {
		Debug.Log ("text = " + text);

		foreach (Transform t in circleArea)
			Destroy (t.gameObject);
		foreach (Transform t in content)
			Destroy (t.gameObject);

		string[] tmp = text.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		images = new Image[tmp.Length];
		texts = new Text[tmp.Length];

		float sum = 0f, prev = 0f;
		for (int i = 0; i < images.Length; i++) {
			sum += float.Parse (tmp [i]);
			images [i] = Instantiate (circleObj, circleArea).GetComponent<Image> ();
			images [i].fillAmount = sum;
			images [i].color = ProjectData.ColorList.colors [i];
			images [i].transform.SetAsFirstSibling ();

			GameObject obj = Instantiate (paramNodeObj, content) as GameObject;
			obj.GetComponentInChildren<Image> ().color = ProjectData.ColorList.colors [i];
			texts [i] = obj.GetComponentInChildren<Text> ();
			texts [i].text = (((int)((sum - prev) * 20f + 0.5f)) / 20f) + "";

			prev = sum;
		}

		images [images.Length - 1].fillAmount = 1f;
		texts [images.Length - 1].text = (((int)((1f - prev) * 20f + 0.5f)) / 20f) + "";
	}

	public void Register (int num) {
		string s = "";
		for (int i = 0; i < num; i++)
			s += (10f / num) + ",";

		Register (s);
	}

	public string GetParameterText () {
		float f = (int)(images [0].fillAmount * 20f + 0.5f) / 20f;
		string s = "" + f;
		for (int i = 1; i < images.Length; i++) {
			s += ":" + ((int)(images [i].fillAmount * 20f + 0.5f) / 20f - f);
			f = (int)(images [i].fillAmount * 20f + 0.5f) / 20f;
		}
	
		return s;
	}

	private void SetText () {
		if (paramText == null)
			return;
		
		paramText.text = GetParameterText ();
	}

	public float[] GetParameter () {
		float[] ret = new float[images.Length];
		for (int i = 0; i < images.Length; i++)
			ret [i] = (int)(images [i].fillAmount * 20f + 0.5f) / 20f;

		return ret;
	}

	private float GetAngle (Vector2 localPos) {
		float rad = Mathf.Acos (localPos.y / localPos.magnitude) * (localPos.x > 0f ? 1f : -1f) + (localPos.x > 0f ? 0f : Mathf.PI * 2f);
		return rad;
	}

	public override void OnLeftClick (Vector2 pos) {
		base.OnLeftClick (pos);

		localOrigin = pos - (Vector2)circleArea.position;

		memo = new float[images.Length];
		for (int i = 0; i < images.Length; i++)
			memo [i] = images [i].fillAmount;

		float angleNorm = GetAngle (localOrigin) / (Mathf.PI * 2);
		for (int i = 0; i < images.Length; i++) {
			if (images [i].fillAmount > angleNorm) {
				index = i;
				break;
			}
		}
	}

	public override void OnLeftDrag (Vector2 start, Vector2 end) {
		base.OnLeftDrag (start, end);

		float dist = (end.y - circleArea.position.y - localOrigin.y) / height;

		Debug.Log ("dist = " + dist);

		bool first = true;
		for (int i = index; i < images.Length - 1; i++) {
			
			float f = memo [i] + ((int)(dist * 20f + 0.5f)) / 20f;
			if (i == 0 && f < 0.05f)
				f = 0.05f;
			else if (i != 0 && first && f < images [i - 1].fillAmount + 0.05f) {
				f = images [i].fillAmount + 0.05f;
				first = false;
			}

			//Debug.Log ("f = " + f);
				
			if (1f - f > (images.Length - i - 1) * 0.05f) 
				images [i].fillAmount = f;

			texts [i].text = (int)((images [i].fillAmount - (i > 0 ? images [i - 1].fillAmount : 0)) * 20f + 0.5f) / 20f + "";

		}
		texts[texts.Length - 1].text = (int)((images [texts.Length - 1].fillAmount - images[texts.Length - 2].fillAmount) * 20f + 0.5f) / 20f + "";
	}
}
