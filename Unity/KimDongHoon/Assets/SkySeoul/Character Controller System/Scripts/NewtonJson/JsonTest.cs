using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    public string jsonName;

    List<string> key = new List<string>();
    List<string> value = new List<string>();

    private string LoadJson()
    {
        var jsonPath = Resources.Load<TextAsset>($"JsonData/{jsonName}");
        
        if( jsonPath != null)
        {
            string JsonString = jsonPath.ToString();
            return JsonString;
        }
        else
        {
            return string.Empty;
        }
    }

    private void Start()
    {
        TestJson();
    }

    public void TestJson()
    {
        var jsonString = LoadJson();
        JArray jsonArr = JArray.Parse(jsonString);

        foreach (var obj in jsonArr)
        {
            Debug.Log($"level : {(string)obj["level"]}");
            Debug.Log($"hp : {(string)obj["hp"]}");
            Debug.Log($"mp : {(string)obj["mp"]}");
            Debug.Log($"attack : {(string)obj["attack"]}");
           
        }
    }
}

public class TestPlayerStat
{
    public int level;
    public int hp;
    public int mp;
    public int attack;
    public int defense;

    public Dictionary<int, TestPlayerStat> levelStat = new Dictionary<int, TestPlayerStat>();

    public IEnumerable<TestPlayerStat> levelStatOut => levelStat.OrderBy(item => item.Key).Select(item => item.Value);

    public TestPlayerStat(int level, int hp, int mp, int attack, int defense)
    {
        this.level = level;
        this.hp = hp;
        this.mp = mp;
        this.attack = attack;
        this.defense = defense;

        levelStat = new Dictionary<int, TestPlayerStat>();
    }
}
