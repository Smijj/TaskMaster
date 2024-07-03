using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.UIModule
{
    public class TaskListCtrl : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int m_MaxNumberOfTasks = 10;
        [SerializeField] private float m_CrossOutAnimTime = 0.25f;
        [SerializeField] private LeanTweenType m_CrossOutAnimEase = LeanTweenType.easeOutExpo;
        [SerializeField] private float m_MoveDoneTaskTime = 0.25f;
        [SerializeField] private LeanTweenType m_MoveDoneAnimEase = LeanTweenType.easeInQuad;
        [SerializeField] private TaskNamesSO m_TaskNames;
        
        [Header("References")]
        [SerializeField] private TaskItemCtrl m_TaskItemPrefab;
        [SerializeField] private RectTransform m_TasksContainer;

        [Header("Debug")]
        [ReadOnly, SerializeField] private int m_CompletedTasks = 0;
        [ReadOnly, SerializeField] private int m_CurrentTaskIndex = 0;
        [SerializeField] private List<TaskItemCtrl> m_TaskItems = new List<TaskItemCtrl>();

        private float m_TaskItemHeight;

        #region Unity + Events

        private void Awake() {
            m_TaskItemHeight = m_TaskItemPrefab.GetComponent<RectTransform>().sizeDelta.y;
            InitTaskList();
        }
        private void OnEnable() {
            StatesModule.TaskStates.OnTaskCompleted += OnTaskCompleted;
        }
        private void OnDisable() {
            StatesModule.TaskStates.OnTaskCompleted -= OnTaskCompleted;            
        }
        private void Update() {
            if (m_CompletedTasks > 0) {
                CrossOffTaskItem(m_TaskItems[m_CurrentTaskIndex]);
                m_CompletedTasks--;
            }
        }
        private void OnTaskCompleted() {
            m_CompletedTasks++;
        }

        #endregion

        private void InitTaskList() {
            m_CurrentTaskIndex = m_MaxNumberOfTasks-1;

            // Instantiate list of Task Item prefabs
            for (int i = 0; i < m_MaxNumberOfTasks; ++i) {
                TaskItemCtrl taskItem = Instantiate(m_TaskItemPrefab, m_TasksContainer);
                InitTaskPrefab(taskItem);
                m_TaskItems.Add(taskItem);
            }
        }
        private void InitTaskPrefab(TaskItemCtrl taskItem) {
            string taskName = string.Empty;
            if (m_TaskNames) taskName = m_TaskNames.GetRandomName();
            taskItem.InitTask(taskName);
        }

        private void CrossOffTaskItem(TaskItemCtrl taskItem) {
            Debug.Log("Cross Off Task: " + taskItem.gameObject.name);

            // Cross off item then move current task up (move whole list object)
            //taskItem.CrossOffItemAnim(m_CrossOutAnimTime).setOnComplete(() => MoveItemAnim(m_MoveDoneTaskTime));
            taskItem.CrossOffItemAnim(m_CrossOutAnimTime)
                .setEase(m_CrossOutAnimEase);
            MoveItemAnim(m_MoveDoneTaskTime)
                .setOnComplete(() => ResetCompletedTaskItem(taskItem))
                .setEase(m_MoveDoneAnimEase);

            m_CurrentTaskIndex--;
            if (m_CurrentTaskIndex < 0) m_CurrentTaskIndex = m_MaxNumberOfTasks - 1;
        }

        private int m_MoveItemAnimID = 0;
        private LTDescr MoveItemAnim(float animTime = 0.25f) {
            LeanTween.cancel(m_MoveItemAnimID);
            LTDescr tween = LeanTween.value(gameObject, x => m_TasksContainer.anchoredPosition = new Vector2(0, x), 0, m_TaskItemHeight, animTime);
            m_MoveItemAnimID = tween.uniqueId;
            Debug.Log("MoveItemTween");
            return tween;
        }

        private void ResetCompletedTaskItem(TaskItemCtrl taskItem) {
            // when its done move task item to the bottom of the list and re-init it
            taskItem.transform.SetSiblingIndex(0);  // Moves TaskItem to the bottom of the list (the list is in reverse order)
            InitTaskPrefab(taskItem);

            m_TasksContainer.anchoredPosition = Vector2.zero;   // Reset Tasks Container pos
            Debug.Log("Reset Task: " + taskItem.gameObject.name);
        }

    }
}
