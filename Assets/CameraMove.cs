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

	private float rotationY = 0f;
	private const float CAMERA_SPEED = 200f;
	private const float CAMERA_CONTROL_SENSITIVITY = 3F;
	private const float MIN_ROTATION_Y = -30F;
	private const float MAX_ROTATION_Y = 30F;

	void Awake()
	{
		// Not to change rigidbody when rotating
		Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
		if (rigidBody)
		{
			rigidBody.freezeRotation = true;
		}
	}

	// Use this for initialization
	void Start ()
	{
		view_src = true;
		cc = GameObject.Find ("CityCreater").GetComponent<CityCreater> ();
		foreach(Text text in canvas.transform.GetComponentsInChildren<Text>())
		{
			text.text = "";
		}

	}

	// Update is called once per frame
	void Update ()
	{
		ControlByKeyboard();
		ControlByMouse();
	}

	private void ControlByKeyboard()
	{
		Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
		Vector3 velocity = Vector3.zero;

		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			velocity += transform.forward * CAMERA_SPEED;
		}
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			velocity += transform.forward * CAMERA_SPEED * -1;
		}

		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			velocity += transform.right *  CAMERA_SPEED * -1;
		}
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			velocity += transform.right * CAMERA_SPEED;
		}

		rigidBody.velocity = velocity;

		if (Input.GetKey(KeyCode.Space))
		{
			rigidBody.velocity = transform.up * CAMERA_SPEED * (Input.GetKey(KeyCode.LeftShift) ? -1 : 1);
		}
	}

	private void ControlByMouse()
	{
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * CAMERA_CONTROL_SENSITIVITY;
		rotationY += Input.GetAxis("Mouse Y") * CAMERA_CONTROL_SENSITIVITY;
		rotationY = Mathf.Clamp(rotationY, MIN_ROTATION_Y, MAX_ROTATION_Y);
		transform.localEulerAngles = new Vector3(rotationY * -1, rotationX, 0);

		Building building = GetRaycastHitBuilding();
		HighlighMouseOverBuilding(building);

		if (Input.GetMouseButtonDown(0))
		{
			MouseClicked(building);
		}
	}

	private void MouseClicked(Building building)
	{
		if (building == null) {return;}
		Debug.Log("click : " + building.name);
	}

	private void HighlighMouseOverBuilding(Building building)
	{
		if (building != null)
		{
			if (selectedBuilding == null || selectedBuilding != building){
				file_name.text = building.transform.name;

				if (selectedBuilding)
				{
					selectedBuilding.Deselected();
				}

				selectedBuilding = building;
				selectedBuilding.Selected();
			}
		}
		else
		{
			if (selectedBuilding)
			{
				selectedBuilding.Deselected();
			}
			file_name.text = "";
		}
	}

	private Building GetRaycastHitBuilding()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 10000)) {
			Building hitBuilding = hit.transform.GetComponent<Building>();
			return hitBuilding;
 		}

 		return null;
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
