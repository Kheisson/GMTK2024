using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    { 
        [SerializeField] private List<TutorialStep> playerTutorialSteps;
        private int currentStep = 0;

        private void Start()
        {
            for (int i = 1; i < playerTutorialSteps.Count; i++)
            {
                playerTutorialSteps[i].SetInactive();
            }
        }

        private void OnEnable()
        {
            if (playerTutorialSteps == null || playerTutorialSteps.Count == 0)
            {
                return;
            }

            foreach (var step in playerTutorialSteps)
            {
                step.OnStepTriggered += ShowNextStep;
            }
        }

        private void ShowNextStep()
        {
            playerTutorialSteps[currentStep].Hide().Forget();
            currentStep++;

            if (currentStep < playerTutorialSteps.Count)
            {
                playerTutorialSteps[currentStep].Show().Forget();
            }
        }
    }

}
