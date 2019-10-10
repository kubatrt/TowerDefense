﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameTileContentFactory", menuName = "ScriptableObjects/GameTileContentFactory")]
public class GameTileContentFactory : ScriptableObject
{
    Scene contentScene;

    [SerializeField]
    GameTileContent desinationPrefab = default;
    [SerializeField]
    GameTileContent emptyPrefab = default;

    public void Reclaim(GameTileContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject);
    }

    GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        MoveToFactoryScene(instance.gameObject);
        return instance;
    }

    public GameTileContent Get(GameTileContentType type)
    {
        switch(type)
        {
            case GameTileContentType.Destination: return Get(desinationPrefab);
            case GameTileContentType.Empty: return Get(emptyPrefab);
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }

    private void MoveToFactoryScene(GameObject o)
    {
        if(!contentScene.isLoaded)
        {
            if(Application.isEditor)
            {
                contentScene = SceneManager.GetSceneByName(name);
                if(!contentScene.isLoaded)
                {
                    contentScene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                contentScene = SceneManager.CreateScene(name);
            }
        }
        SceneManager.MoveGameObjectToScene(o, contentScene);
    }
}
