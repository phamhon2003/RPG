using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public WeaponController weaponController;
    public int Gold;
    public TextMeshProUGUI TextGold;
    public Additem UIadd;
    public WateringTool _WateringTool;
    [SerializeField] Transform Player; 
    private void Awake()
    {
        LoadData();
        Gold = PlayerPrefs.GetInt("Gold");
        SetTextGold();
        instance = this;      
    }
    void Start()
    {
        _WateringTool=GetComponent<WateringTool>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SaveData(Player.position);
        }
    }
    public void SetTextGold()
    {   
        if (TextGold != null) TextGold.SetText(Gold.ToString()+" G");
    }
    public void NotificationAdd(Item item)
    {   
        UIadd.gameObject.SetActive(true);
        UIadd.ShowMessage(item.image);
    }
    void saveGold()
    {
        PlayerPrefs.SetInt("Gold",Gold);
    }
    void OnDisable()
    {
        saveGold();
    }
    void OnApplicationQuit()
    {
        SaveData(Player.position);
        saveGold();
    }
    public void SaveData(Vector3 Pos)
    {
        PlayerPrefs.SetString("NextSceneName",LoadSceneStatic.nextSceneName);
        Debug.Log(LoadSceneStatic.nextSceneName);
        PlayerPrefs.SetFloat("PlayerPosX", Pos.x);
        PlayerPrefs.SetFloat("PlayerPosY", Pos.y);
        PlayerPrefs.SetFloat("PlayerPosZ", Pos.z);
        PlayerPrefs.Save(); 
    }
    public void LoadData()
    {
        if (PlayerPrefs.HasKey("NextSceneName"))
        {
            LoadSceneStatic.nextSceneName = PlayerPrefs.GetString("NextSceneName");
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            LoadSceneStatic.PosPlayer = new Vector3(x, y, z);
        }
        else
        {
            LoadSceneStatic.nextSceneName = "SampleScene";
        }
    }
}
