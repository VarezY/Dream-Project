using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Varez.UI
{
    [CreateAssetMenu(fileName = "WordUI", menuName = "Scriptable/GameUISprites", order = 0)]
    public class GameUIScriptables : ScriptableObject
    {
        public GameUIList[] uIList;
    }

    [Serializable]
    public class GameUIList
    {
        public string schemeName;
        public UISprites icons;
    }
}