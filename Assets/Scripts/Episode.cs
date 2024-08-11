using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Episode : MonoBehaviour
{    
    [SerializeField] private Texture2D _intro;
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private GameObject _buttonObject;
    [SerializeField] private string _videoNumber;
    
    private Button _playButton;
    
    public ScObjVideo _scObjVideo;
    
    private void Awake()
    {
        _playButton = _buttonObject.GetComponent<Button>();
        _playButton.onClick.AddListener(Play);
        
        _scObjVideo = Resources.Load<ScObjVideo>("ScObjVideo/Video" + _videoNumber);
        
        _loadScreen.SetActive(true);
    }

    private void Start()
    {
        SetIntro();
    }

    private void Play()
    {
        Events.onSetVideo?.Invoke(_scObjVideo);
    }

    private async Task<Texture2D> LoadIntro()
    {
        var request = UnityWebRequestTexture.GetTexture(_scObjVideo.referenceIntro);
        Texture2D texture;
        
        request.SendWebRequest();

        while (!request.isDone)
        {
            await Task.Yield();
        }
        
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            request.Dispose();
            texture = null;
            
            return texture;
        }
        
        texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        request.Dispose();
        
        return texture;
    }

    private async void GetIntroFromServer()
    {
        Texture2D texture = await LoadIntro();
        
        while (texture == null && Application.isPlaying)
        {
            texture = await LoadIntro();
        }

        if (Application.isPlaying)
        {
            File.WriteAllBytes(
                Application.dataPath + "/Resources/Intro/Video" + _videoNumber + "/IntroForVideo" + _videoNumber +
                ".jpg", texture.EncodeToJPG());
            GetComponent<RawImage>().texture = texture;
            
            _loadScreen.SetActive(false);
        }
    }

    private void SetIntro()
    {
        _intro = Resources.Load<Texture2D>("Intro/Video" + _videoNumber + "/IntroForVideo" + _videoNumber);

        if (_intro == null)
        {
            GetIntroFromServer();
        }
        else
        {
            GetComponent<RawImage>().texture = _intro;
            _loadScreen.SetActive(false);
        }
    }
}
