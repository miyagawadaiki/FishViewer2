using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimultionController : MonoBehaviour {

	[SerializeField]
	private Slider stepSlider = null;
	[SerializeField]
	private Slider repStartSlider = null;
	[SerializeField]
	private Slider repEndSlider = null;
	[SerializeField]
	private Slider speedSlider = null;
	[SerializeField]
	private Text speedText = null;
	[SerializeField]
	private Image playPauseImage = null;
	[SerializeField]
	private Sprite playSprite = null;
	[SerializeField]
	private Sprite pauseSprite = null;
	[SerializeField]
	private Image repButtonImage = null;
	[SerializeField]
	private Image backgroundImage = null;
	[SerializeField]
	private Image repStartHandle = null;
	[SerializeField]
	private Image repEndFill = null;
	[SerializeField]
	private Image repEndHandle = null;
	[SerializeField]
	private Text fileName = null;

	private bool playing, repeating;
	private float time = 0f, dt, defDt;
	private int repStartStep, repEndStep;
	private Color repButtonColor, repOnColor, repOffColor;
	private bool firstFlag = true;

	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		Init ();
	}

	public void Init() {
		stepSlider.maxValue = (float)DataBase.step;
		stepSlider.minValue = 0f;
		stepSlider.value = 0f;
		repStartSlider.maxValue = (float)DataBase.step;
		repStartSlider.minValue = 0f;
		repStartSlider.value = 0f;
		repEndSlider.maxValue = (float)DataBase.step;
		repEndSlider.minValue = 0f;
		repEndSlider.value = 0f;
		speedSlider.maxValue = 50f;
		speedSlider.minValue = 1f;
		speedSlider.value = 10f;

		if (firstFlag) {
			repButtonColor = repButtonImage.color;
			repOnColor = repStartHandle.color;
			repOffColor = backgroundImage.color;
			firstFlag = false;
		}

		fileName.text = ProjectData.FileName.GetName (ProjectData.FileKey.Read);

		defDt = DataBase.dt;

		Pause ();
		RepeatOff ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Simulation.step = (int)(stepSlider.value + 0.5f);

		repStartSlider.value = (int)(repStartSlider.value + 0.5f);
		repEndSlider.value = (int)(repEndSlider.value + 0.5f);
		if (repStartSlider.value > repEndSlider.value)
			repEndSlider.value = repStartSlider.value;
		
		repStartStep = (int)repStartSlider.value;
		repEndStep = (int)repEndSlider.value;

		speedSlider.value = (int)(speedSlider.value + 0.5f);
		speedText.text = "x" + (speedSlider.value / 10f) + "";

		dt = defDt / speedSlider.value * 10f;

		time += Time.deltaTime;

		if (time < dt)
			return;

		time = 0f;

		if (Simulation.IsEnd()) {
			Debug.Log ("<color=blue>Simulation end</color>");
			Pause ();
			Simulation.step = DataBase.step - 1;
			return;
		}

		if (playing) {
			Debug.Log ("<color=blue>Simulating... step=" + Simulation.step + "</color>");
			GoNext ();
		} else {
			Simulation.Execute ();
		}

		if (repeating) {
			if (Simulation.step > repEndStep)
				Simulation.step = repStartStep;
		}

		stepSlider.value = (float)Simulation.step;

	}

	public void Play() {
		playing = true;
		playPauseImage.sprite = pauseSprite;
	}

	public void Pause() {
		playing = false;
		playPauseImage.sprite = playSprite;
	}

	public void SwitchPlay() {
		if (playing)
			Pause ();
		else
			Play ();
	}

	public void RepeatOn() {
		repeating = true;
		repButtonImage.color = repButtonColor;
		repStartHandle.color = repOnColor;
		repEndFill.color = repOnColor;
		repEndHandle.color = repOnColor;
	}

	public void RepeatOff() {
		repeating = false;
		repButtonImage.color = Color.white;
		repStartHandle.color = repOffColor;
		repEndFill.color = repOffColor;
		repEndHandle.color = repOffColor;
	}

	public void SwitchRepeat() {
		if (repeating) {
			RepeatOff ();
		} else {
			RepeatOn ();
		}
	}

	public void Reset() {
		Simulation.step = 0;
		stepSlider.value = 0f;
	}

	public void GoNext() {
		Simulation.Execute ();
		Simulation.step++;
	}

	public void GoBack() {
		Simulation.Execute ();
		Simulation.step--;
	}
}
