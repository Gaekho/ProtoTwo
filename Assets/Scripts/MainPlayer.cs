using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto2.Enums
{
    public enum MoveDirection
    {
        Left,
        Right
    }
}

public class MainPlayer : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private float MoveSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            Move(MoveDirection.Left);
        }
        if(Input.GetKey(KeyCode.D))
        {
            Move(MoveDirection.Right);
        }

        //Debug.Log(Time.deltaTime);
    }

    private void Move(MoveDirection direction)
    {
        if(direction == MoveDirection.Left)
        {
            transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
            //transform.localPosition += Vector3.left * MoveSpeed * Time.deltaTime;
        }
        if(direction == MoveDirection.Right)
        {
            transform.position += Vector3.right * MoveSpeed * Time.deltaTime;
            //transform.localPosition += Vector3.right * MoveSpeed * Time.deltaTime;
        }
    }
}
