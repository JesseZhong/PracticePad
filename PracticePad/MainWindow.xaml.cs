#region Usings
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text;
#endregion

namespace PracticePad {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		#region Dynamic Resources
		private string m_ButtonForeground = "ButtonForeground";
		private string m_ButtonActiveForeground = "ButtonActiveForeground";
		#endregion

		#region Math Symbols
		private const char DIVIDE_SIGN = '\u00F7';
		private const char MULTIPLY_SIGN = '\u00D7';
		private const char ADD_SIGN = '\u002B';
		private const char SUBTRACT_SIGN = '\u2212';
		#endregion

		#region Prompt Constants
		private const int PROMPT_TERM_LEN = 5;
		private const int PERC_FIRST_NEGATIVE = 20;
		private const int MAX_TOKENS_IN_PROMPT = 3;
		#endregion

		public MainWindow() {
			InitializeComponent();

			SetNumLockButtonForeground(m_IsNumLockActive);

			CreateNewPrompt();
        }





		#region Prompt Creation and Input Processing
		/// <summary>
		/// Receive user input and display to the input box.
		/// </summary>
		/// <param name="userInput"></param>
		private static readonly List<char> mathSigns = new List<char>() { DIVIDE_SIGN, MULTIPLY_SIGN, ADD_SIGN, SUBTRACT_SIGN };
		private void ProcessInput(char userInput) {

			if (InputBox.Text.Length < 1)
				return;

			// Error correction section

			char lastChar = InputBox.Text[InputBox.Text.Length - 1];
			if (mathSigns.Contains(userInput) && mathSigns.Contains(lastChar)) {
				StringBuilder strBldr = new StringBuilder(InputBox.Text);
				strBldr[strBldr.Length - 1] = userInput;
				InputBox.Text = strBldr.ToString();
				return;
			}

			if ((InputBox.Text == "0")) {

				if (userInput == '.')
					InputBox.Text = "0";
				else
					InputBox.Text = "";
			}

			InputBox.Text += userInput;
		}

		/// <summary>
		/// 
		/// </summary>
		private void ValidateInputPhrase() {

			// If the user input is correct, generate new input and shift the previous prompts.
			if(m_CurrentPrompt == InputBox.Text) {


				// Create a new prompt.
				CreateNewPrompt();
			}

			// Clear the input box.
			InputBox.Text = "0";
		}

		/// <summary>
		/// Creates a new prompt phrase and displays it in the history log.
		/// </summary>
		private void CreateNewPrompt() {
			m_CurrentPrompt = GeneratePhrase();

			StringBuilder strBldr = new StringBuilder(HistoryLog.Text);
			strBldr.Insert(0, String.Format("{0}\n", m_CurrentPrompt));
			HistoryLog.Text = strBldr.ToString();
		}

		/// <summary>
		/// Generates a random number or math equation.
		/// Uses PROMPT_TERM_LEN to determine how many characters are in each numeric term.
		/// Uses PERC_FIRST_NEGATIVE to determine the chance the first number is negative.
		/// Uses MAX_TOKENS_IN_PROMPT to determine the number of +,-,*,/,. between terms.
		/// </summary>
		/// <returns></returns>
		private string GeneratePhrase() {

			string phrase = "";

			// New seeded random.
			Random rand = new Random();

			int numOfTokens = rand.Next(0, MAX_TOKENS_IN_PROMPT);

			// Start by adding a term (string of numeric characters)
			// and add +,-,*,/,. accordingly. The string phrase should
			// always end with another term.
			for (int index = 0; index < (numOfTokens + 1); index++) {

				// If this isn't the first iteration, append a token.
				if (index != 0) {

					switch (rand.Next(0, 4)) {
						case 0:
							phrase += '.';
							break;
						case 1:
							phrase += DIVIDE_SIGN;
							break;
						case 2:
							phrase += MULTIPLY_SIGN;
							break;
						case 3:
							phrase += ADD_SIGN;
							break;
						case 4:
							phrase += SUBTRACT_SIGN;
							break;
						default:
							break;
					}
				} else {
					if (rand.Next(0, 99) < PERC_FIRST_NEGATIVE)
						phrase += SUBTRACT_SIGN;
				}
				
				// Append numbers (terms)
				int termLen = rand.Next(1, PROMPT_TERM_LEN);
				if (termLen == 1)
					phrase += rand.Next(0, 9);

				else {

					// Always lead with a non-zero character.
					phrase += ((phrase.Length > 0) && (phrase[phrase.Length - 1] != '.')) ? rand.Next(0, 9) : rand.Next(1, 9);
					for (int i = 1; i < termLen; i++) {
						phrase += rand.Next(0, 9);
					}
				}
			}

			return phrase;
		}
		#endregion

