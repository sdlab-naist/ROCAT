using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
	private Color color;

	public void Init(Color color)
	{
		this.color = color;

		SetMaterial(this.color);
	}

	public void Selected()
	{
		SetMaterial(Building.SELECTED_COLOR);
	}

	public void Deselected()
	{
		SetMaterial(color);
	}

	public void SetMaterial(Color color)
	{
		if (Building.HasMaterial(color))
		{
			GetComponent<Renderer>().sharedMaterial = Building.GetMaterial(color);
		}
		else
		{
			GetComponent<Renderer>().material.color = color;
			Building.AddMaterial(color, GetComponent<Renderer>().material);
		}
	}

#region Static
	private static Dictionary<Color, Material> materialDict = new Dictionary<Color, Material>();
	private static readonly Color SELECTED_COLOR = Color.red;

	public static bool HasMaterial(Color color)
	{
		return materialDict.ContainsKey(color);
	}

	public static Material GetMaterial(Color color)
	{
		return materialDict[color];
	}

	public static void AddMaterial(Color color, Material material)
	{
		materialDict.Add(color, material);
	}
#endregion
}
