using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.TaskModule {
    public class SequenceTaskManager : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private int m_InputSequenceLength = 4;
        [SerializeField] private Color m_ColourInputCorrect = Color.green;
        [SerializeField] private Color m_ColourInputIncorrect = Color.red;
        [SerializeField] private Color m_ColourCurrentInput = Color.yellow;

        [Header("References")]
        [SerializeField] private Transform m_InputGlyphContainer;
        [SerializeField] private InputGlyphCtrl m_InputGlyphPrefab;
        [SerializeField] private List<InputGlyph> m_PossibleTaskInputs = new List<InputGlyph>();

        [Header("Debug")]
        [SerializeField] private InputGlyph[] m_InputSequence;
        [SerializeField] private int m_CurrentInputSequenceIndex = 0;

        private List<InputGlyphCtrl> m_InputGlyphs = new List<InputGlyphCtrl>();
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
            GenerateInputSequence();
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

                if (inputGlyph.KeyCode == m_InputSequence[m_CurrentInputSequenceIndex].KeyCode) {
                    // The correct key has been pressed
                    // Highlight the UI element for that key green
                    ProgressTask();
                }
                else {
                    // The incorrect key has been pressed
                    FailTask();
                }
            }
        }

        private void GenerateInputSequence() {
            if (m_PossibleTaskInputs.Count <= 0) return;

            Debug.Log("Generate Input Seq");

            // Clear input glyphs objects and list
            foreach (var glyph in m_InputGlyphs) {
                Destroy(glyph.gameObject);
            }
            m_InputGlyphs.Clear();

            // Reset Index
            m_CurrentInputSequenceIndex = 0;
            // Create new InputSequence array
            m_InputSequence = new InputGlyph[m_InputSequenceLength];
            
            // Set InputSequence and spawn the corresponding UI elements
            for (int i = 0; i < m_InputSequence.Length; i++) {
                m_InputSequence[i] = m_PossibleTaskInputs[Random.Range(0, m_PossibleTaskInputs.Count)];
                
                // Spawn InputGlyph
                InputGlyphCtrl inputGlyph = Instantiate(m_InputGlyphPrefab, m_InputGlyphContainer);
                inputGlyph.SetImageSprite(m_InputSequence[i].Sprite);
                inputGlyph.SetImageColour(Color.white);
                m_InputGlyphs.Add(inputGlyph);
            }

            // Highlight the first inputGlyph
            m_InputGlyphs[0].SetImageColour(m_ColourCurrentInput);

            m_ListenForInput = true;
        }

        private void ProgressTask() {
            // Mark the currentIndex of the InputSequence as correct
            m_InputGlyphs[m_CurrentInputSequenceIndex].SetImageColour(m_ColourInputCorrect);

            if (m_CurrentInputSequenceIndex < m_InputSequence.Length - 1) {
                // Increment Index
                m_CurrentInputSequenceIndex++;  
                // Highlight Glyph at the new Index
                m_InputGlyphs[m_CurrentInputSequenceIndex].SetImageColour(m_ColourCurrentInput);
            }
            else {
                // InputSequence Complete
                CompleteTask();
            }
        }
        private void CompleteTask() {
            m_ListenForInput = false;   // Stop TaskManager from listening for inputs until the next Input Seq is generated
            Debug.Log("Task Complete!");

            StatesModule.TaskStates.OnTaskCompleted?.Invoke();  // Invoke OnTaskCompleted event
            GenerateInputSequence();
        }
        private void FailTask() {
            m_ListenForInput = false;   // Stop TaskManager from listening for inputs until the next Input Seq is generated
            Debug.Log("Task Failed...");

            StatesModule.TaskStates.OnTaskFailed?.Invoke();                                     // Invoke OnTaskFailed event
            m_InputGlyphs[m_CurrentInputSequenceIndex].SetImageColour(m_ColourInputIncorrect);  // Highlight Key that the player failed on red
            Invoke("GenerateInputSequence", 0.5f);                                              // Wait 0.5s, then Reset InputSequence
        }
    }
}
