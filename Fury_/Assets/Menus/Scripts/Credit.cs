using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Credit : MonoBehaviour
{
    public GUISkin creditSkin;
    public float creditSpeed;
    private TextReader tr;
    private string path;
    private List<string> credits = new List<string>();
    private List<Rect> positionRect = new List<Rect>();

    // Use this for initialization
    void Start()
    {
        // Set the path for the credits.txt file
        path = "Assets/Menus/Resources/Credits.txt";
        // Create reader & open file
        tr = new StreamReader(path);
        string temp;
        int count = 0;
        while ((temp = tr.ReadLine()) != null)
        {
            // Read a line of text
            credits.Add(temp);
            positionRect.Add(new Rect(200, 790 + (30 * count), 300, 100));
            Debug.Log(temp);
            count++;
        }
        // Close the stream
        tr.Close();
    }
    // Update is called once per frame
    void OnGUI()
    {
        GUI.skin = creditSkin;
        for (int i = 0; i < credits.Count; i++)
        {
            GUI.Label(positionRect[i], credits[i], "item");
            Rect tempRect = positionRect[i];
            tempRect.y = tempRect.y - creditSpeed;
            positionRect[i] = tempRect;
        }
    }
}