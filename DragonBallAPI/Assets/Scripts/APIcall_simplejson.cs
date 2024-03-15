/*

Código de exemplo de parseo de API con simple JSON. 

Simple JSON é unha librería externa que permite acceder de forma dinámica e sen crear as clases que representan o obxecto json.
https://github.com/Bunny83/SimpleJSON

Para usar a clase, hai este código de exemplo. Asegúrate de descargar o código simplejson.cs e incluilo no proxecto (como está feito neste exemplo).

*/


using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections; 


public class APIConsumer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetDataCoroutine());
    }

    IEnumerator GetDataCoroutine()
    {
        string apiUrl = "https://opentdb.com/api.php?amount=10";
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var N = JSON.Parse(request.downloadHandler.text);
            Debug.Log(N.ToString());
            // Now you can access your data dynamically. For example:
            string value = N["results"][0]["type"].Value; // Assuming 'key' exists in your JSON
            Debug.Log(value);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}
