using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatController : MonoBehaviour
{

    bool showConsole;
    string input;
    public static CheatCommand SUMMON_FOX;
    public static CheatCommand SUMMON_DRAGON;
    public static CheatCommand SUMMON_BEAR;
    public static CheatCommand KILL_PET;

    public static CheatCommand FULL_HP;
    public List<object> commandList;
    public void OnToggleDebug(InputValue value)
    {
        showConsole = !showConsole;
    }

    public void OnReturn(InputValue value)
    {
        
        if (showConsole)
        {
            Debug.Log(value.ToString());
            HandleInput();
            input = "";
        }
    }

    private void Awake()
    {
        GameObject spawnerObj = GameObject.FindGameObjectWithTag("Spawner");
        Spawner spawner = spawnerObj.GetComponent<Spawner>();
        SUMMON_FOX = new CheatCommand("/f", "summon fox (healer pet)", "summon_fox", () =>
        {
            
            spawner.SpawnObject(0);
        });

        SUMMON_DRAGON = new CheatCommand("/d", "summon dragon (attacker pet)", "summon_dragon", () =>
        {
           
            spawner.SpawnObject(1);
        });

        SUMMON_BEAR = new CheatCommand("/b", "summon bear (aura pet)", "summon_bear", () =>
        {

            spawner.SpawnObject(2);
        });

        KILL_PET = new CheatCommand("/kill", "kill active pet", "kill_pet", () =>
        {

            //TODO
            spawner.DestroyObject();
        });


        FULL_HP = new CheatCommand("/god", "gives pet invinsibility", "full_hp", () =>
        {

            //TODO
            spawner.FullHpPet();
        });





        commandList = new List<object>()
        {
            SUMMON_DRAGON, SUMMON_FOX, SUMMON_BEAR,KILL_PET, FULL_HP
        };
    }

    private void OnGUI()
    {
        if (!showConsole) { return; }
        float y = 0f;
        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

    }

    private void HandleInput()
    {
       
        for (int i=0; i<commandList.Count; i++)
        {
            CheatCommandBase commandBase = commandList[i] as CheatCommandBase;
            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as CheatCommand != null)
                {
                    (commandList[i] as CheatCommand).Invoke();
                }
            }
        }
        
    }


    
}
