using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EvgeniiMaklaev.PopUp
{
    [CustomEditor(typeof(PopUpExamples))]
    public class PopUpExamplesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Work in playmode!", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Evgenii Maklaev - PopUp Examples", EditorStyles.helpBox);

            // basic notification
            if (GUILayout.Button("1. Inventory is full"))
            {
                _ = PopUpController.Instance.ShowMessagePopUp(
                    message: "Your backpack is full. Free some space.",
                    header: "Warning!",
                    buttons: new List<PopUpButtonConfig>()
                    {
                        new PopUpButtonConfig()
                        {
                            Label = "Got it",
                            OnClick = () => Debug.Log("[Scenario 1] Player close it")
                        }
                    }
                );
            }

            // boss fight (like dnd or like that)
            if (GUILayout.Button("2. Boss Fight"))
            {
                _ = PopUpController.Instance.ShowMessagePopUp(
                    message: "You see a dragon. Are you ready?",
                    header: "Point of No Return",
                    buttons: new List<PopUpButtonConfig>()
                    {
                        new PopUpButtonConfig()
                        {
                            Label = "Fight!",
                            OnClick = () => Debug.Log("[Scenario 2] Loading arena..")
                        },
                        new PopUpButtonConfig()
                        {
                            Label = "Retreat",
                            OnClick = () => Debug.Log("[Scenario 2] Player retreat")
                        }
                    }
                );
            }

            // story choice
            if (GUILayout.Button("3. Dungeon"))
            {
                _ = PopUpController.Instance.ShowMessagePopUp(
                    message: "Which way will you go?",
                    header: "Choose your path",
                    buttons: new List<PopUpButtonConfig>()
                    {
                        new PopUpButtonConfig()
                        {
                            Label = "Left",
                            OnClick = () => Debug.Log("[Scenario 3] Player go Left")
                        },
                        new PopUpButtonConfig()
                        {
                            Label = "Center",
                            OnClick = () => Debug.Log("[Scenario 3] Player go Center")
                        },
                        new PopUpButtonConfig()
                        {
                            Label = "Right",
                            OnClick = () => Debug.Log("[Scenario 3] Player go Right")
                        }
                    }
                );
            }

            // shop example
            if (GUILayout.Button("4. Merchants Shop"))
            {
                _ = PopUpController.Instance.ShowMessagePopUp(
                    message: "Greetings, traveler!",
                    header: "Shop",
                    buttons: new List<PopUpButtonConfig>()
                    {
                        new PopUpButtonConfig()
                        {
                            Label = "Sword (100)",
                            OnClick = () => Debug.Log("[Scenario 4] Transaction: Sword purchased. -100 gold")
                        },
                        new PopUpButtonConfig()
                        {
                            Label = "Shield (50)",
                            OnClick = () => Debug.Log("[Scenario 4] Transaction: Shield purchased. -50 gold")
                        },
                        new PopUpButtonConfig()
                        {
                            Label = "Potion (10)",
                            OnClick = () => Debug.Log("[Scenario 4] Transaction: Mana potion purchased. -10 gold")
                        },
                        new PopUpButtonConfig()
                        {
                            Label = "Sell loot",
                            OnClick = () => Debug.Log("[Scenario 4] Player want sell loot")
                        },
                        new PopUpButtonConfig()
                        {
                            Label = "Leave",
                            OnClick = () => Debug.Log("[Scenario 4] Player close shop")
                        }
                    }
                );
            }
        }
    }

    public class PopUpExamples : MonoBehaviour
    {
        // class for display inspector example buttons
    }
}