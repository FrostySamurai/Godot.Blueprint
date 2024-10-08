﻿using Godot;
using Samurai.Application.Configs;
using Samurai.Application.Pooling;
using Samurai.Application.Scenes;
using Samurai.Application.SessionHandling;

namespace Samurai.Application
{
    public partial class StartUp : Node
    {
        [Export]
        private string _resourcesPath = "res://Resources";
        [Export]
        private string _definitionsFolder = "Definitions";
        [Export]
        private string _configsFolder = "Configs";
        [Export]
        private NodeSessionHandler[] _sessionHandlers;
        [Export]
        private Node _sceneParent;
        [Export]
        private CanvasItem _poolParent2D;
        [Export]
        private Node3D _poolParent3D;
        
        private static bool _wasStartedUp;

        public override void _EnterTree()
        {
            base._EnterTree();
                
            if (_wasStartedUp)
            {
                return;
            }
            
            _wasStartedUp = true;
            Definitions.Create(_resourcesPath, _definitionsFolder, _configsFolder);
            SceneLoader.Init(_sceneParent);
            NodePool.Init(_poolParent2D, _poolParent3D);
            Session.Register(_sessionHandlers);
            
            App.Add(_sceneParent);
            App.Init(GetTree());
            
            SceneLoader.LoadScene(Definitions.Config<AppConfig>().MainMenuScene);
        }
    }
}