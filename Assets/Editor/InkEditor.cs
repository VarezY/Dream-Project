using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEditor;
using UnityEngine;
using Varez.Interact;

namespace Varez
{
    [CustomEditor(typeof(InteractablePerson))]
    [InitializeOnLoad]
    public class InkEditor : Editor
    {
        static bool storyExpanded;
        static InkEditor () 
        {
            InteractablePerson.OnCreateStory += OnCreateStory;
        }

        static void OnCreateStory (Story story) 
        {
            // If you'd like NOT to automatically show the window and attach (your teammates may appreciate it!) then replace "true" with "false" here. 
            InkPlayerWindow window = InkPlayerWindow.GetWindow(false);
            if(window != null) InkPlayerWindow.Attach(story);
        }
        
        public override void OnInspectorGUI () 
        {
            Repaint();
            base.OnInspectorGUI ();
            var realTarget = target as BasicInkExample;
            if (realTarget != null)
            {
                var story = realTarget.story;
                InkPlayerWindow.DrawStoryPropertyField(story, ref storyExpanded, new GUIContent("Story"));
            }
        }
    }
}