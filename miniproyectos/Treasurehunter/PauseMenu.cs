using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Root panel del menú")]
    public GameObject root; // arrastra aquí el Panel PauseMenu

    [Header("Botones (asigna en Inspector o se autoligan)")]
    public Button btnContinue;
    public Button btnSave;
    public Button btnLoad;
    public Button btnQuit;

    void Awake()
    {
        // Si no los asignaste, los buscamos por nombre dentro del panel
        if (!btnContinue) btnContinue = root.transform.Find("BtnContinue")?.GetComponent<Button>();
        if (!btnSave)     btnSave     = root.transform.Find("BtnSave")?.GetComponent<Button>();
        if (!btnLoad)     btnLoad     = root.transform.Find("BtnLoad")?.GetComponent<Button>();
        if (!btnQuit)     btnQuit     = root.transform.Find("BtnQuit")?.GetComponent<Button>();

        // Asignamos listeners
        if (btnContinue) btnContinue.onClick.AddListener(OnContinue);
        if (btnSave)     btnSave.onClick.AddListener(OnSave);
        if (btnLoad)     btnLoad.onClick.AddListener(OnLoad);
        if (btnQuit)     btnQuit.onClick.AddListener(OnQuit);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }

    public void Toggle()
    {
        bool show = !root.activeSelf;
        root.SetActive(show);
        Time.timeScale = show ? 0f : 1f;
    }

    public void OnContinue()
    {
        root.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnSave()
    {
        GameManager.I.SaveGame();
    }

    public void OnLoad()
    {
        bool ok = GameManager.I.LoadGame();
        if (ok)
        {
            root.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
