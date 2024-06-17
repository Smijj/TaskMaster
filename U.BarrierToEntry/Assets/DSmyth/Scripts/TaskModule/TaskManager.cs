using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.TaskModule {
    public class TaskManager : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private Color m_ColourInputCorrect = Color.green;
        [SerializeField] private Color m_ColourInputIncorrect = Color.red;
        [SerializeField] private Color m_ColourCurrentInput = Color.yellow;

        [SerializeField] private Transform m_InputGlyphContainer;
        [SerializeField] private InputGlyphCtrl m_InputGlyphPrefab;
        [SerializeField] private List<InputGlyph> m_PossibleTaskInputs = new List<InputGlyph>();

        [Header("Debug")]
        [SerializeField] private InputGlyph[] m_InputSequence = new InputGlyph[4];
        [SerializeField] private int m_CurrentInputSequenceIndex = 0;

        private List<InputGlyphCtrl> m_InputGlyphs = new List<InputGlyphCtrl>(); 

        private void Start() {
            GenerateInputSequence();
        }

        private void Update() {
            HandleTaskInput();
        }

        private void HandleTaskInput() {
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

            // Clear input glyphs objects and list
            foreach (var glyph in m_InputGlyphs) {
                Destroy(glyph.gameObject);
            }
            m_InputGlyphs.Clear();

            // Reset Index
            m_CurrentInputSequenceIndex = 0;
            // Create new InputSequence array
            m_InputSequence = new InputGlyph[4];
            
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
            Debug.Log("Task Complete!");
            // Invoke OnTaskCompleted event
            GenerateInputSequence();    // Create new InputSequence

        }
        private void FailTask() {
            Debug.Log("Task Failed...");
            
            // Invoke OnTaskFailed event
            // Highlight Key that the player failed on red
            m_InputGlyphs[m_CurrentInputSequenceIndex].SetImageColour(m_ColourInputIncorrect);
            // Wait 0.5s
            // Reset InputSequence
            GenerateInputSequence();    

            
        }

    }
}
