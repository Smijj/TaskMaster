using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSmyth.TaskModule
{
    public class TaskManager : MonoBehaviour
    {

        [Header("Task Timeout Settings")]
        [SerializeField] private float m_MaxTaskTimeoutTime = 6f;
        [SerializeField] private float m_MinTaskTimeoutTime = 0.5f;
        [SerializeField] private Gradient m_TimeoutGradient;
        [ReadOnly, SerializeField] private float m_CurrentTaskTimeoutTime = 0f;
        [ReadOnly, SerializeField] private float m_TaskTimeoutCounter = 0f;
        [Header("Task Reset Delay Settings")]
        [SerializeField] private float m_ResetDelayTaskCorrect = 0.15f;
        [SerializeField] private float m_ResetDelayTaskFailed = 0.5f;
        [Header("Colour Settings")]
        [SerializeField] private Color m_ColourCurrentTask = Color.yellow;
        [SerializeField] private Color m_ColourInputCorrect = Color.green;
        [SerializeField] private Color m_ColourInputFailed = Color.red;
        [Header("Input Settings")]
        [SerializeField] private List<InputGlyph> m_PossibleTaskInputs = new List<InputGlyph>();

        [Header("References")]
        [SerializeField] private InputGlyphCtrl m_InputGlyph;
        [SerializeField] private Image m_ImgTimeoutUI;

        [Header("Debug")]
        [ReadOnly, SerializeField] private InputGlyph m_CurrentTask;

        private bool m_ListenForInput = true;


        #region Unity + Events

        private void Awake() {
            m_CurrentTaskTimeoutTime = m_MaxTaskTimeoutTime;
        }
        private void OnEnable() {
            StatesModule.GameStates.OnInitGameplay += OnInitGameplay;
            StatesModule.GameStates.OnDifficultyChanged += OnDifficultyChanged;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnInitGameplay -= OnInitGameplay;
            StatesModule.GameStates.OnDifficultyChanged -= OnDifficultyChanged;
        }
        private void Update() {
            HandleTaskInput();
            HandleTaskTimeout();
        }

        private void OnInitGameplay() {
            GenerateTask();
        }
        private void OnDifficultyChanged(float difficultyPercentage) {
            m_CurrentTaskTimeoutTime = Mathf.Lerp(m_MaxTaskTimeoutTime, m_MinTaskTimeoutTime, difficultyPercentage);
        }

        #endregion


        private void HandleTaskTimeout() {
            if (!StatesModule.GameStates.IsGamePlaying || !m_ListenForInput) return;    // Counter wont count down in the delay before generating a new Task

            m_TaskTimeoutCounter -= Time.deltaTime;
            if (m_TaskTimeoutCounter <= 0) {
                // Timeout Task
                FailTask();
                m_TaskTimeoutCounter = m_CurrentTaskTimeoutTime;   // Also gets reset in the GenerateTask func but oh well *Redundancy*
                return;     // Return here to not update the TimeoutUI until the new Task has been generated
            }

            if (m_ImgTimeoutUI) {
                float percentage = m_TaskTimeoutCounter / m_CurrentTaskTimeoutTime;
                m_ImgTimeoutUI.fillAmount = percentage;
                m_ImgTimeoutUI.color = m_TimeoutGradient.Evaluate(percentage);
            }
        }

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
            m_InputGlyph.SetImageColour(m_ColourInputCorrect);  // Change the colour of the InputGlyph
            Invoke("GenerateTask", m_ResetDelayTaskCorrect);    // Wait 0.5s, then Reset InputSequence
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

            m_TaskTimeoutCounter = m_CurrentTaskTimeoutTime;           // Reset Timeout counter

            m_ListenForInput = true;
        }
    }
}
