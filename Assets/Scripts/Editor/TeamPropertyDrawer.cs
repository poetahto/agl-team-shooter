using System.Collections.Generic;
using Gameplay;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomPropertyDrawer(typeof(TeamReferenceAttribute))]
    public class TeamPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            List<string> choices = new List<string>();
            TeamDefinition[] teamDefinitions = Object.FindObjectsByType<TeamDefinition>(FindObjectsSortMode.InstanceID);
            int defaultValue = 0;

            for (int i = 0 ; i < teamDefinitions.Length; i++)
            {
                TeamDefinition teamDefinition = teamDefinitions[i];

                choices.Add(teamDefinition.displayName);

                if (teamDefinition.id == property.intValue)
                    defaultValue = i;
            }

            var container = new VisualElement();
            var label = new Label(property.displayName);
            var dropdownField = new DropdownField(choices, defaultValue);

            dropdownField.RegisterValueChangedCallback(eventData =>
            {
                foreach (TeamDefinition teamDefinition in teamDefinitions)
                {
                    if (eventData.newValue == teamDefinition.displayName)
                    {
                        property.intValue = teamDefinition.id;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
            });

            container.style.flexDirection = FlexDirection.Row;
            container.Add(label);
            container.Add(dropdownField);
            return container;
        }
    }
}
