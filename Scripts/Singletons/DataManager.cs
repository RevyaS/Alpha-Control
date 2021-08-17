using Godot;
using System;
using GC = Godot.Collections;

/*  Handles anything related to data, including usage of 
    Data Structures or required Data centric algorithms
*/
public class DataManager : Node
{
    /// <summary>
    ///General MergeSort Function
    /// </summary>
    /// <param name="list">Array to be sorted</param>
    /// <param name="comparisonFunction">A static variable from SortDelegates 
    /// class that determines how two variables are compared</param>
    /// <returns>Sorted Array</returns>
    public static GC.Array MergeSort(GC.Array list, SortDelegates.SortDelegate comparisonFunction)
    {
        MergeSort(ref list, 0, list.Count -1, comparisonFunction);
        return list;
    }


    //Recursive MergeSort
    static void MergeSort(ref GC.Array list, int startIndex, int endIndex, SortDelegates.SortDelegate comparisonFunction)
    {
        if(startIndex >= endIndex) return;
        int partitionIndex = (startIndex + endIndex)/2;
        MergeSort(ref list, startIndex, partitionIndex, comparisonFunction);
        MergeSort(ref list, partitionIndex+1, endIndex, comparisonFunction);
        Merge(ref list, startIndex, partitionIndex, endIndex, comparisonFunction);
    }


    //Merge function of MergeSort()
    static void Merge(ref GC.Array list, int startIndex, int midIndex, int endIndex, Delegate comparisonFunction)
    {
        //3 pointers
        int sIndex = startIndex;
        int mIndex = midIndex+1;
        int index = startIndex; //Traversing Index
        GC.Array newList = new GC.Array();
        while(sIndex <= midIndex && mIndex <= endIndex)
        {
            object output = comparisonFunction.DynamicInvoke(list[sIndex], list[mIndex]);
            newList.Add(output);
            if(output.ToString() == list[mIndex].ToString()) mIndex++;
            else sIndex++;
        }

        while(sIndex <= midIndex)
        {
            newList.Add(list[sIndex]);
            sIndex++;
        }

        while(mIndex <= endIndex)
        {
            newList.Add(list[mIndex]);
            mIndex++;
        }
        //Copy the rest to list
        foreach(object obj in newList)
        {
            list[startIndex] = obj;
            startIndex++;
        }
    }
}