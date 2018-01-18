using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphContent : MyWindowContent {

	[SerializeField]
	protected RectTransform graphRecTra = null;
	[SerializeField]
	protected Text graphTitleText = null;

	//[System.NonSerialized]
	//public GraphManager memo = null;
	[System.NonSerialized]
	public ViewMode viewMode = ViewMode.ShowAxis;

	protected GraphContentType gcType;

	// Use this for initialization
	public virtual void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.SetSize (defaultSize);
		int num = mwc.mwm.Count ();
		//Vector2 tmp = mwc.mwm.GetComponent<RectTransform> ().rect.size;
		//Vector2 hoge = mwc.GetComponent<RectTransform> ().rect.size;
		//Vector2 topLeft = new Vector2 (-tmp.x / 2 + hoge.x / 2, tmp.y / 2 - hoge.y / 2);
		//mwc.MoveTo (topLeft + new Vector2 (10f * num + 10f, - 10f * num - 10f));
		mwc.MoveTo(new Vector2(0.1f * num - 1f, 1f - 0.1f * num));

		GameObject obj = Instantiate (Resources.Load ("Menubar/Grid") as GameObject, mwc.menuArea.transform);
		obj.GetComponent<GridButtonController> ().gc = this;
		obj.GetComponent<Button>().onClick.AddListener(() => ChangeGridMode());

		obj = Instantiate (Resources.Load("Menubar/Setting") as GameObject, mwc.menuArea.transform);
		obj.GetComponent<Button> ().onClick.AddListener (() => OpenSettingWindow ());

		OpenSettingWindow ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void Init() {

	}

	public virtual void Set(string parameters) {

	}

	public virtual void OpenSettingWindow() {
		mwc.mwm.AddWindow (System.Enum.GetName(typeof(GraphContentType), gcType) + "GraphSetting");
		mwc.mwm.GetLastWindowController ().gameObject.GetComponentInChildren<GraphSettingContent> ().RegisterGraphContent (this);
	}

	public virtual void RemoveGraphManager() {

	}

	public void ChangeGridMode() {
		switch (viewMode) {
		case ViewMode.ShowAxis:
			viewMode = ViewMode.ShowGrid;
			break;
		case ViewMode.ShowGrid:
			viewMode = ViewMode.ShowCompletely;
			break;
		case ViewMode.ShowCompletely:
			viewMode = ViewMode.Hide;
			break;
		case ViewMode.Hide:
			viewMode = ViewMode.ShowAxis;
			break;
		default:
			break;
		}

		ShowView ();
	}

	public virtual void ShowView() {

		switch(viewMode) {
		case ViewMode.ShowAxis:
			SetGrid ();
			ShowAxis ();
			break;
		case ViewMode.ShowGrid:
			SetGrid ();
			ShowGrid ();
			break;
		case ViewMode.ShowCompletely:
			SetCompletely ();
			ShowCompletely ();
			break;
		case ViewMode.Hide:
			HideView ();
			break;
		default:
			break;
		}
	}

	public virtual void SetGrid() {

	}

	public virtual void SetCompletely() {

	}

	public virtual void ShowAxis() {

	}

	public virtual void ShowGrid() {

	}

	public virtual void ShowCompletely() {

	}

	public virtual void HideView() {

	}

	public virtual void Translate(Vector2 start, Vector2 end) {
		
	}

	public virtual void Expand(float expand) {

	}

	public override void OnExpand(Vector2 vec, Vector2 expandDir) {
		ShowView();
	}
		
	public override void OnLeftClick(Vector2 pos) {
		mwc.AppearMenu ();
	}

	public override void OnRightClick(Vector2 pos) {
		
	}

	public override void OnLeftDrag(Vector2 start, Vector2 end) {

	}

	public override void OnRightDrag(Vector2 start, Vector2 end) {
		Translate (start, end);
	}

	public override void OnWheelChange(float value) {
		Expand (value);
	}

	public virtual string GetTitle() {
		return "";
	}

	public virtual string GetParameterText() {
		return "";
	}
}

public enum GraphContentType {
	Single,
	MultiEven,
	MultiVarious
}

public enum ViewMode {
	ShowAxis,
	ShowGrid,
	ShowCompletely,
	Hide,
}