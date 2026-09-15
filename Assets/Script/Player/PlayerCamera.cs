using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerCamera : MonoBehaviour
{
    public Transform Character;
    public Scene OptionScene;
    public GameObject OptionCanvas;
    public SelectOption selectOption;
    public Player player;
    public CinemachineCamera cam;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayerInstance = Initialize.Instance.GetPlayerInstance();
        if (PlayerInstance != null)
        {
            Transform playerCameraRoot = PlayerInstance.transform.Find("CameraRoot");
            Character = PlayerInstance.transform;
        }
        else
        {
            Debug.LogError("Player miss");
        }

        player = FindObjectOfType<Player>();
        cam = FindObjectOfType<CinemachineCamera>();
        StartCoroutine(LoadOptionSceneAsync());

    }

    void Update()
    {
        if (Initialize.Instance != null)
        {
            if (Character != null) 
            {
                offset.x = Character.position.x;
                offset.y = Character.position.y;
                offset.z = -30;
                transform.position = offset;
            }
            else
            {
                UpdateCharacterReference();
            }

            OpenOption();
        }
    }

    
    private void UpdateCharacterReference()
    {
        GameObject PlayerInstance = Initialize.Instance.GetPlayerInstance();
        if (PlayerInstance != null)
        {
            Character = PlayerInstance.transform;
            cam.PlayerSearch(Character);
        }
        else
        {
            Debug.LogWarning("PlayerInstance를 찾을 수 없습니다. 리스폰 후 다시 확인됩니다.");
        }
    }

    private void OpenOption()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
        {
            Time.timeScale = 0;
            OptionCanvas.SetActive(true);
            selectOption.IsGameScence = true;
            Initialize.Instance.isPaused = true;
        }
    }
    private IEnumerator LoadOptionSceneAsync()
    {
        // LoadSceneAsync의 경우 AsyncOperation 타입
        UnityEngine.AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("OptionScene", LoadSceneMode.Additive);
        // 씬이 로드될 때까지 기다림
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        // 씬이 로드되면 SelectOption 스크립트가 적용된 오브젝트를 찾음
        OptionCanvas = GameObject.Find("SelectCanvas");
        if (OptionCanvas != null)
        {
            // SelectOption 스크립트를 가져옴
            selectOption = OptionCanvas.GetComponent<SelectOption>();

        }
        OptionCanvas.SetActive(false);
    }
}
