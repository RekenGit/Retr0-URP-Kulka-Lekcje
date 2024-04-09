using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    private static GameManager _instance;
    public static GameManager Instance
    {
        get => _instance;
        private set
        {
            if (_instance == null) _instance = value;
            else if (_instance != value)
            {
                Debug.Log($"{nameof(GameManager)} instance already exists, destroing duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField] private GameObject lastCheckPoint;

    public void SetNewCheckPoint(GameObject checkpoint)
    {
        lastCheckPoint = checkpoint;
    }

    public GameObject GetLastCheckPoint()
    {
        return lastCheckPoint;
    }

    public Vector3 GetLastCheckPointPosition()
    {
        return lastCheckPoint.transform.position + new Vector3(0f, 3f, 0f);
    }

    private void Start()
    {
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
