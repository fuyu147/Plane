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

    public List<KeyCode> Run;
    public List<KeyCode> Jump;
    public List<KeyCode> Crawl;
}

public struct CameraConfig
{
    public float Speed;
    public float Sensitivity;
    public float FOV;
}

public class Config : MonoBehaviour
{
    [SerializeField] string mConfigFile;

    public MovementConfig movementConfig;
    public CameraConfig cameraConfig;

    string mConfigPath;
    DateTime mLastWrite;

    // Awake is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mConfigPath = Path.Combine(Application.dataPath, mConfigFile);
        if (!File.Exists(mConfigPath))
        {
            print("Config.cs :: File not found: {mConfigPath}, unable to load settings.");
            return;
        }

        LoadConfiguration();
    }

    // Update is called once per frame
    void Update()
    {
        if (!File.Exists(mConfigPath)) return;

        // print($"Config.cs :: Camera speed: {cameraConfig.Speed}");

        var writeTime = File.GetLastWriteTime(mConfigPath);

        if (writeTime != mLastWrite)
        {
            mLastWrite = writeTime;
            LoadConfiguration();
        }
    }

    void LoadConfiguration()
    {
        movementConfig = new()
        {
            Forward = new() { KeyCode.W },
            Left = new() { KeyCode.A },
            Backward = new() { KeyCode.S },
            Right = new() { KeyCode.D },

            Up = new() { KeyCode.E },
            Down = new() { KeyCode.Q },

            Run = new() { KeyCode.LeftShift },
            Jump = new() { KeyCode.Space },
            Crawl = new() { KeyCode.LeftControl },
        };

        cameraConfig = new()
        {
            Speed = 10,
            Sensitivity = 50,
            FOV = 80
        };

        using var sr = new StreamReader(mConfigPath);
        while (!sr.EndOfStream)
        {
            var _line = sr.ReadLine();
            if (!_line.StartsWith("--")
                && !String.IsNullOrWhiteSpace(_line)
                )
            {
                string[] _entry = _line.Split("=");

                if (_entry.Length != 2)
                    continue;

                string _name = _entry[0];
                string _value = _entry[1];

                // print($"<{_name}>, <{_value}>");

                switch (_name)
                {
                    case "MoveUp":
                        movementConfig.Up = parseStringToKeycodes(_value);
                        break;
                    case "MoveDown":
                        movementConfig.Down = parseStringToKeycodes(_value);
                        break;
                    case "MoveForward":
                        movementConfig.Forward = parseStringToKeycodes(_value);
                        break;
                    case "MoveLeft":
                        movementConfig.Left = parseStringToKeycodes(_value);
                        break;
                    case "MoveBackward":
                        movementConfig.Backward = parseStringToKeycodes(_value);
                        break;
                    case "MoveRight":
                        movementConfig.Right = parseStringToKeycodes(_value);
                        break;
                    case "MoveRun":
                        movementConfig.Run = parseStringToKeycodes(_value);
                        break;
                    case "CameraSpeed":
                        float.TryParse(_value, out cameraConfig.Speed);
                        break;
                    case "CameraSensitivity":
                        float.TryParse(_value, out cameraConfig.Sensitivity);
                        break;
                    case "CameraFOV":
                        float.TryParse(_value, out cameraConfig.FOV);
                        break;
                    default:
                        break;
                }
            }
        }

    }

    static List<KeyCode> parseStringToKeycodes(string stringKeys)
    {
        List<KeyCode> keycodes = new() { };
        var _keysString = stringKeys.Split(",");
        foreach (var _keyString in _keysString)
        {
            if (Enum.TryParse(_keyString.Trim(), out KeyCode key))
            {
                keycodes.Add(key);
            }
        }

        return keycodes;
    }
}