		#region Prompt Members
		private string m_CurrentPrompt;
		#endregion

		#region User Input Handlers
		/// <summary>
		/// Processes user keyboard input.
		/// Simulates the on-screen button presses.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_KeyDown(object sender, KeyEventArgs e) {

			switch(e.Key) {
				
				case Key.NumLock:
					SetNumLockButtonForeground(e.IsToggled);
					break;
				case Key.NumPad0:
					ZeroButton.PressDown();
					break;
				case Key.NumPad1:
					OneButton.PressDown();
					break;
				case Key.NumPad2:
					TwoButton.PressDown();
					break;
				case Key.NumPad3:
					ThreeButton.PressDown();
					break;
				case Key.NumPad4:
					FourButton.PressDown();
					break;
				case Key.NumPad5:
					FiveButton.PressDown();
					break;
				case Key.NumPad6:
					SixButton.PressDown();
					break;
				case Key.NumPad7:
					SevenButton.PressDown();
					break;
				case Key.NumPad8:
					EightButton.PressDown();
					break;
				case Key.NumPad9:
					NineButton.PressDown();
					break;
				case Key.Divide:
					DivButton.PressDown();
					break;
				case Key.Multiply:
					MulButton.PressDown();
					break;
				case Key.Subtract:
					SubButton.PressDown();
					break;
				case Key.Add:
					AddButton.PressDown();
					break;
				case Key.Enter:
					EnterButton.PressDown();
					break;
				case Key.Decimal:
					DecimalButton.PressDown();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Processes user keyboard input.
		/// Simulates the on-screen button presses.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_KeyUp(object sender, KeyEventArgs e) {
			switch (e.Key) {
				
				case Key.NumLock:
					SetNumLockButtonForeground(e.IsToggled);
					break;
				case Key.NumPad0:
					ZeroButton.PressUp();
					break;
				case Key.NumPad1:
					OneButton.PressUp();
					break;
				case Key.NumPad2:
					TwoButton.PressUp();
					break;
				case Key.NumPad3:
					ThreeButton.PressUp();
					break;
				case Key.NumPad4:
					FourButton.PressUp();
					break;
				case Key.NumPad5:
					FiveButton.PressUp();
					break;
				case Key.NumPad6:
					SixButton.PressUp();
					break;
				case Key.NumPad7:
					SevenButton.PressUp();
					break;
				case Key.NumPad8:
					EightButton.PressUp();
					break;
				case Key.NumPad9:
					NineButton.PressUp();
					break;
				case Key.Divide:
					DivButton.PressUp();
					break;
				case Key.Multiply:
					MulButton.PressUp();
					break;
				case Key.Subtract:
					SubButton.PressUp();
					break;
				case Key.Add:
					AddButton.PressUp();
					break;
				case Key.Enter:
					EnterButton.PressUp();
					break;
				case Key.Decimal:
					DecimalButton.PressUp();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Toggle the on-screen num lock button.
		/// </summary>
		/// <param name="state"></param>
		private void SetNumLockButtonForeground(bool state) {
			NumLkButton.Foreground = (Brush)(state ? FindResource(m_ButtonActiveForeground) : FindResource(m_ButtonForeground));
		}
		#endregion

		#region User Input Members
		private bool m_IsNumLockActive = Keyboard.IsKeyToggled(Key.NumLock);
		#endregion

		#region Button Click Handlers
		private void EnterButton_Click(object sender, RoutedEventArgs e) {
			ValidateInputPhrase();
		}

		private void DecimalButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('.');
		}

		private void ZeroButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('0');
		}

		private void OneButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('1');
		}

		private void TwoButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('2');
		}

		private void ThreeButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('3');
		}

		private void FourButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('4');
		}

		private void FiveButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('5');
		}

		private void SixButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('6');
		}

		private void SevenButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('7');
		}

		private void EightButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('8');
		}

		private void NineButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput('9');
		}

		private void AddButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput(ADD_SIGN);
		}

		private void SubButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput(SUBTRACT_SIGN);
		}

		private void MulButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput(MULTIPLY_SIGN);
		}

		private void DivButton_Click(object sender, RoutedEventArgs e) {
			ProcessInput(DIVIDE_SIGN);
		}
		#endregion
	}
}
