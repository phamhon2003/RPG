using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]
public class PlacedObjectData
{
    public string prefabName; // Tên prefab dùng để Instantiate
    public Vector2 position;  // Vị trí trong thế giới
    public string uniqueID;   // ID duy nhất để phân biệt từng object
    // Thêm các trạng thái đặc biệt khác nếu có
    public float grown;
}
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private HashSet<string> collectedIds = new HashSet<string>();

    public List<GameObject> spawnablePrefabs;
    public List<PlacedObjectData> placedObjects = new();
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            Destroy(gameObject);
        }
        if (LoadSceneStatic.nextSceneName == "SampleScene")
        {
            LoadData();
            LoadDataInstatiate();
        }
        Debug.Log( placedObjects.Count);
    }
    public void RegisterCollected(string id)
    {
        collectedIds.Add(id);
        SaveData();
    }

    public bool IsCollected(string id)
    {
        return collectedIds.Contains(id);
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(new SaveWrapper { collected = new List<string>(collectedIds) });
        PlayerPrefs.SetString("CollectedData", json);
    }

    public void LoadData()
    {
        string json = PlayerPrefs.GetString("CollectedData", "");
        if (!string.IsNullOrEmpty(json))
        {
            SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);
            collectedIds = new HashSet<string>(wrapper.collected);
        }
    }

    [System.Serializable]
    private class SaveWrapper
    {
        public List<string> collected;
    }

    public void SaveDataInstatiate()
    {
        string json = JsonUtility.ToJson(new PlacedObjectListWrapper { list = placedObjects }, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Game Saved to: " + SavePath);
    }

    public void LoadDataInstatiate()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            PlacedObjectListWrapper wrapper = JsonUtility.FromJson<PlacedObjectListWrapper>(json);
            placedObjects = wrapper.list;

            // Instantiate lại tất cả object đã lưu
            foreach (PlacedObjectData data in placedObjects)
            {
                GameObject prefab = spawnablePrefabs.FirstOrDefault(p => p.name == data.prefabName);
               // Debug.Log(prefab.name+"1122");
                if (prefab != null)
                {
                    GameObject instance = Instantiate(prefab, data.position, Quaternion.identity);
                    if (instance.GetComponent<Chest>() != null) instance.GetComponent<Chest>().chestID = data.uniqueID;
                    if (instance.GetComponent<Cow>() != null)
                    {
                        instance.GetComponent<Cow>().CowID = data.uniqueID;
                        instance.GetComponent<Cow>().Grown = data.grown;
                    }
                    if (instance.GetComponent<Seeds>() != null) instance.GetComponent<Seeds>().Grown = data.grown;
                }
            }

            Debug.Log("Game Loaded from: " + SavePath);
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }
    public void RemovePlacedObjectByPosition(Vector2 position)
    {
        placedObjects.RemoveAll(obj => Vector2.Distance(obj.position, position) < 0.1f);
    }
    public void UpdateSeedData(PlacedObjectData data)
    {
        foreach (PlacedObjectData objdata in SaveManager.Instance.placedObjects)
        {
            if (objdata.position == data.position)
            {
                objdata.grown = data.grown;
            }
        }
    }
    [System.Serializable]
    private class PlacedObjectListWrapper
    {
        public List<PlacedObjectData> list;
    }
}
