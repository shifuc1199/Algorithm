using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int[] array = new int[] { 6, 1, 2, 7, 9, 3, 4, 5, 10, 8 };
        QuickSort(ref array, 0, array.Length-1);
        string s="";
        foreach (var item in array)
        {
            s += item + ",";
        }
        Debug.Log(s);
    }
    public void QuickSort(ref int[] array,int low,int high)
    {
        int temp = array[low];
        int i = low, j = high;
        while(i<j)
        {
            while(array[j]>=temp && i < j)
            {
                j--;
                
            }
            if (i < j)
            {
                array[i] = array[j];
                i++;
            }

            while (array[i] <= temp&&i<j)
            {
                low++;
            }
            if (i < j)
            {
                array[j] = array[i];
                j--;
            }
        }
        array[i] = temp;
        QuickSort(ref array, low , i - 1);
        QuickSort(ref array, i+1, high);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
