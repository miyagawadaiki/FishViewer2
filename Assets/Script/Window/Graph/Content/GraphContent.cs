using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

	public override void Awake () {
		base.Awake ();

		//if (!typeName.Equals (""))
		//	Set (typeName);
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();

		GameObject obj = Instantiate (Resources.Load ("Menubar/Grid") as GameObject, mwc.menuArea.transform);
		obj.GetComponent<GridButtonController> ().gc = this;
		obj.GetComponent<Button>().onClick.AddListener(() => ChangeGridMode());

		obj = Instantiate (Resources.Load ("Menubar/Capture") as GameObject, mwc.menuArea.transform);
		obj.GetComponent<Button> ().onClick.AddListener (() => OpenFileSelectWindow ());

		obj = Instantiate (Resources.Load("Menubar/Setting") as GameObject, mwc.menuArea.transform);
		obj.GetComponent<Button> ().onClick.AddListener (() => OpenSettingWindow ());

		if (typeName.Equals (""))
			OpenSettingWindow ();
		else
			Set (typeName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual bool IsReady () {
		return false;
	}

	public virtual void Init() {
		string s = System.Enum.GetName (typeof(GraphContentType), gcType);
		if (s [0] == 'M')
			s = s.Substring (5, s.Length - 5);
		if (s [0] != 'V')
			s += "\t";
		contentName = s + "\t" + GetShortTypeText ();
	}

	public virtual void Set(string parameters) {

	}

	public virtual void Plot (int step) {
		graphTitleText.text = GetTitle ();
	}

	public virtual void OpenSettingWindow() {
		mwc.mwm.AddWindow (System.Enum.GetName(typeof(GraphContentType), gcType) + "GraphSetting");
		mwc.mwm.GetLastWindowController ().gameObject.GetComponentInChildren<GraphSettingContent> ().RegisterGraphContent (this);
	}

	public void OpenFileSelectWindow () {
		char[] sep = { '.' };
		string n = ProjectData.FileName.GetName (ProjectData.FileKey.Read).Split (sep) [0];
		ProjectData.FileName.Set (ProjectData.FileKey.Image, ProjectData.FileName.GetPath (ProjectData.FileKey.Image),  n.Substring (0, n.Length - 3) + "_" + GetShortTitle () + ".png");
		mwc.mwm.AddWindow ("FileSelect/Image");
		mwc.mwm.GetLastWindowController ().gameObject.GetComponentInChildren<FileSelectContent> ().doneButton.onClick.AddListener (() => Capture ());
	}

	public virtual void RemoveGraphManager() {
		Simulation.Remove (this);
	}

	public void SetGridMode (ViewMode vm) {
		viewMode = vm;
		ShowView ();
	}

	public void ChangeGridMode() {
		switch (viewMode) {
		case ViewMode.ShowAxis:
			viewMode = ViewMode.ShowGrid;
			break;
		case ViewMode.ShowGrid:
			viewMode = ViewMode.ShowAxisCompletely;
			break;
		case ViewMode.ShowAxisCompletely:
			viewMode = ViewMode.ShowGridCompletely;
			break;
		case ViewMode.ShowGridCompletely:
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
			return;
		case ViewMode.ShowGrid:
			SetGrid ();
			ShowGrid ();
			return;
		case ViewMode.ShowAxisCompletely:
			SetCompletely ();
			ShowAxisCompletely ();
			return;
		case ViewMode.ShowGridCompletely:
			SetCompletely ();
			ShowGridCompletely ();
			return;
		case ViewMode.Hide:
			HideView ();
			return;
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

	public virtual void ShowAxisCompletely() {

	}

	public virtual void ShowGridCompletely() {

	}

	public virtual void HideView() {

	}

	public virtual void Translate(Vector2 start, Vector2 end) {
		if (!Simulation.playing)
			Plot (Simulation.step);
		ShowView ();
	}

	public virtual void Expand(float expand) {
		if (!Simulation.playing)
			Plot (Simulation.step);
		ShowView ();
	}

	public override void OnTranslate (Vector2 vec) {
		if (!IsReady ())
			return;

		base.OnTranslate (vec);
		if (!Simulation.playing)
			Plot (Simulation.step);
		ShowView ();
	}

	public override void OnExpand(Vector2 vec, Vector2 expandDir) {
		if (!IsReady ())
			return;

		base.OnExpand (vec, expandDir);
		if (!Simulation.playing)
			Plot (Simulation.step);
		ShowView ();
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

	public virtual string GetShortTypeText () {
		return "";
	}

	public virtual string GetShortTitle () {
		return "";
	}

	public virtual string GetParameterText() {
		return "";
	}

	public override void MakeClone () {
		base.MakeClone ();

		mwc.mwm.AddWindow (System.Enum.GetName (typeof (GraphContentType), gcType) + "Graph/" + this.GetParameterText ());
		MyWindowController mwc_ = mwc.mwm.GetLastWindowController ();
		mwc_.content.defaultSize = mwc.recTra.sizeDelta;
		mwc_.content.defaultPosition = mwc.transform.localPosition + new Vector3 (30f, -30f, 0f);
		mwc_.gameObject.GetComponentInChildren<GraphContent> ().SetGridMode (viewMode);
	}

	public void Capture () {
		StartCoroutine ("CaptureCoroutine");
	}

	private IEnumerator CaptureCoroutine () {
		Vector2 size = mwc.recTra.sizeDelta;
		mwc.recTra.SetAsLastSibling ();

		Texture2D tex = new Texture2D ((int)size.x + 1, (int)size.y + 1, TextureFormat.ARGB32, false);

		yield return new WaitForEndOfFrame ();

		tex.ReadPixels (new Rect (this.transform.position.x - size.x / 2f, this.transform.position.y - size.y / 2f, size.x, size.y), 0, 0);
		tex.Apply ();

		byte[] bytes = tex.EncodeToPNG ();
		Destroy (tex);
		File.WriteAllBytes (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Image), bytes);
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
	ShowAxisCompletely,
	ShowGridCompletely,
	Hide,
}