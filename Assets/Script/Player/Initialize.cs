using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    public static Initialize Instance { get; private set; }
    public GameObject PlayerInstance { get; private set; }

    public GameObject border;

    public GameObject StartPoint;

    bool IsStart = true;

    public bool isPaused = false;

    [SerializeField] private HeartBar Heart;
    [SerializeField] private UI ui;
    [SerializeField] private BossControl boss;
    [SerializeField] private CameraChange cam;
    [SerializeField] private CinemachineCamera cCam;
    private BossControl bossCon;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void SetPlayerInstance(GameObject player)
    {
        PlayerInstance = player;
    }

    public GameObject GetPlayerInstance()
    {
        return PlayerInstance;
    }

    private void Start()
    {
        Heart = FindObjectOfType<HeartBar>();
        ui = FindObjectOfType<UI>();
        boss = FindObjectOfType<BossControl>();
        cam = FindObjectOfType<CameraChange>();
        cCam = FindObjectOfType<CinemachineCamera>();
        bossCon = FindObjectOfType<BossControl>();
        SpawnPlayer("SpawnPoint_T");
    }

    public void SpawnPlayer(string curMap)
    {
        GameObject PlayerPrefab = Resources.Load<GameObject>("Prefabs/Player");

        StartPoint = GameObject.Find(curMap);

        if (PlayerPrefab != null)
        {
            Debug.Log("Yes");

            PlayerInstance = Instantiate(PlayerPrefab, new Vector3(StartPoint.transform.position.x, StartPoint.transform.position.y), Quaternion.identity);
            cCam.PlayerSearch(PlayerInstance.transform);
            // 처음 시작했을땐 스폰시 IsStart에 따라 0번맵에 소환, 이후는 죽었다 살아나는 판정이니 IsStart가 false가 되어 1번맵에 소환
            cCam.change(IsStart? 0:1);

            IsStart = false;
        }
        else Debug.Log("No");

    }

    public void Death()
    {
        if (PlayerInstance != null)
        {
            Destroy(PlayerInstance);
            StartCoroutine(RespawnCoroutine());
            
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
        ReSpawn();
        ui.player = FindObjectOfType<Player>();
    }

    public void ReSpawn()
    {
        boss.BossStart = false;
        cam.ChangeMain();
        SpawnPlayer("SpawnPoint1");
        bossCon.PlayerAwake();
        ui.CamChange("Main");
        //Heart.Renewal();
    }
}
