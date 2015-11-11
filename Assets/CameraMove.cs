using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System; //Exception
using System.Text;
using MiniJSON;
//using CityCreater;

public class CameraMove : MonoBehaviour {
	const float SPEED = 0.1f;
	public Canvas canvas;
	public Text file_name;
	private string src_txt = "";
	public CityCreater cc;
	public string ta;
	public Color preColor;
	public Material viewMaterial;
	private bool view_src;
	// Use this for initialization
	void Start () {
		view_src = true;
		cc = GameObject.Find ("CityCreater").GetComponent<CityCreater> ();
		foreach( Transform child in canvas.transform){
			file_name = child.gameObject.GetComponent<Text>();
//			file_name = GameObject.Find("Text");
			file_name.text = "";
			//viewMaterial = new Material();
			//viewMaterial.color = Color.red;
		}

	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.UpArrow)){
			GetComponent<Rigidbody>().velocity = transform.forward * 100.0f;
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			//GetComponent<Rigidbody>().velocity = transform.right *  -100.0f;
			transform.Rotate(new Vector3(0,-0.8f,0));
		}
		if(Input.GetKey(KeyCode.DownArrow)){
			GetComponent<Rigidbody>().velocity = transform.forward * -100.0f;
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			//GetComponent<Rigidbody>().velocity = transform.right * 100.0f;
			transform.Rotate(new Vector3(0,0.8f,0));
		}
		if (Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.LeftShift)) {
			GetComponent<Rigidbody> ().velocity = transform.up * -100.0f;
		}
		else if (Input.GetKey (KeyCode.Space)) {
			GetComponent<Rigidbody>().velocity = transform.up * 100.0f;
		}
		if (Input.GetKey (KeyCode.V)) {
			view_src = true;
		}
		if (Input.GetKey (KeyCode.H)) {
			view_src = false;
		}
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, fwd, out hit, 10000)) {
			if (hit.transform.name != file_name.text){
				var prev = GameObject.Find (file_name.text);
				var next = GameObject.Find (hit.transform.name);
				var path = SearchPathFromFileName(hit.transform.name);
				src_txt = ReadFile(path);
				file_name.text = hit.transform.name;
				if (prev)
					prev.GetComponent<Renderer>().material.color = preColor;
					//prev.GetComponent<Renderer>().material.color = Color.white;
				preColor = next.GetComponent<Renderer>().material.color;
				next.GetComponent<Renderer>().material.color = Color.red;
				//next.GetComponent<Renderer>().material = viewMaterial;

			}
 		} else {
			var prev = GameObject.Find (file_name.text);
			if (prev)
				prev.GetComponent<Renderer>().material.color = preColor;
				//prev.GetComponent<Renderer>().material.color = Color.white;
			file_name.text = "";
		}


//		this.transform.Translate ( 0, 0,( Input.GetAxis ( "Vertical" ) * 1 ) );
//		this.transform.Rotate (0,( Input.GetAxis ("Horizontal" )  * 1 ),0);
	}
	// void OnGUI()
	// {
	// 	if(view_src)
	// 		//src_txt = GUI.TextArea (new Rect (5, 5, Screen.width-10, Screen.height-100), src_txt);
	// 		src_txt = GUI.TextArea (new Rect (5, 5, Screen.width-10, Screen.height-100), src_txt.Substring(0,Math.Min(1000,src_txt.Length)));

	// }
	/*
	void OnGUI () {
		// ラベルを表示する
		GUI.Label(new Rect(10,10,100,100), “MenuWindow”);
	}
	*/

	void OnCollisionEnter(Collision collision){
		 //file_name.text = collision.transform.name;
	}
	void OnCollisionExit(Collision collision){
		//file_name.text = "";
	}
	string ReadFile(string path){
		string st = "";
		return st;
	}

	string SearchPathFromFileName(string file_name){
		string path = "";
		IList buildings = cc.GetCity()["buildings"] as IList;
		foreach (Dictionary<string,object> building in buildings) {
			if(building["name"].ToString() == file_name){
				path = building["path"].ToString();
			}
		}
		return path;

	}

	// 改行コード処理
	string SetDefaultText(){
		return "cant read\n";
	}
}
