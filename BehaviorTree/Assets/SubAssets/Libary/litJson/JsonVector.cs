using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public static class JsonVector {

    public static string ToJson(this Vector3 v3) {
        JsonData data = new JsonData();
        data["x"] = v3.x;
        data["y"] = v3.y;
        data["z"] = v3.z;
        return JsonMapper.ToJson(data);
    }

    public static Vector3 GetVector3(JsonData json) {
        return new Vector3(float.Parse(json["x"].ToString()), float.Parse(json["y"].ToString()), float.Parse(json["z"].ToString()));
    }

    public static string ToJson(this Vector2 v2) {
        JsonData data = new JsonData();
        data["x"] = v2.x;
        data["y"] = v2.y;
        return data.ToJson();
    }

}
