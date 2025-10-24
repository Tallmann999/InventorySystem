using UnityEngine;

public class TestPlayerMover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    public float runSpeed = 5f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
       Mover();
    }

    private void Mover()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        _rigidbody2D.linearVelocity = new Vector2(xDirection* runSpeed, yDirection* runSpeed);
    }
}
