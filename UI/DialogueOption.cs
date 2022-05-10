using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogueOption : MonoBehaviour
{
	public TMP_Text Text;
	public Button Button;

	private string option = null;
	private DialogueDisplay display;

	public void Setup(string optionText, DialogueDisplay d)
	{
		option = optionText;
		Text.text = option;

		display = d;

		// Set the selected option field to the option on click
		Button.onClick.AddListener(SetSelectedOption);
		Button.interactable = true;

	}

	public void Clear()
	{
		Button.onClick.RemoveAllListeners();
		Button.interactable = false;
		Text.text = string.Empty;
	}

	public void SetSelectedOption()
	{
		display.SetSelectedOption(option);
	}
}
