using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questions
{
    public int response_code;
    public List<Question> results;

    public static Questions CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Questions>(jsonString);
    }
}
