using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _progressRectComponent;
    private float _rotateSpeed = 200f;

    void Update()
    {
        _progressRectComponent.Rotate(0f,0f, _rotateSpeed * Time.deltaTime);
    }
}
