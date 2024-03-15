using System;
using SimpleJSON;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{

    public GameObject image1;
    public GameObject image2;
    public GameObject name1;
    public GameObject name2;
    private long powerLevel1;
    private long powerLevel2;
    public GameObject textResult;
    public GameObject buttonNext;
    public GameObject hitText;
    public GameObject missText;
    public GameObject restart;
    public AudioClip win;
    public AudioClip lost;
    private bool next;
    private bool result;
    private bool draw;
    private int hits;
    private int misses;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetDataCoroutine(image1, name1, 1));
        StartCoroutine(GetDataCoroutine(image2, name2, 2));
        next = false;
        draw = false;
        hits = 0;
        misses = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (next) {
            if (!draw) {
                if (result) {
                    textResult.GetComponent<TMP_Text>().text = "You won!";
                } else {
                    textResult.GetComponent<TMP_Text>().text = "You lost!";
                }
            } else {
                textResult.GetComponent<TMP_Text>().text = "Draw!";
            }
            textResult.SetActive(true);
            buttonNext.SetActive(true);
        } else {
            textResult.SetActive(false);
            buttonNext.SetActive(false);
            name1.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            name2.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }

        hitText.GetComponent<TMP_Text>().text = "Hits: " + hits;
        missText.GetComponent<TMP_Text>().text = "Misses: " + misses;
    }

    IEnumerator GetDataCoroutine(GameObject image, GameObject name, int character)
    {
        string apiUrl = "https://dragonball-api.com/api/characters?limit=48";
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var N = JSON.Parse(request.downloadHandler.text);
            // Now you can access your data dynamically. For example:
            int rand = UnityEngine.Random.Range(0, N["items"].Count);
            StartCoroutine(obtainCharacter(N, image, name, rand));
            DefinePowerLevel(N, rand, character);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    public IEnumerator obtainCharacter(SimpleJSON.JSONNode N, GameObject img, GameObject name, int rand) {
        string nombre = N["items"][rand]["name"];
        string image = N["items"][rand]["image"];
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(image.Replace(".webp", ".jpg"));
        //chara.ki = (chara.ki).Replace(".", "");
        yield return request.SendWebRequest();
        Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        Sprite newSprite = SpriteFromTexture2D(myTexture);
        img.GetComponent<UnityEngine.UI.Image>().sprite = newSprite;
        name.GetComponent<TMP_Text>().text = nombre;
    }

    public void DefinePowerLevel(SimpleJSON.JSONNode N, int rand, int character) {
        string powerLevel = N["items"][rand]["maxKi"];
        powerLevel =  powerLevel.Replace(".", "");
        powerLevel =  powerLevel.Replace(",", "");
        if (powerLevel.Contains(" ")) {
            switch(powerLevel.Split(" ")[1].ToLower()) {
                case "googolplex":
                    powerLevel = powerLevel.Split(" ")[0] + "00000000000000";
                    break;
                case "septllion":
                    powerLevel = powerLevel.Split(" ")[0] + "0000000000000";
                    break;
                case "septillion":
                    powerLevel = powerLevel.Split(" ")[0] + "0000000000000";
                    break;
                case "sextillion":
                    powerLevel = powerLevel.Split(" ")[0] + "000000000000";
                    break;
                case "quintillion":
                    powerLevel = powerLevel.Split(" ")[0] + "00000000000";
                    break;
                case "trillion":
                    powerLevel = powerLevel.Split(" ")[0] + "0000000000";
                    break;
                case "billion":
                    powerLevel = powerLevel.Split(" ")[0] + "000000000";
                    break;
                default:
                    break;
            }
            if (character == 1) {
                powerLevel1 = Int64.Parse(powerLevel);
            } else if (character == 2){
                powerLevel2 = Int64.Parse(powerLevel);
            }
        } else {
            if (character == 1) {
                powerLevel1 = Int32.Parse(powerLevel);
            } else if (character == 2){
                powerLevel2 = Int32.Parse(powerLevel);
            }
        }
    }

    public void clickButton1() {
        if (!next) {
            result = powerLevel1 > powerLevel2;
            if (powerLevel1 == powerLevel2) {
                draw = true;
            } else {
                draw = false;
            }
            if (!draw) {
                if (result) {
                    name2.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                    name1.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                    hits += 1;
                    GetComponent<AudioSource>().clip = win;
                    GetComponent<AudioSource>().Play();
                } else {
                    name2.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                    name1.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                    misses += 1;
                    GetComponent<AudioSource>().clip = lost;
                    GetComponent<AudioSource>().Play();
                }
            } else {
                name2.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
                name1.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
            }
        }
        next = true;
    }

    public void ClickButton2() {
        if (!next) {
            result = powerLevel2 > powerLevel1;
            if (powerLevel1 == powerLevel2) {
                draw = true;
            } else {
                draw = false;
            }
            if (!draw) {
                if (result) {
                    name1.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                    name2.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                    hits += 1;
                    GetComponent<AudioSource>().clip = win;
                    GetComponent<AudioSource>().Play();
                } else {
                    name1.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                    name2.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                    misses += 1;
                    GetComponent<AudioSource>().clip = lost;
                    GetComponent<AudioSource>().Play();
                }
            } else {
                name1.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
                name2.transform.parent.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
            }
        }
        next = true;
    }

    public void Replay() {
        StartCoroutine(GetDataCoroutine(image1, name1, 1));
        StartCoroutine(GetDataCoroutine(image2, name2, 2));
        next = false;
    }

    public void Restart() {
        hits = 0;
        misses = 0;
        next = false;
        StartCoroutine(GetDataCoroutine(image1, name1, 1));
        StartCoroutine(GetDataCoroutine(image2, name2, 2));
    }

    Sprite SpriteFromTexture2D(Texture2D texture) {
		return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
	}
}
