using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace bpdev
{
    [ExecuteInEditMode]
    public class UIDebugDisplayLine : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string prefix; 
        [SerializeField] private string suffix;

        private TMP_Text TMP; 

        private void Awake()
        {
            TMP = GetComponent<TMP_Text>();

            TMP.text = prefix + "___" + suffix;
        }

        public void Set(string value)
        {
            TMP.text = prefix + value + suffix;
        }
    }
}