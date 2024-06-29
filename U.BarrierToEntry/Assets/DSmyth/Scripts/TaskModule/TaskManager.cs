using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.TaskModule
{
    public class TaskManager : MonoBehaviour
    {

        [Header("Settings")]
        [SerializeField] private Color m_ColourCurrentTask = Color.yellow;
        [SerializeField] private Color m_ColourInputCorrect = Color.green;
        [SerializeField] private Color m_ColourInputFailed = Color.red;
        [SerializeField] private float m_ResetDelayTaskCorrect = 0.15f;
        [SerializeField] private float m_ResetDelayTaskFailed = 0.5f;

        [Header("References")]
        [SerializeField] private InputGlyphCtrl m_InputGlyph;
        [SerializeField] private List<InputGlyph> m_PossibleTaskInputs = new List<InputGlyph>();

        [Header("Debug")]
        [SerializeField] private InputGlyph m_CurrentTask;

        private bool m_ListenForInput = true;


        #region Unity + Events

        private void OnEnable() {
            StatesModule.GameStates.OnInitGameplay += OnInitGameplay;
            StatesModule.GameStates.OnStartGameplay += OnStartGameplay;
            StatesModule.GameStates.OnGameOver += OnGameOver;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnInitGameplay -= OnInitGameplay;
            StatesModule.GameStates.OnStartGameplay -= OnStartGameplay;
            StatesModule.GameStates.OnGameOver -= OnGameOver;
        }

        private void Update() {
            HandleTaskInput();
        }

        private void OnInitGameplay() {
            GenerateTask();
        }
        private void OnStartGameplay() {

        }
        private void OnGameOver() {

        }

        #endregion


        private void HandleTaskInput() {
            if (!StatesModule.GameStates.IsGamePlaying || !m_ListenForInput) return;

            // check if any of the possible task inputs are being pressed
            // if one is, check if it matches the current index pos of the input sequence
            // Matches = correct continue, Doesnt match = Incorrect failed task
            foreach (InputGlyph inputGlyph in m_PossibleTaskInputs) {
                if (!Input.GetKeyDown(inputGlyph.KeyCode)) continue;   // Pass the rest of the checks if the player isnt pressing one of the possible keys.

                if (inputGlyph.KeyCode == m_CurrentTask.KeyCode) {
                    // The correct key has been pressed
                    CompleteTask();
                }
                else {
                    // The incorrect key has been pressed
                    FailTask();
                }
            }
        }
        private void CompleteTask() {
            m_ListenForInput = false;   // Stop TaskManager from listening for inputs until the next Input Seq is generated
            Debug.Log("Task Complete!");

            StatesModule.TaskStates.OnTaskCompleted?.Invoke();  // Invoke OnTaskCompleted event

            // Do some sort of effect & change the colour of the InputGlyph
            m_InputGlyph.SetImageColour(m_ColourInputCorrect);

            Invoke("GenerateTask", m_ResetDelayTaskCorrect);        // Wait 0.5s, then Reset InputSequence
        }
        private void FailTask() {
            m_ListenForInput = false;   // Stop TaskManager from listening for inputs until the next Input Seq is generated
            Debug.Log("Task Failed...");

            StatesModule.TaskStates.OnTaskFailed?.Invoke();         // Invoke OnTaskFailed event
            m_InputGlyph.SetImageColour(m_ColourInputFailed);       // Highlight Key that the player failed on red
            Invoke("GenerateTask", m_ResetDelayTaskFailed);         // Wait 0.5s, then Reset InputSequence
        }


        private void GenerateTask() {
            if (m_PossibleTaskInputs.Count <= 0) return;

            // Set CurrentTask randomly and update the InputGlyph UI element
            m_CurrentTask = m_PossibleTaskInputs[Random.Range(0, m_PossibleTaskInputs.Count)];
            m_InputGlyph.SetImageSprite(m_CurrentTask.Sprite);
            m_InputGlyph.SetImageColour(m_ColourCurrentTask);

            m_ListenForInput = true;
        }
    }
}
