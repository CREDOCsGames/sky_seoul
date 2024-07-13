using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterInfo : MonoBehaviour
{
    Dictionary<string, ulong[]> Monsters = new Dictionary<string, ulong[]>();

    private void Awake()
    {
        var path = Resources.Load<TextAsset>("MonsterSpawn");
        if (path == null) return;
        JObject jsonObj = JObject.Parse(path.text);
        LoadJson(jsonObj);

        foreach (var item in Monsters)
        {
            foreach (var item2 in item.Value)
                Debug.Log($"{item.Key} : {item2}");
        }
    }

    public void LoadJson(JObject jsonObj)
    {
        Monsters = ((JArray)jsonObj["spawnMonsterInfo"])
            .Select(p => new KeyValuePair<string, ulong[]>(
                p["spawnType"].ToObject<string>(), p["monsterPrefabID"].ToObject<ulong[]>())
            ).ToDictionary(p => p.Key, item => item.Value);
    }

}
