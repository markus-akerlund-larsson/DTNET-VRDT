using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace ScenarioTaskSystem
{
    [Serializable]
    public class Scenario
    {
        [SerializeField] List<TaskSettings> tasks;
        [SerializeField] public UnityEvent OnAllCompleted;
        //[SerializeField] RestartSystem Restart;
        [SerializeField] bool active;
        [SerializeField] private bool guided;
        [SerializeField] ResultHandler Scoring;
        
        /*
        public Scenario(TaskSettings[] ts, UniversalOperation operation)
        {
            tasks = new List<TaskSettings>();
            for (int i = 0; i < ts.Length; i++)
            {
                tasks.Add(ts[i]);
                if(ts[i].Task != null)
                {
                    ts[i].Task.AddParentScenario(this);
                }
                else
                {
                    Debug.LogWarning("There is an empty task in the scenario!");
                }
            }
            OnAllCompleted = operation;
            Restart = UnityEngine.Object.FindObjectOfType<RestartSystem>();
            Debug.Log("Is the scenario active? " + active);
        }
        */

        public void Reconnect()
        {
            foreach (TaskSettings ts in tasks)
            {
                ts.Task.AddParentScenario(this);
            }
        }

        public void RecieveScore()
        {
            //Scoring.Refresh(tasks);
        }

        public int TotalScore()
        {
            int _s = 0;
            foreach (TaskSettings _ts in tasks)
            {
                _s += _ts.Score;
            }
            return _s;
        }

        public void SetGuidedMode(bool enabled)
        {
            guided = enabled;
        }

        public void WrongOrder(TaskSettings completedTaskSettings)
        {
            Debug.Log("Task completion aborted! Out of order!");

            if (!guided)
            {
                if (completedTaskSettings.OnWrongOrder != null)
                    completedTaskSettings.OnWrongOrder.Invoke();
                if (!completedTaskSettings.Secondary)
                {
                    FailTasksUpTo(completedTaskSettings);
                }
            }
            else
            {
                if (completedTaskSettings.IsCritical)
                {
                    //Restart.Load();
                }
            }
        }

        private void FailTasksUpTo(TaskSettings completedTaskSettings)
        {
            //things
            foreach (TaskSettings currentTask in tasks)
            {
                if (currentTask == completedTaskSettings)
                {
                    Debug.Log(currentTask.Task.gameObject.name + " is our task, let's go!");
                    currentTask.Task.Complete(completedTaskSettings.Score);
                    break;
                }
                if (currentTask.Completed == false)
                {
                    currentTask.Task.Complete(0);
                }
                Debug.Log(currentTask.Task.gameObject.name + " is not our task yet, but it is completed");
            }
        }

        int RecalculateScore()
        {
            int score = 0;
            foreach (TaskSettings task in tasks)
            {
                score += task.Score;
            }
            return score;
        }

        public void OnTaskCompleted(Task taskCompleted, int score)
        {
            if (!active)
                return;
            TaskSettings completedTaskSettings = null;
            if(tasks == null)
            {
                Debug.Log("Tasks field is empty");
            }
            foreach (TaskSettings currentTask in tasks)
            {
                if (currentTask.Task == taskCompleted)
                {
                    completedTaskSettings = currentTask;
                    if (!currentTask.Completed)
                        break;
                }
            }
            if (completedTaskSettings == null)
            {
                Debug.LogError("Error! Task not on this scenarios task list.");
                return;
            }
            if (completedTaskSettings.Completed)
            {
                Debug.LogWarning("This task is getting completed multiple times");
                return;
            }
            completedTaskSettings.Score = score;
            if (completedTaskSettings.maxScore < score)
            {
                score = completedTaskSettings.maxScore;
                Debug.LogWarning(taskCompleted.gameObject.name + " score is too high");
            }

            if (completedTaskSettings.maxScore > score)
            {
                if (guided)
                {
                    //Restart.Load();
                    Debug.LogWarning(taskCompleted.gameObject.name + " task failed, loading a save!");
                    return;
                }
            }
            

            if (completedTaskSettings.Order == null)
            {
                completedTaskSettings.Completed = true;
                Debug.Log("Task completion confirmed!");
                if (completedTaskSettings.OnCompleted != null)
                    completedTaskSettings.OnCompleted.Invoke();
                allCompleted();
                completedTaskSettings.Score = score;
            }
            else
            {
                bool ordered = true;
                foreach (TaskSettings currentTask in tasks)
                {
                    if (currentTask == completedTaskSettings)
                    {
                        Debug.Log(currentTask.Task.gameObject.name + " is our task, let's go!");
                        break;
                    }
                    if (currentTask.Completed == false)
                    {
                        Debug.Log(currentTask.Task.gameObject.name + " is not our task and it is not completed, aborting!");
                        ordered = false;
                        break;
                    }
                    Debug.Log(currentTask.Task.gameObject.name + " is not our task yet, but it is completed");
                }
                if (ordered)
                {
                    completedTaskSettings.Completed = true;
                    Debug.Log("Task completion confirmed!");
                    if (completedTaskSettings.OnCompleted != null)
                        completedTaskSettings.OnCompleted.Invoke();
                    allCompleted();
                }
                else
                {
                    WrongOrder(completedTaskSettings);
                }
            }
        }

        public void OnTaskFailed(Task taskFailed)
        {
            TaskSettings completedTaskSettings = null;
            foreach (TaskSettings currentTask in tasks)
            {
                if (currentTask.Task == taskFailed)
                {
                    completedTaskSettings = currentTask;
                }
            }
            if (completedTaskSettings == null)
            {
                Debug.LogError("Error! Task not on this scenarios task list.");
                return;
            }
            Debug.Log("Task completion confirmed!");
            if (completedTaskSettings.OnFailed != null)
                completedTaskSettings.OnFailed.Invoke();
            allCompleted();
        }

        private void allCompleted()
        {
            foreach (TaskSettings currentTask in tasks)
            {
                if (!currentTask.Completed)
                {
                    return;
                }
            }
            if (OnAllCompleted != null)
                OnAllCompleted.Invoke();
            Scoring.HandleResult(this);
        }

        public void Activate(bool _active = true)
        {
            active = _active;
        }
    }

    [System.Serializable]
    public class TaskSettings
    {
        public Task Task;
        public Task Order;
        //[System.NonSerialized]
        public bool Completed;
        [SerializeField] public UnityEvent OnCompleted;
        [SerializeField] public UnityEvent OnWrongOrder;
        [SerializeField] public UnityEvent OnFailed;
        public int Score;
        public int maxScore;
        public bool IsCritical;
        public bool Secondary;
    }
}