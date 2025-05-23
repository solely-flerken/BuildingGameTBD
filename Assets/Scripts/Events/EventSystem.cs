﻿using System;
using Buildings;
using Save;
using State;
using UnityEngine;
using Utils;

namespace Events
{
    public class EventSystem : MonoBehaviour
    {
        public static EventSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Mouse
        public event Action<float> OnMouseScroll;
        public event Action OnClick;
        public event Action OnClickRight;
        public event Action<bool> OnClickRightHold;
        public event Action OnMouseWheelClick;
        public event Action<bool> OnMouseWheelHold;

        // Keyboard
        public event Action<GameObject> OnKeyR;
        public event Action OnKeyF3;
        public event Action OnCancel;

        // UI
        public event Action<string> OnButtonClick;
        public event Action<string> OnPlaceBuildingUI;

        // State
        public event Action<Mode, Mode> OnModeChanged;

        // Save
        public event Action<BaseSaveData> OnSaveGame;
        public event Action<string> OnLoadGame;

        // Building
        public event Action<GameObject> OnClickableClick;
        public event Action<GameObject> OnBuildingClick;
        public event Action<GameObject> OnBuildingPlaced;

        public void InvokeClick()
        {
            OnClick?.Invoke();

            var objectUnderMouse = MouseUtils.GetObjectUnderMouse(Camera.main);

            if (objectUnderMouse is null) return;

            // Check for IClickable component
            if (!objectUnderMouse.TryGetComponent<IClickable>(out var component)) return;
            component.OnClick(objectUnderMouse);

            // Only use for UI etc. Worse performance since every component listening to OnClickableClick does checks.
            InvokeClickableClick(objectUnderMouse);

            // Check for Building component
            if (!objectUnderMouse.TryGetComponent<Building>(out _)) return;
            InvokeBuildingClick(objectUnderMouse);
        }

        private void InvokeClickableClick(GameObject obj)
        {
            OnClickableClick?.Invoke(obj);
        }

        public void InvokeBuildingClick(GameObject obj)
        {
            OnBuildingClick?.Invoke(obj);
        }

        public void InvokeBuildingPlaced(GameObject obj)
        {
            OnBuildingPlaced?.Invoke(obj);
        }

        public void InvokeKeyR()
        {
            OnKeyR?.Invoke(null);
        }

        public void InvokeKeyF3()
        {
            OnKeyF3?.Invoke();
        }

        public void InvokeCancel()
        {
            OnCancel?.Invoke();
        }

        public void InvokeClickRight()
        {
            OnClickRight?.Invoke();
        }

        public void InvokeMouseScroll(float value)
        {
            OnMouseScroll?.Invoke(value);
        }

        public void InvokeClickRightHold(bool isHeld)
        {
            OnClickRightHold?.Invoke(isHeld);
        }

        public void InvokeMouseWheelClick()
        {
            OnMouseWheelClick?.Invoke();
        }

        public void InvokeMouseWheelHold(bool isHeld)
        {
            OnMouseWheelHold?.Invoke(isHeld);
        }

        public void InvokeButtonClick(string identifier)
        {
            OnButtonClick?.Invoke(identifier);
        }

        public void InvokeOnPlaceBuildingUI(string identifier)
        {
            OnPlaceBuildingUI?.Invoke(identifier);
        }

        public void InvokeModeChanged(Mode currentMode, Mode newMode)
        {
            OnModeChanged?.Invoke(currentMode, newMode);
        }

        public void InvokeSaveGame(BaseSaveData saveData)
        {
            OnSaveGame?.Invoke(saveData);
        }

        public void InvokeLoadGame(string saveFile)
        {
            OnLoadGame?.Invoke(saveFile);
        }
    }
}