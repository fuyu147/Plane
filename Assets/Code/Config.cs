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

    public List<KeyCode> Run;
    public List<KeyCode> Jump;

    public float Speed;
    public float SpeedRunModifier;
    public float JumpImpulse;
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

    const float CAMERA_DEFAULT_SPEED = 10;
    const float CAMERA_DEFAULT_SENSITIVITY = 50;
    const float CAMERA_DEFAULT_FOV = 80;

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

        if (Manager.DEBUG && false) print($"Config.cs :: Camera speed: {cameraConfig.Speed}");

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

            Run = new() { KeyCode.LeftShift },
            Jump = new() { KeyCode.Space },
        };

        cameraConfig = new()
        {
            Speed = CAMERA_DEFAULT_SPEED,
            Sensitivity = CAMERA_DEFAULT_SENSITIVITY,
            FOV = CAMERA_DEFAULT_FOV
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

                if (Manager.DEBUG && false) print($"<{_name}>, <{_value}>");

                switch (_name)
                {
                    case "MoveForward":
                        movementConfig.Forward = ParseStringToKeycodes(_value);
                        break;
                    case "MoveLeft":
                        movementConfig.Left = ParseStringToKeycodes(_value);
                        break;
                    case "MoveBackward":
                        movementConfig.Backward = ParseStringToKeycodes(_value);
                        break;
                    case "MoveRight":
                        movementConfig.Right = ParseStringToKeycodes(_value);
                        break;
                    case "MoveRun":
                        movementConfig.Run = ParseStringToKeycodes(_value);
                        break;
                    case "MoveJump":
                        movementConfig.Jump = ParseStringToKeycodes(_value);
                        break;
                    case "CameraSpeed":
                        if (!float.TryParse(_value, out cameraConfig.Speed))
                        {
                            print($"Config.cs :: Failed to load camera speed, wrong value: {_value}. Using default value {CAMERA_DEFAULT_SPEED}");
                        }
                        break;
                    case "CameraSensitivity":
                        if (!float.TryParse(_value, out cameraConfig.Sensitivity))
                        {
                            print($"Config.cs :: Failed to load camera sensitivity, wrong value: {_value}. Using default value {CAMERA_DEFAULT_SENSITIVITY}");
                        }
                        break;
                    case "CameraFOV":
                        if (!float.TryParse(_value, out cameraConfig.FOV))
                        {
                            print($"Config.cs :: Failed to load camera speed, wrong value: {_value}. Using default value {CAMERA_DEFAULT_FOV}");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }

    static List<KeyCode> ParseStringToKeycodes(string stringKeys)
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
