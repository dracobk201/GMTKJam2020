using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Message")]
public class Message : ScriptableObject
{
    public string messageTitle;
    public MessageType type;
    [TextArea] public string messageContent;
    //public AudioClip clip;
}

public enum MessageType
{
    None, Bad, Regular, Good
}
