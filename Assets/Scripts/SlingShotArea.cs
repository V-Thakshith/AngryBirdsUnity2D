using UnityEngine;
using UnityEngine.InputSystem;
public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask _slingshotAreaMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isWithinSlingShotArea()
    {

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (Physics2D.OverlapPoint(worldPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
