// Main Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// Game Dependencies
using SoM.Core;

namespace SoM.Controllers {
public class AiController : Singleton<AiController> {

#region -------------------- Serialized Variables --------------------
    
#endregion
#region -------------------- Public Variables --------------------
    
#endregion
#region -------------------- Private Variables --------------------
    
#endregion
#region -------------------- Initial Functions --------------------
    
#endregion
#region -------------------- Coroutines --------------------
    
#endregion
#region -------------------- Public Methods --------------------
    public void InitializeController()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Initializing the controller.");

        CoreController.Inst.LoadingStepCompleted();
    }

    public int GetAiNoise()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting Ai noise.");

        int aiSetting = GameplayController.Inst.GameOptions.AiDifficulty;

        switch (aiSetting)
        {
            // Rookie
            case 0: return Random.Range(1,5);

            // Hall of Famer
            case 2: return 0;

            // Veteran
            case 1:
            default: return Random.Range(0,3);
        }
    }

    public int GetAiStaminaNoise()
    {
        CoreController.Inst.WriteLog(this.GetType().Name, $"Getting Ai stamina noise.");
        
        int aiSetting = GameplayController.Inst.GameOptions.AiDifficulty;

        switch (aiSetting)
        {
            // Rookie
            case 0: return Random.Range(0, 60);

            // Hall of Famer
            case 2: return Random.Range(60, 90);

            // Veteran
            case 1:
            default: return Random.Range(30, 70);
        }
    }

    public int GetAiNextLine(int currentLine)
    {
        int aiSetting = GameplayController.Inst.GameOptions.AiDifficulty;
        List<int> weightedChoices = new();

        switch (aiSetting)
        {
            // Rookie
            case 0:
                for (int line = 1; line <= 4; line++)
                {
                    if (line != currentLine)
                    {
                        weightedChoices.Add(line);
                    }
                }
                break;

            // Hall of Famer
            case 2:
                AddWeightedLine(weightedChoices, 1, currentLine, 5);
                AddWeightedLine(weightedChoices, 2, currentLine, 5);
                AddWeightedLine(weightedChoices, 3, currentLine, 1);
                AddWeightedLine(weightedChoices, 4, currentLine, 1);
                break;

            // Veteran
            case 1:
            default:
                AddWeightedLine(weightedChoices, 1, currentLine, 4);
                AddWeightedLine(weightedChoices, 2, currentLine, 4);
                AddWeightedLine(weightedChoices, 3, currentLine, 4);
                AddWeightedLine(weightedChoices, 4, currentLine, 1);
                break;
        }

        return weightedChoices[Random.Range(0, weightedChoices.Count)];
    }

    public int GetAiNextPair(int currentPair)
    {
        int aiSetting = GameplayController.Inst.GameOptions.AiDifficulty;
        List<int> weightedChoices = new();

        switch (aiSetting)
        {
            // Rookie
            case 0:
                for (int pair = 1; pair <= 3; pair++)
                {
                    if (pair != currentPair)
                    {
                        weightedChoices.Add(pair);
                    }
                }
                break;

            // Hall of Famer
            case 2:
                AddWeightedPair(weightedChoices, 1, currentPair, 5);
                AddWeightedPair(weightedChoices, 2, currentPair, 5);
                AddWeightedPair(weightedChoices, 3, currentPair, 1);
                break;

            // Veteran
            case 1:
            default:
                AddWeightedPair(weightedChoices, 1, currentPair, 3);
                AddWeightedPair(weightedChoices, 2, currentPair, 3);
                AddWeightedPair(weightedChoices, 3, currentPair, 2);
                break;
        }

        return weightedChoices[Random.Range(0, weightedChoices.Count)];
    }
#endregion
#region -------------------- Private Methods --------------------
    private void AddWeightedLine(List<int> list, int lineNumber, int currentLine, int weight)
    {
        if (lineNumber == currentLine) { return; }

        for (int i = 0; i < weight; i++)
        {
            list.Add(lineNumber);
        }
    }

    private void AddWeightedPair(List<int> list, int pairNumber, int currentPair, int weight)
    {
        if (pairNumber == currentPair) { return; }

        for (int i = 0; i < weight; i++)
        {
            list.Add(pairNumber);
        }
    }
#endregion
}}
