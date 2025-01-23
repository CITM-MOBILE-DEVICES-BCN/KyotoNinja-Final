using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class PersistenceManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    private string SaveFilePath => Application.dataPath + "/PlayerStats.data";

    private void Start()
    {
        playerStats.ResetStats();
        playerStats.filePath = SaveFilePath;
        LoadAndApply();
    }

    public async Task<PlayerSerializableData> LoadAsync()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning("No save file found.");
            return null;
        }

        return await Task.Run(() =>
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(SaveFilePath, FileMode.Open, FileAccess.Read))
            {
                return (PlayerSerializableData)formatter.Deserialize(stream);
            }
        });
    }

    public void LoadAndApply()
    {
        if (File.Exists(SaveFilePath))
        {
            FileStream fileStream = new FileStream(SaveFilePath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            PlayerSerializableData data = (PlayerSerializableData)bf.Deserialize(fileStream);
            fileStream.Close();

            ApplyLoadedStats(data);
        }
    }

    public async Task<bool> LoadAndApplyAsync()
    {
        PlayerSerializableData loadedStats = await LoadAsync();

        if (loadedStats == null)
            return false;

        ApplyLoadedStats(loadedStats);
        return true;
    }

    private void ApplyLoadedStats(PlayerSerializableData loadedStats)
    {
        playerStats.currency = loadedStats.serializedCurrency;
        for(int i = 0; i < playerStats.metaPowerUps.Count; i++)
        {
            playerStats.metaPowerUps[i].price = loadedStats.serializedMetaPowerUps[i].price;
            playerStats.metaPowerUps[i].level = loadedStats.serializedMetaPowerUps[i].level;
        }
        playerStats.CalculateStats();

        Debug.Log("PlayerStats loaded and applied successfully!");
    }
}
