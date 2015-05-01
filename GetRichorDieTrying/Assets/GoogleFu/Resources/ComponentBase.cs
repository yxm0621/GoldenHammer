//----------------------------------------------
//    GoogleFu: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace GoogleFu
{
    public interface IGoogleFuRow
    {
        string GetStringData(string in_colID);
    }

    public interface IGoogleFuDB
    {
        IGoogleFuRow GetGenRow(string in_rowString);
    }

    public class GoogleFuComponentBase : MonoBehaviour
    {


        public virtual void AddRowGeneric(List<string> input)
        {
        }
        public virtual void Clear()
        {
        }
    }
}