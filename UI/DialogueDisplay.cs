using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using NaughtyAttributes;
using MacSalad.Core.UI;
using MacSalad.Core;
using MacSalad.Core.Events;
using static TalkBox.Core.Events;
using TalkBox.Nodes;
using TalkBox.Core;
using UnityEngine.UI;

public class DialogueDisplay : MSBehaviour
{

	[Header("Parameters")]
	public float LetterDelay = 0.01f;
	public float SpaceDelay = 0.025f;
	public float SentanceDelay = 0.05f;

	[Header("References")]
	public TMP_Text DialogueText;
	public TMP_Text NameText;
	public UIFader Fader;
	public DialogueOption dialogueOptionPrefab;

	public GameObject ContinuePrompt;

	private bool ConversationInProgress = false;
	private Character speakingCharacter;

	public Image HeaderBackground;
	public Color HeaderTextLight;
	public Color HeaderTextDark;

	// A list of dialogue option buttons
	private List<DialogueOption> dialogueOptions = new List<DialogueOption>();

	// The recently selected option
	private string selectedOption = null;
	protected override void SafeInitialize()
	{
		base.SafeInitialize();
	}

	protected override void AddEventListeners()
	{
		base.AddEventListeners();
		EventDispatcher.AddListener<ConversationEvent>(StartConversation);
	}

	protected override void RemoveEventListeners()
	{
		base.RemoveEventListeners();

		EventDispatcher.RemoveListener<ConversationEvent>(StartConversation);
	}

	public void StartConversation(ConversationEvent e)
	{
		if (e.Started)
		{
			if (ConversationInProgress)
			{
				Debug.LogError("Tried to start conversation while one was in progress.");
			}

			StartCoroutine(DisplayConversation(e.Conversation));
		}
	}

	IEnumerator DisplayConversation(Conversation convo)
	{
		// Initialize the conversation
		convo.Init();

		// get the current dialogue
		DialogueNode current = convo.current;

		// Safety in case this conversation is empty
		if (!current)
		{
			Debug.LogError("Tried to start empty conversation!");
		}

		ConversationInProgress = true;

		DialogueManager.Instance.SetState(DialogueManager.GameState.Dialogue);

		// Clear text box
		DialogueText.text = "";
		NameText.text = "";

		// set the background color before we fade in
		HeaderBackground.color = current.Character.CharacterColor;

		// hide all dialogue options that exist
		for (int i = 0; i < dialogueOptions.Count; i++)
		{
			dialogueOptions[i].Clear();
			dialogueOptions[i].gameObject.SetActive(false);
		}

		// Hide the continue prompt
		ContinuePrompt.SetActive(false);

		// And show it 
		Fader.FadeIn();

		// Wait to make sure it's shown
		yield return new WaitForSeconds(Fader.FadeInTime);

		// while there's still dialogue to be had
		while (current != null)
		{
			// Show the dialogue
			yield return StartCoroutine(DisplayDialogue(current));

			// attempt to move to the next node 
			if (convo.Next(selectedOption))
			{
				// if we can, set it to current
				current = convo.current;
			}
			else
			{
				// otherwise, the conversation is over
				current = null;

				// hide the text box
				Fader.FadeOut();

				// wait to make sure it's gone
				yield return new WaitForSeconds(Fader.FadeOutTime);
			}
		}

		ConversationInProgress = false;

		// Dispatch an event saying this conversation is over
		EventDispatcher.Dispatch(ConversationEvent.Prepare(convo, false));

		// Return the game state to gameplay
		DialogueManager.Instance.SetState(DialogueManager.GameState.Gameplay);

	}

