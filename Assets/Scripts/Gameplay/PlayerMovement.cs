using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private BoolReference playerCaughtAMessage = null;
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

    private bool _havingACaughtableMessage;
    private float _rotAroundX, rotAroundY;
    private Vector3 _targetLocation;
    private Rigidbody _playerRigidbody;
    private GameObject _messageCaughted;
    private GameObject _messageToCaughted;

    private void Start()
    {
        _playerRigidbody = TryGetComponent(out Rigidbody rigidbodyResult) ? rigidbodyResult : new Rigidbody();
    }

    public void Move()
    {
        _playerRigidbody.position += _playerRigidbody.transform.forward * verticalAxis.Value * Time.deltaTime * moveSpeed.Value;
        _playerRigidbody.position += _playerRigidbody.transform.right * horizontalAxis.Value * Time.deltaTime * moveSpeed.Value;
    }

    public void Rotate()
    {
        _rotAroundX += mouseHorizontalAxis.Value * horizontalCameraSensitivity.Value;
        rotAroundY += mouseVerticalAxis.Value * verticalCameraSensitivity.Value;

        CameraRotation();
    }

    private void CameraRotation()
    {
        _playerRigidbody.rotation = Quaternion.Euler(-_rotAroundX, rotAroundY, 0);
        playerCamera.transform.rotation = Quaternion.Euler(-_rotAroundX, rotAroundY, 0);
    }

    public void ShootGrapplerHook()
    {
        StopAllCoroutines();
        if (AimNewLocation())
        {
            StartCoroutine(TranslatePlayer(_targetLocation));
        }
    }

    public void CaughtMessage()
    {
        if (_messageToCaughted != null)
        {
            playerCaughtAMessage.Value = true;
            _messageCaughted = _messageToCaughted;
        }
        else
        {
            playerCaughtAMessage.Value = false;
            _messageCaughted = null;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Global.MessageTag))
            _messageToCaughted = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Global.MessageTag))
            _messageToCaughted = null;
    }

}
