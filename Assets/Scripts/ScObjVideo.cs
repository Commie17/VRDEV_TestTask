using UnityEngine;

[CreateAssetMenu(fileName = "VideoObj", menuName = "Scriptable Objects/Video Object", order = 1)]
public class ScObjVideo : ScriptableObject
{
    public string referenceVideo;
    public string referenceIntro;
    public string episodeName;
}