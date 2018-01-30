using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataType {

	public string name;

	private string[] elements;
	private List<float> parameters;
	private Stack<float> values;
	private char[] elementSeparator = { ' ' };
	private char[] prefixSeparator = { '#' };

	public DataType () {
		parameters = new List<float> ();
		values = new Stack<float> ();
	}

	public DataType (string name, string text) {
		this.name = name;

		elements = text.Split (elementSeparator, StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < elements.Length; i++) {
			if (elements [i] [0].Equals ('$'))
				parameters.Add (0f);
		}

		for (int i = 0; i < parameters.Count; i++)
			parameters [i] = 1f / parameters.Count;
	}

	public float Eval (int step) {
		for (int i = 0; i < elements.Length; i++) {
			char prefix = elements [i] [0];
			string text = elements [i].Split (prefixSeparator, StringSplitOptions.RemoveEmptyEntries)[0];

			switch (prefix) {
			case '!':
				break;
			case '#':
				values.Push (float.Parse (text));
				break;
			case '$':
				values.Push (parameters [int.Parse (text)]);
				break;
			default :
				EvalOperator (text);
				break;
			}
		}

		return values.Pop ();
	}

	public void SetData (string text) {
		char[] separator = { ',' };
		string[] tmp = text.Split (separator, StringSplitOptions.RemoveEmptyEntries);

	}

	public void EvalOperator (string op) {
		switch (op) {
		case "+":
			Plus ();
			break;
		case "-":
			Minus ();
			break;
		case "*":
			Multiply ();
			break;
		case "/":
			Devide ();
			break;
		case "%":
			Rest ();
			break;
		case "==":
			Equal ();
			break;
		case ">":
			Bigger ();
			break;
		case "<":
			Smaller ();
			break;
		case "Abs":
			Abs ();
			break;
		case "Sqrt":
			Sqrt ();
			break;
		case "Acos":
			Acos ();
			break;
		case "If":
			If ();
			break;
		case "Pow":
			Pow ();
			break;
		case "Mag":
			Magnitude ();
			break;
		case "Dot":
			InnerProduct ();
			break;
		case "Cross":
			CrossProduct ();
			break;
		case "Ave":
			Ave ();
			break;
		default :
			break;
		}
	}

	public void Plus () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a + b);
	}

	public void Minus () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a - b);
	}

	public void Multiply () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a * b);
	}

	public void Devide () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a / b);
	}

	public void Rest () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a % b);
	}

	public void Equal () {
		float b = values.Pop ();
		float a = values.Pop ();
		if (a == b)
			values.Push (1f);
		else
			values.Push (0f);
	}

	public void Bigger () {
		float b = values.Pop ();
		float a = values.Pop ();
		if (a > b)
			values.Push (1f);
		else
			values.Push (0f);
	}

	public void Smaller () {
		float b = values.Pop ();
		float a = values.Pop ();
		if (a < b)
			values.Push (1f);
		else
			values.Push (0f);
	}

	public void Abs () {
		float a = values.Pop ();
		values.Push (Mathf.Abs (a));
	}

	public void Sqrt () {
		float a = values.Pop ();
		values.Push (Mathf.Sqrt (a));
	}

	public void Acos () {
		float a = values.Pop ();
		if (a > 1f)
			a = 1f;
		else if (a < -1f)
			a = -1f;
		values.Push (Mathf.Acos (a));
	}

	public void If () {
		float c = values.Pop ();
		float b = values.Pop ();
		float a = values.Pop ();
		if (a > 0)
			values.Push (b);
		else
			values.Push (c);
	}

	public void Pow () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (Mathf.Pow (a, b));
	}

	public void Magnitude () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (Mathf.Sqrt (a * a + b * b));
	}

	public void InnerProduct () {
		float d = values.Pop ();
		float c = values.Pop ();
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a * c + b * d);
	}

	public void CrossProduct () {
		float d = values.Pop ();
		float c = values.Pop ();
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a * d - b * c);
	}

	public void Ave () {

	}
}