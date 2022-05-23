using System;
using System.Text.RegularExpressions;
using TalkBox.Nodes;
using UnityEngine;

namespace TalkBox.Core
{
	public static class DialogueTextProcessor
	{
		const string handlebarsPattern = @"{{(.*?)/(.*?)}}";
		private const string err = "<c=red><b>[err]</b></c>";

		public static string GetFormattedText(DialogueNode node)
		{
			return Regex.Replace(node.Text, handlebarsPattern, match => GetReferenceValue(match, node));
		}

		private static string GetReferenceValue(Match match, DialogueNode dialogueNode)
		{
			// pattern should only return 2 groups + the original match per match
			// (index 0 is the full match)
			if (match.Groups.Count != 3) throw new Exception("Unexpected number of matching groups in match");

			// match root to character
			CharacterData character = GetCharacter(match.Groups[1].Value, dialogueNode);
			if (character == null) return err;

			// return property
			try
			{
				return GetPropertyValue(match.Groups[2].Value, character);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Debug.LogError(e.Message);
				return err;
			}
		}

		private static CharacterData GetCharacter(string name, DialogueNode node)
		{
			if (name.Equals("self", StringComparison.InvariantCultureIgnoreCase)) return node.Character;

			var convo = (Conversation)node.graph;
			var participants = convo.GetParticipants();

			// check character types (must be first)
			foreach (var kvp in DialogueManager.Instance.CharacterTypeMap)
			{
				if (kvp.Key.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
				{
					return kvp.Value;
				}
			}
			
			// check participants
			foreach (var characterData in participants)
			{
				if (characterData.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
				{
					return characterData;
				}
			}

			Debug.LogError($"Could not find character {name} in conversation or character type map");
			return null;
		}

		private static string GetPropertyValue(string prop, CharacterData character)
		{
			// spec here: https://docs.google.com/document/d/1nWoMQ52CtlXLhwcNu6_JB5LQLmzQtI5-8Ka3ONQL-pk/edit#
			return prop switch
			{
				"name" => character.ShortName,
				"shortName" => character.ShortName,
				"fullName" => character.FullName,
				_ => throw new ArgumentOutOfRangeException(nameof(prop), prop, $"Invalid property name {prop}")
			};
		}
	}
}