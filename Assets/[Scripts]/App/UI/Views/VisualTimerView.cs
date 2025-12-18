using System;
using System.Collections;
using Serjbal.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Serjbal
{
    public class VisualTimer : Page
    {
        [SerializeField] private Image _progressBar;
        public Action action;

        private void Awake() => Hide();

        private void OnEnable()
        {
            _progressBar.fillAmount = 0;
            StartCoroutine(DoTimer());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            action = null;
        }

        private IEnumerator DoTimer()
        {
            float steps = 15f;
            float delay = 1f / steps;
            for (int i = 0; i < steps; i++)
            {
                _progressBar.fillAmount = Mathf.Lerp(0, 1, i/steps);
                yield return new WaitForSeconds(delay);
            }

            action?.Invoke();
            Hide();
        }
    }
}
