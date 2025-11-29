using UnityEngine;

using System;
using System.IO;
using System.Collections.Generic;

public struct Configuration
{
    public List<KeyCode> mMovementForward;
    public List<KeyCode> mMovementLeft;
    public List<KeyCode> mMovementBackward;
    public List<KeyCode> mMovementRight;
    public List<KeyCode> mMovementDown;
    public List<KeyCode> mMovementUp;

    public Configuration(string _filepath)
    {
        mMovementForward = new() { KeyCode.None };
        mMovementLeft = new() { KeyCode.None };
        mMovementBackward = new() { KeyCode.None };
        mMovementRight = new() { KeyCode.None };
        mMovementDown = new() { KeyCode.None };
        mMovementUp = new() { KeyCode.None };


        if (!File.Exists(_filepath))
        {
            Debug.Log($"File not found : {_filepath}");
            return;
        }

        using (var sr = new StreamReader(_filepath))
        {
            while (!sr.EndOfStream)
            {
                var _line = sr.ReadLine();
                if (!_line.StartsWith("--")
                    && !System.String.IsNullOrWhiteSpace(_line)
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
                            mMovementUp = parseStringToKeycodes(_entryValue);
                            break;
                        case "MoveDown":
                            mMovementDown = parseStringToKeycodes(_entryValue);
                            break;
                        case "MoveForward":
                            mMovementForward = parseStringToKeycodes(_entryValue);
                            break;
                        case "MoveLeft":
                            mMovementLeft = parseStringToKeycodes(_entryValue);
                            break;
                        case "MoveBackward":
                            mMovementBackward = parseStringToKeycodes(_entryValue);
                            break;
                        case "MoveRight":
                            mMovementRight = parseStringToKeycodes(_entryValue);
                            break;
                        default:
                            break;
                    }
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
