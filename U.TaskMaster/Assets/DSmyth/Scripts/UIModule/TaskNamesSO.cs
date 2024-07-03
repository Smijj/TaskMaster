using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.UIModule
{
    [CreateAssetMenu(fileName ="New Task Names", menuName ="ScriptableObjects/Create TaskNames")]
    public class TaskNamesSO : ScriptableObject
    {
        public List<string> Names = new List<string>();

        public string GetRandomName() {
            if (Names.Count == 0) return "Jellyfish";

            return Names[Random.Range(0, Names.Count)];
        }
    }
}