	IEnumerator DisplayDialogue(DialogueNode d)
	{
		// emit dialogue started event
		EventDispatcher.Dispatch(DialogueEvent.Prepare(d, true));

		// Find the speaking character
		if(!DialogueManager.Instance.CharacterTypeMap.TryGetValue(d.Character, out var character))
		{
			character = d.Character;
		}
		
		speakingCharacter = DialogueManager.Instance.GetCharacter(character);

		// Clear the text box
		DialogueText.text = "";

		NameText.text = character.FullName;

		HeaderBackground.color = character.CharacterColor;

		Color.RGBToHSV(character.CharacterColor, out float H, out float S, out float V);

		// this is a light colored backgroud
		if (V > 0.5f)
		{
			NameText.color = HeaderTextDark;
		}
		else
		{
			NameText.color = HeaderTextLight;
		}

		// Hide the continue prompt
		ContinuePrompt.SetActive(false);

		// Hide all dialogue options 
		for (int i = 0; i < dialogueOptions.Count; i++)
		{
			dialogueOptions[i].gameObject.SetActive(false);
		}

		// Show as many as we'll need for this dialogue
		if (d.dialogueOptions.Length > 0)
		{
			// if there's ever more options than there are buttons, spawn some more
			while (d.dialogueOptions.Length > dialogueOptions.Count)
			{
				dialogueOptions.Add(Instantiate(dialogueOptionPrefab, transform.parent));
			}
			for (int i = 0; i < d.dialogueOptions.Length; i++)
			{
				dialogueOptions[i].gameObject.SetActive(true);

				// Clear the options so the player can't click or read the options while the text is scrolling
				dialogueOptions[i].Clear();
			}
		}

		var index = 0;

		// Get the text from the dialogue
		var rawText = DialogueTextProcessor.GetFormattedText(d);

		// Wait a frame to avoid accidentally skipping to next dialogue
		yield return null;

		// While there's letters left to show
		while (index < rawText.Length)
		{
			if (Input.GetMouseButtonUp(0))
			{
				index = rawText.Length - 1;
			}
			// get a version of the text we can modify
			var currentText = rawText;

			if (rawText[index] == '<')
			{
				while (index < rawText.Length && rawText[index] != '>')
				{
					index++;
				}
			}

			// Insert the alpha tag to hide text after the index
			currentText = currentText.Insert(index + 1, "<alpha=#00>");
			currentText += "<\\alpha>";

			// Display the text 
			DialogueText.text = currentText;

			// Wait for a while depending on what character we just showed
			if (Char.IsLetterOrDigit(rawText[index]))
			{
				EventDispatcher.Dispatch(SpeakingEvent.Prepare(character, rawText[index], LetterDelay));
				yield return StartCoroutine(SkippableWaitForSeconds(LetterDelay));
			}
			else if (rawText[index] == '.')
			{
				yield return StartCoroutine(SkippableWaitForSeconds(SentanceDelay));
			}
			else if (rawText[index] == ' ')
			{
				yield return StartCoroutine(SkippableWaitForSeconds(SpaceDelay));
			}
			else
			{
				yield return StartCoroutine(SkippableWaitForSeconds(LetterDelay));
			}

			// Go to the next character
			index++;
		}

		// Wait a frame to avoid accidentally skipping to next dialogue
		yield return null;

		// Clear the last selected option
		selectedOption = null;

		// If there's dialogue options, display them
		if (d.dialogueOptions.Length > 0)
		{
			// if there's ever more options than there are buttons, spawn some more
			while (d.dialogueOptions.Length > dialogueOptions.Count)
			{
				dialogueOptions.Add(Instantiate(dialogueOptionPrefab, transform.parent));
			}

			// Show as many buttons as there are options, and set them up 
			for (int i = 0; i < d.dialogueOptions.Length; i++)
			{
				dialogueOptions[i].gameObject.SetActive(true);
				dialogueOptions[i].Setup(d.dialogueOptions[i], this);			
			}

			// the button's onclick has been set to a method that will make selected option not null
			while (selectedOption == null)
			{
				yield return null;
			}
		}
		// otherwise wait for any input
		else
		{
			// Show that continue prompt if we're waiting for input
			ContinuePrompt.SetActive(true);
			// If we're supposed to wait for input, wait for that here
			while (d.WaitForInput && !Input.GetMouseButtonUp(0))
			{
				yield return null;
			}
		}

		// Emit an event saying the dialogue is over
		EventDispatcher.Dispatch(DialogueEvent.Prepare(d, false));

	}

	public void SetSelectedOption(string option)
	{
		selectedOption = option;
	}

	IEnumerator SkippableWaitForSeconds(float time)
	{
		var start = Time.time;

		while (Time.time < start + time)
		{
			yield return null;

			if (Input.GetMouseButtonUp(0))
			{
				break;
			}
		}
	}
}
