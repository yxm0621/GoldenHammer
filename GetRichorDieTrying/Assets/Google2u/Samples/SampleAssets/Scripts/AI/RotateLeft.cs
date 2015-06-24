using UnityEngine;
using System.Collections;

public class RotateLeft : MonoBehaviour
{
    private Transform _MyTransform;
    private const float _MyRotationRate = -10.0f;
    public float Speed = 1.0f;
    // Use this for initialization
	void Start ()
	{
	    _MyTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        var frameRotation = Time.deltaTime * _MyRotationRate * Speed;
        _MyTransform.Rotate(0, frameRotation, 0);
	}
}
