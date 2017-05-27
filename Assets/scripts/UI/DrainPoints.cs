using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DrainPoints : MonoBehaviour {
    private Image light;
    private Text text;
	// Use this for initialization
	void Awake () {
        light = transform.GetChild(0).GetChild(2).GetComponent<Image>();
        text = transform.GetChild(0).GetChild(4).GetComponent<Text>();
    }
    
    public void DrainPoint(int number, int lastsum)
    {
        if (light == null)
        {
            light = transform.GetChild(0).GetChild(2).GetComponent<Image>();
            text = transform.GetChild(0).GetChild(4).GetComponent<Text>();
        }
        text.text = lastsum.ToString();
        StartCoroutine(Get_Point(number,lastsum));
    }

    private IEnumerator Get_Point(int n,int lastsum)
    {
        yield return new WaitForSeconds(1f);
        string path = "HUD\\Achivement\\";
        int i = 1;
        while (i<=n)
        {
            string point = path + "F" + i.ToString();
            light.sprite = Resources.Load<Sprite>(point);
            light.SetNativeSize();
            i++;
            yield return new WaitForSeconds(0.3f);
        }

        text.text = (lastsum + n).ToString();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);

    }
}
