using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DSmyth.UIModule
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] UnityEvent m_OnPlayPressed;


        private void OnEnable() {
            StatesModule.GameStates.OnPlayTransition += () => m_OnPlayPressed?.Invoke();
        }
        private void OnDisable() {
            StatesModule.GameStates.OnPlayTransition -= () => m_OnPlayPressed?.Invoke();
        }
    }
}
