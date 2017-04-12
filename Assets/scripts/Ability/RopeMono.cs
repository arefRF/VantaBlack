using UnityEngine;
using System.Collections;

public class RopeMono : MonoBehaviour
{
    public static RopeMono instance { get; set; }
    
    void Start()
    {
        instance = this;
    }



}
