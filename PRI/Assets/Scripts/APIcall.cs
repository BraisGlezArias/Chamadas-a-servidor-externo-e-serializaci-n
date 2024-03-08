using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIcall : MonoBehaviour
{
    public Questions preguntas;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("https://opentdb.com/api.php?amount=10"));
    }

     IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    preguntas = Questions.CreateFromJSON(webRequest.downloadHandler.text);
                    Debug.Log("Número de preguntas solicitadas: " + preguntas.results.Count);
                    Debug.Log("Datos da primeira pregunta:");
                    Debug.Log("Tipo: " + preguntas.results[0].type);
                    Debug.Log("Dificultade: " + preguntas.results[0].difficulty);
                    Debug.Log("Categoría: " + preguntas.results[0].category);
                    Debug.Log("Pregunta: " + preguntas.results[0].question);
                    break;
            }
        }
    }
}
