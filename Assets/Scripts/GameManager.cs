using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public GameObject m_PlayerShipPrefab;
    public TextMeshProUGUI m_UIText;
    public GameObject mainCamera;
    public HUDController HUDController;

    PlayerShip playerShip;
    GameAnnouncer announce;

    bool requestTitleScreen = true;

    private void Awake()
    {
        SingletonInstanceGuard();
        announce = GameAnnouncer.AnnounceTo(Announcer.TextComponent(m_UIText), Announcer.Log(this));
    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    private void SingletonInstanceGuard()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("Only one instance of singleton allowed.");
        }
    }

    IEnumerator GameLoop()
    {
        NewGame();
        while (true)
        {
            if (requestTitleScreen)
            {
                requestTitleScreen = false;
                yield return StartCoroutine(ShowTitleScreen());
            }
            yield return StartCoroutine(GamePlay());
            yield return StartCoroutine(GameEnd());
        }
    }

    IEnumerator ShowTitleScreen()
    {
        announce.Title();
        HUDController.gameObject.SetActive(false);
        while (!Input.anyKeyDown) yield return null;
    }

    IEnumerator GamePlay()
    {
        HUDController.gameObject.SetActive(true);
        playerShip = PlayerShip.Spawn(m_PlayerShipPrefab);
        playerShip.EnableControls();
        mainCamera.GetComponent<CameraFollow>().AttachToPlayer();
        announce.ClearAnnouncements();
        while (playerShip.IsAlive) yield return null;
    }

    IEnumerator GameEnd()
    {
        bool gameover = !playerShip.IsAlive;
        if (gameover)
        {
            announce.GameOver();
            yield return Pause.Brief();
            announce.ClearAnnouncements();
            HUDController.gameObject.SetActive(false);
            NewGame();
        }
        yield return Pause.Long();
    }

    void NewGame()
    {
        requestTitleScreen = true;
    }

    public static class Pause
    {
        public static WaitForSeconds Long()
        {
            return new WaitForSeconds(2f);
        }

        public static WaitForSeconds Brief()
        {
            return new WaitForSeconds(1f);
        }
    }
}
