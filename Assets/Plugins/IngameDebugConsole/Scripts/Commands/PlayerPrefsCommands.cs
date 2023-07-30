using UnityEngine;

namespace IngameDebugConsole.Commands
{
	public class PlayerPrefsCommands
	{
		[ConsoleMethod( "prefs.clear", "Deletes all PlayerPrefs fields" ), UnityEngine.Scripting.Preserve]
		public static void PlayerPrefsClear()
		{
			PlayerPrefs.DeleteAll();
		}
	}
}