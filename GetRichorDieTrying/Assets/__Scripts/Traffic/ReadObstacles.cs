using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ReadObstacles : MonoBehaviour {
    public FileInfo theSourceFile = null;
    public StreamReader reader = null;
    public string text = " "; // assigned to allow first line to be read below
 
    void Start () {
        theSourceFile = new FileInfo(@"D:\GitHub\GoldenHammer\GetRichorDieTrying\Assets\__Scripts\data\Obstacles.txt");
        reader = theSourceFile.OpenText();
        //StreamReader sr1 = new StreamReader(@"D:\GitHub\GoldenHammer\GetRichorDieTrying\Assets\__Scripts\data\Obstacles.txt"); 
    }
   
    void Update () {
        if (text != null) {
            text = reader.ReadLine();
            //Console.WriteLine(text);
            print (text);
            //Debug.Log(text);
        }
    }
}
