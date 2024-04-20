using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class SpirteOrder : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] private Order order;
    Order beforeOrder;
    [SerializeField] string beforeSorder = "";

    public enum Order
    {
        None = -1,
        Start,
        Mid,
        End,
    }

    private void OnValidate()
    {
        if (order != beforeOrder)
        {
            beforeOrder = order;
            beforeSorder = order.ToString();
        }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Order newOrder = (Order)System.Enum.Parse(typeof(Order), beforeSorder);
        sr.sortingOrder = (int)newOrder;
    }

}
