using UnityEditor;

namespace EvgeniiMaklaev.SaveSystem
{
    public class SaveSystemEditor : Editor
    {
        [MenuItem("Tools/Save System/Save")]
        public static async void Save()
        {
            // for example, create Settings there
            GameSettings gameSettings = new();
            await SaveSystem.Save();
        }

        [MenuItem("Tools/Save System/Load")]
        public static async void Load()
        {
            await SaveSystem.Load();
        }
    }
}
