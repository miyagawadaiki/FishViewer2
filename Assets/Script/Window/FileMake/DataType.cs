using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataType {

	// タグ名
	public string dataName;
	// 式のstring
	public string formula;

	private string[] elements;
	private List<float> parameters;
	private Stack<float> values;
	// 要素の区切り文字
	private char[] elementSeparator = { '\t', ' ' };
	// 接頭辞
	private char[] prefixSeparator = { '!', '#', '$', '@', '?' };

	public DataType () {
		parameters = new List<float> ();
		values = new Stack<float> ();
	}

	// タグ名と式textを受け取るコンストラクタ
	public DataType (string name, string text) : this () {
		this.dataName = name;
		this.formula = text;

		// textを分割する
		elements = text.Split (elementSeparator, StringSplitOptions.RemoveEmptyEntries);

		// parameterが記述されていればリストに登録
		for (int i = 0; i < elements.Length; i++) {
			if (elements [i] [0].Equals ('$')) {
				parameters.Add (0f);
			}
		}

		// それぞれのパラメータの比率を設定
		for (int i = 0; i < parameters.Count; i++)
			parameters [i] = 1f / parameters.Count;
	}


	// 式を評価して値を返す
	public float Eval (int step, int fish) {
		values.Clear ();

		for (int i = 0; i < elements.Length; i++) {
			// 接頭辞を取得
			char prefix = elements [i] [0];
			// 接頭辞を除去した文字列を用意
			string text = elements [i].Split (prefixSeparator, StringSplitOptions.RemoveEmptyEntries)[0];

			if (prefix >= '0' && prefix <= '9') {
				values.Push (float.Parse (elements [i]));
				continue;
			}

			switch (prefix) {
			case '!':	// データベース内変数の接頭辞 (e.q. !PositionX,0)
				values.Push (GetData (step, fish, text));
				break;
			//case '#':	// 数値の接頭辞 (e.q. #0)
			//	values.Push (float.Parse (text));
			//	break;
			case '$':	// パラメータの接頭辞 (e.q. $0, $1)
				values.Push (parameters [int.Parse (text)]);
				break;
			case '@':	// 定数の接頭辞 (e.q. @pi=3.141592, @dt)
				values.Push (DB.constNums [text]);
				break;
			case '?':	// 同じ式内においてこの要素より前で計算された結果を取得する接頭辞 (e.q. ?-1)
				float f = values.ToArray () [values.Count + int.Parse (text)];
				values.Push (f);
				break;
			default :	// 上記以外は演算子あるいは関数
				EvalOperator (step, fish, text);
				break;
			}
		}

		return values.Pop ();
	}

	// データベースから値をとってくる
	public float GetData (int step, int fish, string text) {
		char[] separator = { ',' };
		string[] tmp = text.Split (separator, StringSplitOptions.RemoveEmptyEntries);

		int tag = DB.tags[(tmp [0])];	// Dictionaryからindexを取得

		if (tmp.Length == 1)	// タグ名のみの記述ならindexを返す
			return (float)tag;
		
		int st = int.Parse (tmp [1]);	// 相対的なstep値を取得

		// 要検討：相対step値が0を下回ればその分データとして信用されるラインを上げる処理がしたい
		if (step + st < 0) {
			if (DB.start < step - st)
				DB.start = step - st;
			return 0f;
		}

		int fi = fish;	// 引数が1つだけなら魚のindexはそのまま
		if (tmp.Length > 2)	// 引数が2つあれば2つ目は魚のindexとして使う
			fi = int.Parse (tmp [2]);

		// データベースから値を取得
		return DB.data [step + st, fi, tag];
	}

	// パラメータの値を文字列化して取得 ParameterContentに渡す用
	public string GetParametersText () {
		if (parameters.Count < 2)
			return "";
		
		string s = parameters[0] + "";
		for (int i = 1; i < parameters.Count; i++)
			s += ":" + parameters [i];

		return s;
	}

	// ParameterContentからの文字列をパラメータ値として登録
	public void SetParametersText (string text) {
		parameters.Clear ();

		char[] sep = { ':' };
		string[] tmp = text.Split (sep, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < tmp.Length; i++)
			parameters.Add (float.Parse (tmp [i]));
	}

	// パラメータを含む式かどうか
	public bool HasParameter () {
		return parameters.Count > 1;
	}

	// どの関数か判断する 関数を増やすときはここにも追加すること
	public void EvalOperator (int step, int fish, string op) {
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
		case "IsNaN":
			IsNaN ();
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
		case "Sum":
			Sum (step, fish);
			break;
		case "Ave":
			Ave (step, fish);
			break;
		case "SD":
			StandardDeviation (step, fish);
			break;
		case "CalcAngle":
			CalcAngle ();
			break;
		default :
			break;
		}
	}

	// 引数の個数：役割

	// 2 : 和算
	public void Plus () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a + b);
	}

	// 2 : 減算 (先 - 後)
	public void Minus () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a - b);
	}

	// 2 : 乗算
	public void Multiply () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a * b);
	}

	// 2 : 除算 (先 / 後)
	public void Devide () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a / b);
	}

	// 2 : 余り (先 / 後)
	public void Rest () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a % b);
	}

	// 2 : 等号 (等しければ1 そうでなければ0)
	public void Equal () {
		float b = values.Pop ();
		float a = values.Pop ();
		if (a == b)
			values.Push (1f);
		else
			values.Push (0f);
	}

	// 2 : 不等号 (先 > 後)
	public void Bigger () {
		float b = values.Pop ();
		float a = values.Pop ();
		if (a > b)
			values.Push (1f);
		else
			values.Push (0f);
	}

	// 2 : 不等号 (先 < 後)
	public void Smaller () {
		float b = values.Pop ();
		float a = values.Pop ();
		if (a < b)
			values.Push (1f);
		else
			values.Push (0f);
	}

	// 3 : NaNかどうか (2項演算子的に書くと a.IsNaN ? b : c)
	public void IsNaN () {
		float c = values.Pop ();
		float b = values.Pop ();
		float a = values.Pop ();
		if (float.IsNaN (a))
			values.Push (b);
		else
			values.Push (c);
	}

	// 1 : 絶対値
	public void Abs () {
		float a = values.Pop ();
		values.Push (Mathf.Abs (a));
	}

	// 1 : 平方根
	public void Sqrt () {
		float a = values.Pop ();
		values.Push (Mathf.Sqrt (a));
	}

	// 1 : 逆余弦
	public void Acos () {
		float a = values.Pop ();
		if (a > 1f)
			a = 1f;
		else if (a < -1f)
			a = -1f;
		values.Push (Mathf.Acos (a));
	}

	// 3 : 条件式 (2項演算子的に書くと a > 0 ? b : c)
	public void If () {
		float c = values.Pop ();
		float b = values.Pop ();
		float a = values.Pop ();
		if (a > 0)
			values.Push (b);
		else
			values.Push (c);
	}

	// 2 : 累乗 (a ^ b)
	public void Pow () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (Mathf.Pow (a, b));
	}

	// 2 : ベクトル (a, b) の長さ
	public void Magnitude () {
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (Mathf.Sqrt (a * a + b * b));
	}

	// 4 : ベクトル (a, b) とベクトル (c, d) との内積
	public void InnerProduct () {
		float d = values.Pop ();
		float c = values.Pop ();
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a * c + b * d);
	}

	// 4 : ベクトル (a, b) とベクトル (c, d) との外積
	public void CrossProduct () {
		float d = values.Pop ();
		float c = values.Pop ();
		float b = values.Pop ();
		float a = values.Pop ();
		values.Push (a * d - b * c);
	}

	// 2 : 過去からの合計 (a = タグのindex, b = サンプル数)
	public void Sum (int step, int fish) {
		int b = (int)values.Pop ();
		int a = (int)values.Pop ();

		int num = b;
		float sum = 0f;

		if (b == 0) {
			num = DB.step - DB.start;
			for (int i = 0; i < num; i++) {
				sum += DB.data [i + DB.start, fish, a];
			}
			values.Push (sum);
			return;
		}
			
		if (step < DB.start) {
			values.Push (0f);
			return;
		}

		for (int i = 0; i < b; i++) {
			if (step - i < 0) {
				num--;
				continue;
			}
			sum += DB.data [step - i, fish, a];
		}

		values.Push (sum);
	}

	// 2 : 過去からの平均 (a = タグのindex, b = サンプル数)
	public void Ave (int step, int fish) {
		int b = (int)values.Pop ();
		int a = (int)values.Pop ();

		int num = b;
		float sum = 0f;

		if (b == 0) {
			num = DB.step - DB.start;
			for (int i = 0; i < num; i++) {
				sum += DB.data [i + DB.start, fish, a];
			}
			values.Push (sum / num);
			return;
		}

		if (step < DB.start) {
			values.Push (0f);
			return;
		}

		for (int i = 0; i < b; i++) {
			if (step - DB.start - i < 0) {
				num--;
				continue;
			}
			sum += DB.data [step - i, fish, a];
		}

		values.Push (sum / num);
	}

	// 2 : 過去からの標準偏差 (a = タグのindex, b = サンプル数)
	public void StandardDeviation (int step, int fish) {
		int b = (int)values.Pop ();
		int a = (int)values.Pop ();

		int num = b;
		float sum = 0f;

		if (b == 0) {
			num = DB.step - DB.start;
			for (int i = 0; i < num; i++) {
				sum += DB.data [i + DB.start, fish, a];
			}

			float av = sum / num;
			sum = 0f;
			for (int i = 0; i < num; i++) {
				float f = DB.data [i + DB.start, fish, a] - av;
				sum += f * f;
			}

			values.Push (Mathf.Sqrt (sum / num));
			return;
		}

		if (step < DB.start) {
			values.Push (0f);
			return;
		}

		for (int i = 0; i < b; i++) {
			if (step - i < 0) {
				num--;
				continue;
			}
			sum += DB.data [step - i, fish, a];
		}

		float ave = sum / num;
		sum = 0f;
		for (int i = 0; i < num; i++) {
			float f = DB.data [step - i, fish, a] - ave;
			sum += f * f;
		}

		// 割る数って(num - 1)じゃないと標本標準偏差にならないらしいけど, 標本数小さいから良いかな？
		values.Push (Mathf.Sqrt (sum / num));
	}

	// 4 : ベクトル (aX, bY) からベクトル (cX, dY) までの角度 (正負)
	public void CalcAngle () {
		float bY = values.Pop ();
		float bX = values.Pop ();
		float aY = values.Pop ();
		float aX = values.Pop ();

		float ip = aX * bX + aY * bY;
		float p = (aX * aX + aY * aY) * (bX * bX + bY * bY);
		if (p <= 0f) {
			values.Push (0f);
			return;
		}
		p = Mathf.Sqrt (p);

		float cos = ip / p;
		if (cos * cos > 1f)
			cos = (float)((int)cos);

		float cp = aX * bY - aY * bX;
		if (cp > 0f)
			values.Push (Mathf.Acos (cos));
		else
			values.Push (Mathf.Acos (cos) * -1f);
	}
}