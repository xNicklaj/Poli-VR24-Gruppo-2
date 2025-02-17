using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Triplet : MonoBehaviour
{
    [SerializeField][Range(0, 100)] private int probability;
    [SerializeField] private float speed;
    public TextMeshPro textMeshPro;
    [SerializeField] private Color color;

    private Color tColor;
    [SerializeField] private float colorVariance = 10;
    List<string> lettere = new List<string> { "A", "G", "T", "C" };
    void Start()
    {
        speed *= UnityEngine.Random.Range(0.75f,1.5f);
        textMeshPro = GetComponent<TextMeshPro>();
        if(UnityEngine.Random.Range(0,100)>=probability){
            GetComponent<CapsuleCollider>().enabled=true;
        }
        buildString();
    }

   
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, speed * Time.deltaTime);
    }
    void buildString()
    {
        tColor=new Color(
            color.r+UnityEngine.Random.Range(-colorVariance,colorVariance+1)/255,
            color.g+UnityEngine.Random.Range(-colorVariance,colorVariance+1)/255,
            color.b+UnityEngine.Random.Range(-colorVariance,colorVariance+1)/255,
            color.a+UnityEngine.Random.Range(-colorVariance,colorVariance+1)/255
        );

            print(color.r);
        string t =
            "<color=#" + tColor.ToHexString() + ">" +
            lettere[UnityEngine.Random.Range(0, 4)] +
            lettere[UnityEngine.Random.Range(0, 4)] +
            lettere[UnityEngine.Random.Range(0, 4)] +
            "</color>";
        textMeshPro.text = t;
    }
}
