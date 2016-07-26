using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The movement speed.")] [SerializeField] public float Speed;
    [Tooltip("The text UI element.")] [SerializeField] private Text _text;
    [Tooltip("The world.")] [SerializeField] private Procedural2DTileWorld.World _world;
    private string _message;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        _message = _text.text;
    }


    // Update is called once per frame
    public void Update()
    {
        var pos = transform.position;
        pos.x += Input.GetAxis("Horizontal")*Speed;
        pos.y += Input.GetAxis("Vertical")*Speed;
        transform.position = pos;

        _text.text = string.Format(_message,
            Mathf.FloorToInt(transform.position.x),
            Mathf.FloorToInt(transform.position.y),
            Mathf.FloorToInt(_world.CurrentPosition.x),
            Mathf.FloorToInt(_world.CurrentPosition.y));
    }
}