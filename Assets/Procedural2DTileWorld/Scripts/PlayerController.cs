using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The movement speed.")] [SerializeField] public float Speed;

    // Update is called once per frame
    public void Update()
    {
        var pos = transform.position;
        pos.x += Input.GetAxis("Horizontal")*Speed;
        pos.y += Input.GetAxis("Vertical")*Speed;
        transform.position = pos;
    }
}