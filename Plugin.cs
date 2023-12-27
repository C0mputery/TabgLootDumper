using BepInEx;
using Landfall.Network;
using System.IO;
using UnityEngine;

namespace TabgLootDumper
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            GameObject battleRoyaleGameObject = TABGGameModeObjectDataBase.Instance.GetGameModeObject(GameMode.BattleRoyale);

            string path = Path.Combine(Paths.GameRootPath, "BattleRoyaleThings.cubr"); // Computerys Ultimate Battle Royale file
            if (File.Exists(path)) { File.Delete(path); }
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                LootTAG[] lootPoints = battleRoyaleGameObject.GetComponentsInChildren<LootTAG>();
                writer.Write(lootPoints.Length);
                foreach (LootTAG lootTAG in lootPoints)
                {
                    Vector3 pos = lootTAG.gameObject.transform.position;
                    writer.Write(pos.x);
                    writer.Write(pos.y);
                    writer.Write(pos.z);
                }
                VehicleSpawn[] vehicles = battleRoyaleGameObject.GetComponentsInChildren<VehicleSpawn>();
                writer.Write(vehicles.Length);
                foreach (VehicleSpawn vehicle in vehicles)
                {
                    Vector3 pos = vehicle.gameObject.transform.position;
                    writer.Write(pos.x);
                    writer.Write(pos.y);
                    writer.Write(pos.z);
                    Quaternion rotation = vehicle.gameObject.transform.rotation;
                    writer.Write(rotation.x);
                    writer.Write(rotation.y);
                    writer.Write(rotation.z);
                }
                SpawnPointTAG[] spawnPoints = battleRoyaleGameObject.GetComponentsInChildren<SpawnPointTAG>();
                writer.Write(spawnPoints.Length);
                foreach (SpawnPointTAG spawnPointTAG in spawnPoints)
                {
                    Vector3 pos = spawnPointTAG.gameObject.transform.position;
                    writer.Write(pos.x);
                    writer.Write(pos.y);
                    writer.Write(pos.z);
                }
                var RingHotspots = battleRoyaleGameObject.GetComponentsInChildren<RingHotspot>();
                writer.Write(RingHotspots.Length);
                foreach (var RingHotspot in RingHotspots)
                {
                    Vector3 pos = RingHotspot.gameObject.transform.position;
                    writer.Write(pos.x);
                    writer.Write(pos.y);
                    writer.Write(pos.z);
                }
            }

            path = Path.Combine(Paths.GameRootPath, "NormalLootPreset.culp"); // Computerys Ultimate Loot Preset file
            if (File.Exists(path)) { File.Delete(path); }
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                LootTAG lootPoint = battleRoyaleGameObject.GetComponentInChildren<LootTAG>();
                LootPreset lootPreset = lootPoint.lootPreset;
                writer.Write(lootPreset.loot.Count);
                foreach (LootDropWrapper loot in lootPreset.loot)
                {
                    writer.Write(loot.m_loot.Length);
                    foreach (Loot Loot in loot.m_loot)
                    {
                        writer.Write(Loot.quanitity);
                        writer.Write(Loot.loot.GetComponentInChildren<Pickup>().m_itemIndex);
                    }
                    writer.Write(loot.spawnRate);
                }
            }
        }
    }
}