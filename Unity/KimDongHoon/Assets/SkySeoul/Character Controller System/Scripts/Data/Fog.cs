using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[JsonObject]
public class Fog
{
    [System.Serializable]
    public class FogColor
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }

    [JsonProperty("BossId")]
    public ulong bossMonsterId;
    [JsonProperty]
    public bool enabled;
    [JsonProperty]
    public FogColor color;
    [JsonProperty]
    public string mode;
    [JsonProperty]
    public int Start;
    [JsonProperty]
    public int End;
    [JsonProperty]
    public float HaloStrength;
    [JsonProperty]
    public float FlareFadeSpeed;
    [JsonProperty]
    public float Strength;
}
