using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text;
using MiniJSON;

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

	private Building selectedBuilding;

	// Use this for initialization
	void Start () {
		view_src = true;
		cc = GameObject.Find ("CityCreater").GetComponent<CityCreater> ();
		foreach( Transform child in canvas.transform){
			file_name = child.gameObject.GetComponent<Text>();
			file_name.text = "";
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
			Building hitBuilding = hit.transform.GetComponent<Building>();
			if (hitBuilding != null)
			{
				if (selectedBuilding == null || selectedBuilding != hitBuilding){
					var path = SearchPathFromFileName(hit.transform.name);
					src_txt = ReadFile(path);

					file_name.text = hitBuilding.transform.name;

					if (selectedBuilding)
					{
						selectedBuilding.Deselected();
					}

					selectedBuilding = hitBuilding;
					selectedBuilding.Selected();
				}
			}
 		} else {
			if (selectedBuilding)
			{
				selectedBuilding.Deselected();
			}
			file_name.text = "";
		}
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
