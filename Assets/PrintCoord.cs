using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PrintCoord : MonoBehaviour
{
    public GameObject Points;
    Transform childTransform;
    private GameObject TextTransform;
    private int The_number_childs;
    private TextMeshPro Texts;
    private Vector3[] init_coords = new Vector3[6];
    private Vector3[] ref_coords = new Vector3[6];

    // Start is called before the first frame update
    void Start()
    {
       
        if (Points == null)
        {
            Debug.LogError("Points object not found! Make sure it exists in the Hierarchy and is active1.");
        }
        TextTransform = GameObject.Find("Note");
        if (TextTransform != null)
        {
            Debug.Log("Good!");
            Texts = TextTransform.GetComponent<TextMeshPro>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintCoordinates()
    {
        string positions = "";
        if (Points == null)
        {
            Debug.LogError("Points object not found! Make sure it exists in the Hierarchy and is active2.");
        }

        //The_number_childs = Points.transform.childCount;
        for (int i =0; i < 6; i++)
        {
            childTransform = Points.transform.GetChild(i);
            Vector3 pos = childTransform.transform.position;
            //positions += childTransform.name + ": " + childTransform.transform.position.ToString() + "\n";
            init_coords[i]=pos;
            positions += childTransform.name + ": " + (pos.x*1000).ToString("F3") + ", " + (pos.y*1000).ToString("F3") + ", " + (pos.z * 1000).ToString("F3") + "\n";

            Debug.Log(positions);
        }
        
        Texts.text= positions;
        
    }

    public void ReferenceCoordsSave()
    {
        string positions = "";
        if (Points == null)
        {
            Debug.LogError("Points object not found! Make sure it exists in the Hierarchy and is active2.");
        }

        //The_number_childs = Points.transform.childCount;
        for (int i = 0; i < 6; i++)
        {
            childTransform = Points.transform.GetChild(i);
            Vector3 pos = childTransform.transform.position;
            //positions += childTransform.name + ": " + childTransform.transform.position.ToString() + "\n";
            ref_coords[i] = pos;
            positions += childTransform.name + ": " + (pos.x * 1000).ToString("F3") + ", " + (pos.y * 1000).ToString("F3") + ", " + (pos.z * 1000).ToString("F3") + "\n";

            Debug.Log(positions);
        }

        Texts.text = positions;

    }

    public void Eucliodean()
    {
        //string distances = "Euclidean Distances:\n";
        float[] distances = new float[6];

        for (int i = 0; i < 6; i++)
        {
            init_coords[i].x *= 1000f;
            init_coords[i].y *= 1000f;
            init_coords[i].z *= 1000f;

            ref_coords[i].x *= 1000f;
            ref_coords[i].y *= 1000f;
            ref_coords[i].z *= 1000f;

            ;
            distances[i] = Vector3.Distance(init_coords[i], ref_coords[i]);
            //distances += "Point " + (i + 1) + ": " + distance.ToString("F3") + "\n";
        }

        float mean = distances.Average();
        float std = Mathf.Sqrt(distances.Select(d => Mathf.Pow(d - mean, 2)).Sum() / distances.Length);

        string output = "Euclidean Distances:\n";
        for (int i = 0; i < 6; i++)
        {
            output += "Point " + (i + 1) + ": " + distances[i].ToString("F3") + "\n";
        }
        output += "Mean: " + mean.ToString("F3") + "\n";
        output += "Std Dev: " + std.ToString("F3");
        Texts.text = output;
    }
}
