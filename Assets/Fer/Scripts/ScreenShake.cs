using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    Camera MainCameraRef;

    [SerializeField]
    float shakeDur = 0f;
    [SerializeField]
    float shakeMag = 0.6f;
    [SerializeField]
    float damingSpeed = 1f;

    Vector3 initialPos;



    public void Awake()
    {
        instance = this;
        MainCameraRef = GetComponent<Camera>();
    }


    private void OnEnable()
    {
        initialPos = transform.localPosition;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (shakeDur > 0) 
        {
            transform.localPosition = initialPos + Random.insideUnitSphere * shakeMag;
            shakeDur -= Time.deltaTime * damingSpeed;
        }
        else
        {
            shakeDur = 0f;
            transform.localPosition = initialPos;
        }

        if (Input.GetMouseButton(0))
        {
            Shake();
        }

    }


    public void Shake()
    {
        shakeDur = .1f;
    }
}
