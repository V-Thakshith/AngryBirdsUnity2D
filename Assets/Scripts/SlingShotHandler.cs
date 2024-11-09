using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SlingShotHandler : MonoBehaviour
{
    [Header("Line renderer's")]
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;

    [SerializeField] private Transform _leftStartPosition;
    [SerializeField] private Transform _rightStartPosition;
    [SerializeField] private Transform _centerStartPosition;
    [SerializeField] private Transform _idleStartPosition;

    [SerializeField] private SlingShotArea _slingShotArea;

    private float _maxDistance = 3.5f;
    private float _shotForce = 6f;

    private Vector2 _slingShotLinesPosition;

    private bool _clickedWithinArea;

    [SerializeField] private Bird _birdPreFab;
    [SerializeField] private float _birdPositionOffset = 0.275f;

    [SerializeField] private Transform _elasticTransform;
    [SerializeField] private CameraManager _cameraManager;

    private Bird _spawnedBird;

    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool birdOnSlingShot;

    private float _timeBetweenBirdRespawns = 2f;
    private void Awake()
    {
        // Set the position count for both line renderers to 2
        _leftLineRenderer.positionCount = 2;
        _rightLineRenderer.positionCount = 2;

        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;

        //SpawningBird();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && _slingShotArea.isWithinSlingShotArea())
        {
            SpawningBird();
            _clickedWithinArea = true;

            _cameraManager.SwitchFollowCamera(_spawnedBird.transform);
        }

        if (Mouse.current.leftButton.isPressed && _clickedWithinArea)
        {

            DrawSlingShot();
            PositionAndRotateBird();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _clickedWithinArea)
        {
            if (GameManager.instance.HasEnoughShots())
            {
                _clickedWithinArea = false;
                _spawnedBird.LaunchBird(_direction, _shotForce);
                GameManager.instance.UseShot();
                AnimateSlingShot();
                //birdOnSlingShot = false;
                if (GameManager.instance.HasEnoughShots())
                {
                    StartCoroutine(SpawnBirdAfterTime());

                }
            }
        }
    }

    private void DrawSlingShot()
    {

        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        touchPosition.z = 0; // Optional: Set to zero if working in 2D

        _slingShotLinesPosition = _centerStartPosition.position + Vector3.ClampMagnitude(touchPosition - _centerStartPosition.position, _maxDistance);
        SetLines(_slingShotLinesPosition);

        _direction = (Vector2)_centerStartPosition.position - _slingShotLinesPosition;
        _directionNormalized = _direction.normalized;
    }

    private void SetLines(Vector2 position)
    {

        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled)
        {
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }

        _leftLineRenderer.SetPosition(0, position);
        _leftLineRenderer.SetPosition(1, _leftStartPosition.position);

        _rightLineRenderer.SetPosition(0, position);
        _rightLineRenderer.SetPosition(1, _rightStartPosition.position);
    }

    private void SpawningBird()
    {
        SetLines(_idleStartPosition.position);
        Vector2 dir = (_centerStartPosition.position - _idleStartPosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_idleStartPosition.position + dir * _birdPositionOffset;
        _spawnedBird = Instantiate(_birdPreFab, spawnPosition, Quaternion.identity);
        _spawnedBird.transform.right = dir;

    }

    private void PositionAndRotateBird()
    {
        _spawnedBird.transform.position = _slingShotLinesPosition + _directionNormalized * _birdPositionOffset;
        _spawnedBird.transform.right = _directionNormalized;
    }

    private IEnumerator SpawnBirdAfterTime()
    {
        yield return new WaitForSeconds(_timeBetweenBirdRespawns);

        _cameraManager.SwitchToIdleCamera();

    }

    private void AnimateSlingShot()
    {
        _elasticTransform.position = _leftLineRenderer.GetPosition(0);
        float dist = Vector2.Distance(_elasticTransform.position, _centerStartPosition.position);
        float time = dist / 1.2f;
        _elasticTransform.DOMove(_centerStartPosition.position, time).SetEase(Ease.OutElastic);
        StartCoroutine(AnimateSlingShotLines(_elasticTransform, time));
    }

    private IEnumerator AnimateSlingShotLines(Transform trans, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            SetLines(trans.position);
            yield return null;
        }

    }
}
