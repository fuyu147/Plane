using UnityEngine;

using System;
using System.IO;
using System.Collections.Generic;

public struct MovementConfig
{
    public List<KeyCode> Forward;
    public List<KeyCode> Left;
    public List<KeyCode> Backward;
    public List<KeyCode> Right;

    public List<KeyCode> Up;
    public List<KeyCode> Down;

    public List<KeyCode> Jump;
    public List<KeyCode> Crawl;
}

public struct CameraConfig
{
    public float Speed;
    public float Sensitivity;
    public float FOV;
}

public struct Configuration
{
    public MovementConfig movementConfig;
    public CameraConfig cameraConfig;

    public Configuration(string _filepath)
    {
        // mMovementForward = new() { KeyCode.None };
        // mMovementLeft = new() { KeyCode.None };
        // mMovementBackward = new() { KeyCode.None };
        // mMovementRight = new() { KeyCode.None };
        // mMovementDown = new() { KeyCode.None };
        // mMovementUp = new() { KeyCode.None };
        //
        movementConfig = new();

        cameraConfig = new()
        {
            Speed = 0,
            Sensitivity = 0,
            FOV = 80
        };

        if (!File.Exists(_filepath))
        {
            Debug.Log($"File not found : {_filepath}");
            return;
        }

        using var sr = new StreamReader(_filepath);
        while (!sr.EndOfStream)
        {
            var _line = sr.ReadLine();
            if (!_line.StartsWith("--")
                && !String.IsNullOrWhiteSpace(_line)
                )
            {
                string[] _entry = _line.Split("=");
                if (_entry.Length != 2) continue;
                string _entryName = _entry[0];
                string _entryValue = _entry[1];
                Debug.Log($"<{_entryName}>, <{_entryValue}>");

                switch (_entryName)
                {
                    case "MoveUp":
                        movementConfig.Up = parseStringToKeycodes(_entryValue);
                        break;
                    case "MoveDown":
                        movementConfig.Down = parseStringToKeycodes(_entryValue);
                        break;
                    case "MoveForward":
                        movementConfig.Forward = parseStringToKeycodes(_entryValue);
                        break;
                    case "MoveLeft":
                        movementConfig.Left = parseStringToKeycodes(_entryValue);
                        break;
                    case "MoveBackward":
                        movementConfig.Backward = parseStringToKeycodes(_entryValue);
                        break;
                    case "MoveRight":
                        movementConfig.Right = parseStringToKeycodes(_entryValue);
                        break;
                    case "CameraSpeed":
                        float.TryParse(_entryValue, out cameraConfig.Speed);
                        break;
                    case "CameraSensitivity":
                        float.TryParse(_entryValue, out cameraConfig.Sensitivity);
                        break;
                    case "CameraFOV":
                        float.TryParse(_entryValue, out cameraConfig.FOV);
                        break;
                    default:
                        break;
                }
            }
        }

    }

    List<KeyCode> parseStringToKeycodes(string keysString)
    {
        List<KeyCode> keycodes = new() { };
        var _keysString = keysString.Split(",");
        foreach (var _keyString in _keysString)
        {
            if (Enum.TryParse(_keyString, out KeyCode key))
            {
                keycodes.Add(key);
            }
        }

        return keycodes;
    }
}

public class Config : MonoBehaviour
{
    [SerializeField] string mConfigFile;
    public Configuration config;

    // Awake is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        string _configFile = Path.Combine(Application.dataPath, mConfigFile);
        config = new Configuration(_configFile);
        // print($"file : {filePath}");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
