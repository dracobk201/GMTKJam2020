using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private FloatReference moveSpeed = null;
    [SerializeField] private FloatReference traslationSpeed = null;
    [SerializeField] private FloatReference horizontalCameraSensitivity = null;
    [SerializeField] private FloatReference verticalCameraSensitivity = null;
    [SerializeField] private FloatReference reachDistance = null;
    [SerializeField] private FloatReference horizontalAxis = null;
    [SerializeField] private FloatReference verticalAxis = null;
    [SerializeField] private FloatReference mouseHorizontalAxis = null;
    [SerializeField] private FloatReference mouseVerticalAxis = null;
    [SerializeField] private Camera playerCamera = null;

    private float _rotAroundX, rotAroundY;
    private Vector3 _targetLocation;
    private Rigidbody _playerRigidbody;


    private void Start()
    {
        _playerRigidbody = TryGetComponent(out Rigidbody rigidbodyResult) ? rigidbodyResult : new Rigidbody();
    }

    public void Move()
    {
        Vector3 targetPosition = _playerRigidbody.position + new Vector3(horizontalAxis.Value, 0, verticalAxis.Value) * moveSpeed.Value * Time.deltaTime;
        _playerRigidbody.MovePosition(targetPosition);
    }

    public void Rotate()
    {
        _rotAroundX += mouseHorizontalAxis.Value * horizontalCameraSensitivity.Value;
        rotAroundY += mouseVerticalAxis.Value * verticalCameraSensitivity.Value;

        CameraRotation();
    }

    private void CameraRotation()
    {
        _playerRigidbody.rotation = Quaternion.Euler(0, rotAroundY, 0);
        playerCamera.transform.rotation = Quaternion.Euler(-_rotAroundX, rotAroundY, 0);
    }

    public void Shoot()
    {
        StopAllCoroutines();
        if (AimNewLocation())
        {
            StartCoroutine(TranslatePlayer(_targetLocation));
        }
    }

    private bool AimNewLocation()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance.Value))
        {
            _targetLocation = hit.point;
            return true;
        }
        return false;
    }

    private IEnumerator TranslatePlayer(Vector3 targetPosition)
    {
        Vector3 heading = targetPosition - _playerRigidbody.position;
        float distance = heading.magnitude;
        float originalDistance = distance;

        while (distance > originalDistance * 0.1f)
        {
            _playerRigidbody.position = Vector3.MoveTowards(_playerRigidbody.position, targetPosition, traslationSpeed.Value * Time.deltaTime);
            yield return null;
            heading = targetPosition - _playerRigidbody.position;
            distance = heading.magnitude;
        }
    }
}
