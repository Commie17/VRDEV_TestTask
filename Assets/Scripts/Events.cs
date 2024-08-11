using UnityEngine;

public class Events : MonoBehaviour
{
    public delegate void OnSetVideo(ScObjVideo reference);
    public static OnSetVideo onSetVideo;
}
