using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class InputMapManager : MonoBehaviour
{
    private string[] mode ={
        "Player",
        "Dialog",
        "Inventory",
        "Store",
        "Book",
        "Excavation",
        "Camp",
        "Menu"
    };

    public RectTransform InputTipsContainer;
    public GameObject[] TipPrefabs;
    public List<GameObject> Tips;
    private Dictionary<string, GameObject> _tipPrefabs = new Dictionary<string, GameObject>(); 

#if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
#endif
    private void Start()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        for(int i = 0; i < TipPrefabs.Length; i++)
        {
            _tipPrefabs[TipPrefabs[i].name] = TipPrefabs[i];
        }
        UpdateControlTip();
    }

    /// <summary>
    /// Switch the Input Map of player input assets.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns>0-Player,1-Dialog,2-Inventory,3-Store,4-Book,5-Excavation,6-Camp.</returns>
    public void SwitchMap(int ID)
    {
        _playerInput.SwitchCurrentActionMap(mode[ID]);
        UpdateControlTip();
        Debug.Log("Current Input Map is switched to " + _playerInput.currentActionMap);
    }

    public void UpdateControlTip()
    {
        for(int i = 0; i < Tips.Count; i++)
        {
            Destroy(Tips[i]);
        }

        Tips.Clear();

        var actionNames = _playerInput.currentActionMap.actions;

        foreach (var action in actionNames)
        {
            if (_tipPrefabs.ContainsKey(action.name))
            {
                Tips.Add(Instantiate(_tipPrefabs[action.name], InputTipsContainer));
            }
        }

    }

    public void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

}
