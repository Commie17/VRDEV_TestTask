using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private GameObject _objectControlButton;
    [SerializeField] private GameObject _objectTitle;
    [SerializeField] private GameObject _objectTimeline;
    
    private MediaPlayer _mediaPlayer;
    private ScObjVideo _currentVideo;
    private Button _controlButton;
    private Slider _timeline;
    private Image _controlButtonImage;
    private float _videoTimelineCounter;
    private float _currentVideoDuration;
    private Text _title;
    
    public Sprite pause;
    public Sprite play;

    private void Awake()
    {
        Events.onSetVideo += ChangeVideo;
        _currentVideo = Resources.Load<ScObjVideo>("ScObjVideo/Video1");
        
        _mediaPlayer = GetComponent<MediaPlayer>();
        _mediaPlayer.Events.AddListener(OnVideoFinish);
        _mediaPlayer.Events.AddListener(OnVideoReady);
        
        _controlButton = _objectControlButton.GetComponent<Button>();
        _controlButtonImage = _objectControlButton.GetComponent<Image>();
        
        _title = _objectTitle.GetComponentInChildren<Text>();
        
        _timeline = _objectTimeline.GetComponent<Slider>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetVideo();
        
        _controlButtonImage.sprite = play;
        _controlButton.onClick.AddListener(ControlButton);
    }

    private void Update()
    {
        if(_mediaPlayer.Control.IsPlaying())
            SetVideoTimeline();
    }

    private void OnEnable()
    {
        Events.onSetVideo += ChangeVideo;
        _mediaPlayer.Events.AddListener(OnVideoFinish);
        _mediaPlayer.Events.AddListener(OnVideoReady);
    }

    private void OnDisable()
    {
        Events.onSetVideo -= ChangeVideo;
        _mediaPlayer.Events.RemoveListener(OnVideoFinish);
        _mediaPlayer.Events.RemoveListener(OnVideoReady);
    }

    private void ChangeVideo(ScObjVideo _newVideo)
    {
        if (_newVideo != _currentVideo)
        {
            _currentVideo = _newVideo;
            SetVideo();
        }
    }

    private void SetVideo()
    {
        _mediaPlayer.Stop();
        _controlButtonImage.sprite = play;
        
        _mediaPlayer.OpenMedia(new MediaPath(_currentVideo.referenceVideo, MediaPathType.AbsolutePathOrURL),
            autoPlay: false);

        _title.text = _currentVideo.episodeName;
    }

    private void ControlButton()
    {
        if (_mediaPlayer.Control.IsPaused() || _mediaPlayer.Control.IsFinished())
        {
            _mediaPlayer.Play();
            _controlButtonImage.sprite = pause;
        }
        
        else if (_mediaPlayer.Control.IsPlaying())
        {
            _mediaPlayer.Stop();
            _controlButtonImage.sprite = play;
        }
    }

    private void OnVideoFinish(MediaPlayer mp, MediaPlayerEvent.EventType eventType, ErrorCode code)
    {
        if (eventType == MediaPlayerEvent.EventType.FinishedPlaying)
            _controlButtonImage.sprite = play;
    }

    private void OnVideoReady(MediaPlayer mp, MediaPlayerEvent.EventType eventType, ErrorCode code)
    {
        if (eventType == MediaPlayerEvent.EventType.Started)
        {
            _videoTimelineCounter = 0f;
            _currentVideoDuration = (float)_mediaPlayer.Info.GetDuration();
        }
    }

    private void SetVideoTimeline()
    {
        _videoTimelineCounter += Time.deltaTime;
        _timeline.value = (1000f * _videoTimelineCounter) / (1000f * _currentVideoDuration);
    }

    private void OnApplicationQuit()
    {
        Events.onSetVideo -= ChangeVideo;
        _mediaPlayer.Events.RemoveListener(OnVideoFinish);
        _mediaPlayer.Events.RemoveListener(OnVideoReady);
    }
}
