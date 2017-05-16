using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DrainPoints : MonoBehaviour {
    private Image light; 
	// Use this for initialization
	void Awake () {
        light = transform.GetChild(0).GetChild(2).GetComponent<Image>();
	}
    
    public void DrainPoint(int number, int lastsum)
    {
        if(light == null)
            light = transform.GetChild(0).GetChild(2).GetComponent<Image>();
        StartCoroutine(Get_Point(number));
    }

    private IEnumerator Get_Point(int n)
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
            yield return new WaitForSeconds(0.5f);
        }

        gameObject.SetActive(false);

    }
}
