using System;
namespace Linewars.Ui
{

	/// <summary>
	/// Interface to indicate that this is interested in internationalization.
	/// </summary>
	public interface IChangeLanguage
	{
		/// <summary>
		/// Identifier this for the key in the language file.
		/// </summary>
		string Identifier();

		/// <summary>
		/// Sets the specific language text here.
		/// </summary>
		/// <param name="text">Text.</param>
		void SetText(string text);
	}
}


