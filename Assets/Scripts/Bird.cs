using UnityEngine;

public class Bird : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;
    private bool _hasBeenLaunched;
    private bool _shouldFaceVelocityDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();

        _rb.bodyType = RigidbodyType2D.Kinematic; // Updated from isKinematic
        _circleCollider.enabled = false;
    }

    public void LaunchBird(Vector2 direction, float force)
    {
        _rb.bodyType = RigidbodyType2D.Dynamic; // Updated from isKinematic
        _circleCollider.enabled = true;

        _rb.AddForce(direction * force, ForceMode2D.Impulse);
        _hasBeenLaunched = true;
        _shouldFaceVelocityDirection = true;
    }

    void FixedUpdate()
    {
        if (_hasBeenLaunched && _shouldFaceVelocityDirection)
        {

            transform.right = _rb.linearVelocity;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _shouldFaceVelocityDirection = false;
    }
}
