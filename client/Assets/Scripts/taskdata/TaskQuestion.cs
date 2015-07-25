using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract base class for individual task questions. Should be extended by all concrete task questions.
/// TaskQuestion objects are intended for use of both correct question/answers (received from the server) as well as question/answers done by the user.
/// </summary>
public abstract class TaskQuestion{

	/// <summary>
	/// The string question text.
	/// </summary>
	private string questionText;

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskQuestion"/> class.
	/// The constructor of this abstract class only sets the parameter question text.
	/// </summary>
	/// 
	/// <param name="questionT">the string question text.</param>
	public TaskQuestion(string questionT){
		questionText = questionT;
	}

	/// <returns>The question text.</returns>
	public string getQuestionText(){
		return questionText;
	}

	/* abstract methods */

	/// <summary>
	/// Checks the parameter answer against the internal answer.
	/// Is intended to be used with a user-supplied answer (parameter) against the internal correct answer (which is built from the saved task configuration).
	/// </summary>
	/// 
	/// <returns>A double score of the parameter answer [0, 1].</returns>
	/// 
	/// <param name="answer">Answer.</param>
	public abstract double checkAnswer(TaskQuestion answer);

	/// <summary>
	/// Gets the answer of this question, as an object.
	/// </summary>
	public abstract object getAnswer();

	/// <summary>
	/// Gets the CSV representation of this TaskQuestion. Implementation is left for extention classes.
	/// This method is important for .csv string message exchange with the server.
	/// </summary>
	public abstract string getCSVRepresentation();

	/// <summary>
	/// Shuffles the answers. Implementation is left for extention classes.
	/// </summary>
	public abstract void shuffleAnswers();
}