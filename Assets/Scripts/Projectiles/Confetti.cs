using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        LaunchUp();
    }

    private void LaunchUp()
    {
        _rigidbody2D.AddRelativeForce(Vector2.up * speed);
    }

    private void FallDown()
    {

    }
}
