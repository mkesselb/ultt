using System;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Locale handler utility class, offering static methods that deal with the translation of messages.
/// Reads the needed locale file from the resources in './locales/localeName.txt', and sets up an internal map of localized messages.
/// </summary>
public class LocaleHandler
{
	private static string lang = "";

	/// <summary>
	/// A nested localization dict. The outer dict represents the mapping from locale-string to a dict of actual localized messages.
	/// The inner dict represents the mapping of the message-identifier strings to the actual localization, of the outer language.
	/// </summary>
	private static Dictionary<string, Dictionary<string, string>> localeTranslation = new Dictionary<string, Dictionary<string, string>>();

	/// <summary>
	/// Setups the localization mapping for the parameter locale-string.
	/// This is done by trying to fetch the 'locale.txt' file in the resources './locales/', and populating the localization dict for the parameter locale.
	/// 
	/// This method has to be called before a meaningful localization can happen for any locale.
	/// </summary>
	/// 
	/// <param name="locale">string name of the locale to be fetched.</param>
	public static void setupMapping(string locale){
		//read from ./locales/locale.txt into the dict of errorcodes
		Dictionary<string, string> dict = new Dictionary<string, string> ();

		if (locale == "de") {
			dict.Add("dialog-yes", "Ja");
			dict.Add("dialog-no", "Nein");
			dict.Add("noserver-connection", "Es konnte keine Verbindung zum Server hergstellt werden.");
			dict.Add("back-button", "zur\u00FCck");
			dict.Add("logout-button", "ausloggen");
			dict.Add("login-button", "einloggen");
			dict.Add("register-button", "anmelden");
			dict.Add("register-info", "Bitte Anmeldedaten eingeben");
			dict.Add("class-tab", "Meine Klassen");
			dict.Add("course-tab", "Meine Kurse");
			dict.Add("task-tab", "Meine Aufgaben");
			dict.Add("add-class", "Klasse hinzuf\u00FCgen");
			dict.Add("class-add-info", "Bitte Klassendaten eingeben:");
			dict.Add("class-name", "Klassenname");
			dict.Add("class-year", "Schuljahr");
			dict.Add("class-subject", "Fach");
			dict.Add("button-add-class", "erstellen");
			dict.Add("student-results", "Sch\u00FClerinnen-ergebnisse");
			dict.Add("button-add-topic", "Thema hinzuf\u00FCgen");
			dict.Add("button-add-topicok", "OK");
			dict.Add("topicname-info", "Themenname eintragen");
			dict.Add("button-add-tasktotopic", "Aufgabe hinzuf\u00FCgen");
			dict.Add("task-exercise", " (\u00DCbung)");
			dict.Add("task-exam", " (Pr\u00FCfung)");
			dict.Add("info-select-task", "Aufgabe ausw\u00E4hlen");
			dict.Add("info-task-maxattempts", "maximale Anzahl der Versuche");
			dict.Add("info-notobligatory", "\u00DCbung");
			dict.Add("info-obligatory", "\u00DCberpr\u00FCfung");
			dict.Add("add-course", "Kursregistrierung");
			dict.Add("course-info1", "Um dich zu einem Kurs anmelden zu k\u00F6nnen, ben\u00F6tigst du den Anmeldungscode des Kurses. Diesen bekommst du vom Lehrer des Kurses.");
			dict.Add("course-info2", "Wenn du den Code in das untenstehende Feld eingibst und best\u00E4tigst, bist du in diesem Kurs angemeldet. ");
			dict.Add("course-info3", "Der Lehrer entscheidet, ob er dich in den Kurs aufnehmen will. Du kannst den Kurs dann in deinem Profil unter \"Meine Kurse\" finden.");
			dict.Add("course-code-info", "Code eingeben:");
			dict.Add("button-add-course", "registrieren");
			dict.Add("add-task", "Aufgabenerstellung");
			dict.Add("task-add-info", "Bitte Daten der Aufgabe eingeben:");
			dict.Add("task-name", "Name");
			dict.Add("task-privacy", "\u00F6ffentlich?");
			dict.Add("task-subject", "Fach");
			dict.Add("tasktype", "Aufgabentyp");
			dict.Add("button-add-task", "erstellen");
			dict.Add("quiz-info", "Bearbeite das Quiz, indem du Fragen und Antworten hinzuf\u00FCgst. Markiere die richtigen Antworten.");
			dict.Add("button-quiz-add-answer", "Antwort hinzuf\u00FCgen");
			dict.Add("button-quiz-add-question", "Frage hinzuf\u00FCgen");
			dict.Add("button-quiz-save", "speichern");
			dict.Add("cat-info", "");
			dict.Add("button-cat-addphrase", "Phrase hinzuf\u00FCgen");
			dict.Add("button-cat-addcat", "Kategorie hinzuf\u00FCgen");
			dict.Add("button-cat-save", "speichern");
			dict.Add("assig-info", "");
			dict.Add("button-assig-add", "Zuordnung hinzuf\u00FCgen");
			dict.Add("button-assig-save", "speichern");
			dict.Add("info-nextword", "N\u00E4chstes Wort");
			dict.Add("info-cat-num-answers", "Bearbeitet: ");
			dict.Add("info-cat-num-correct", "Richtig: ");
			dict.Add("button-cat-next", "n\u00E4chstes Wort");
			dict.Add("button-cat-end", "Ergebnisse pr\u00FCfen");
			dict.Add("date-invalid", "Das gew\u00E4hlte Datum passt nicht zum Format: ");
			dict.Add("num-invalid", "Dies ist keine Nummer: ");
			dict.Add("num-invalid-range", "Die Nummer ist nicht im folgenden Bereich: ");
			dict.Add("text-empty", "Der Text darf nicht leer sein");
			dict.Add("text-range", "Die L\u00E4nge des Textes muss im folgenden Bereich sein: ");
			dict.Add("text-specialChar", "Das Zeichen \" ist nicht erlaubt");
			dict.Add("valid-errors", "Eingabefehler:");
			dict.Add("taskname", "Aufgabenname");
			dict.Add("classname", "Klassenname");
			dict.Add("school_year", "Schuljahr");
			dict.Add("topic_name", "Themaname");
			dict.Add("max_attempts", "Versuche");
			dict.Add("firstname", "Vorname");
			dict.Add("lastname", "Nachname");
			dict.Add("username", "Benutzername");
			dict.Add("password", "Passwort");
			dict.Add("password2", "Passwort2");
			dict.Add("email", "Email");
			dict.Add("school", "Schule");
		}
		/*
		string line = "";

		System.IO.StreamReader file = new System.IO.StreamReader("./Assets/Scripts/locales/" + locale + ".txt");
		while((line = file.ReadLine()) != null)
		{
			if(line.Length != 0 && !line.StartsWith("#")){
				string[] parts = line.Split(new char[]{';'});
				dict.Add(parts[0], parts[1]);
			}
		}
		*/
		localeTranslation.Add (locale, dict);
	}

	public static void setLang(string newLang){
		lang = newLang;
	}

	/// <summary>
	/// Returns the localized text for the parameter message-identifier and locale strings.
	/// 
	/// If the localization cannot be found (e.g. if the locale dict is not populated, or the localization files are outdated), 'undefined' is returned.
	/// </summary>
	/// 
	/// <returns>The localized text for the parameter message-identifier and locale strings.</returns>
	/// 
	/// <param name="ident">The message-identifier to be localized.</param>
	/// <param name="locale">The locale string.</param>
	public static string getText(string ident, string locale=""){
		if (locale == "") {
			locale = lang;
		}

		if (localeTranslation.ContainsKey (locale) && localeTranslation [locale].ContainsKey (ident)) {
			return localeTranslation[locale][ident];
		}

		return "undefined";
	}
}