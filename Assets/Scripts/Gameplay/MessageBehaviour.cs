using System.Collections;
using UnityEngine;

public class MessageBehaviour : MonoBehaviour
{
    [SerializeField] private BoolReference playerCaughtAMessage = null;
    [SerializeField] private FloatReference moveSpeed = null;
    [SerializeField] private FloatReference timeToPivot = null;
    [SerializeField] private MessageRuntimeSet messageSet = null;
    [SerializeField] private Message messageContent = null;

    private bool _isCaughted;
    private Rigidbody _messageRigidbody;

    private void Start()
    {
        messageSet.Add(this);
        _messageRigidbody = TryGetComponent(out Rigidbody rigidbodyResult) ? rigidbodyResult : gameObject.AddComponent<Rigidbody>();
        StartCoroutine(FloatyBehaviour());
    }

    private IEnumerator FloatyBehaviour()
    {
        float actualSpeed;
        switch (messageContent.type)
        {
            case MessageType.Bad:
                actualSpeed = moveSpeed.Value * 2;
                break;
            case MessageType.Good:
                actualSpeed = moveSpeed.Value / 2;
                break;
            case MessageType.None:
            case MessageType.Regular:
            default:
                actualSpeed = moveSpeed.Value;
                break;
        }

        while (!_isCaughted)
        {
            var randomX = Random.Range(-actualSpeed, actualSpeed);
            var randomY = Random.Range(-actualSpeed, actualSpeed);
            var randomZ = Random.Range(-actualSpeed, actualSpeed);
            var randomFactor = new Vector3(randomX, randomY, randomZ);

            Vector3 targetPosition = _messageRigidbody.position + randomFactor;
            Vector3 heading = targetPosition - _messageRigidbody.position;
            float distance = heading.magnitude;
            float originalDistance = distance;

            while (distance > originalDistance * 0.1f)
            {
                _messageRigidbody.position = Vector3.MoveTowards(_messageRigidbody.position, targetPosition, actualSpeed * 2 * Time.deltaTime);
                yield return null;
                heading = targetPosition - _messageRigidbody.position;
                distance = heading.magnitude;
            }
            yield return new WaitForSeconds(timeToPivot.Value);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Global.PlayerTag) && playerCaughtAMessage.Value && !_isCaughted)
            _isCaughted = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Global.PlayerTag))
            _isCaughted = false;
    }
}
