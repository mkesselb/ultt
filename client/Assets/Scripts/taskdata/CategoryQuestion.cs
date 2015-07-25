using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Category uestions represent the task of assigning category members to the matching category.
/// This class provides functionality to manage data for such tasks and to perform answer checking.
/// </summary>
public class CategoryQuestion : TaskQuestion{

	/// <summary>
	/// String name of the category.
	/// </summary>
	string categoryName;

	/// <summary>
	/// List of strings of category members.
	/// </summary>
	List<string> categoryMembers;

	/// <summary>
	/// Initializes a new instance of the <see cref="CategoryQuestion"/> class.
	/// </summary>
	/// 
	/// <param name="catName">string category name.</param>
	/// <param name="catMembers">list of string category members.</param>
	public CategoryQuestion(string catName, List<string> catMembers) : base(catName){
		categoryName = catName;
		categoryMembers = catMembers;
	}

	/// <summary>
	/// Gets the name of the category.
	/// </summary>
	/// 
	/// <returns>The category name.</returns>
	public string getCategoryName(){
		return categoryName;
	}

	/// <summary>
	/// Checks a single member against the category members of this question. Intended to be used for incremental validation.
	/// </summary>
	/// 
	/// <returns>1 if the parameter answer is contained in the category, 0 otherwise.</returns>
	/// 
	/// <param name="answer">answer string.</param>
	public double checkSingleAnswer(string answer){
		//checks for contain of single answer, e.g. when single phrases have to be matched to a category
		if (categoryMembers.Contains (answer)) {
			return 1;
		}
		return 0;
	}
	
	/// <summary>
	/// Checks the parameter answer against the internal answer.
	/// Is intended to be used with a user-supplied answer (parameter) against the internal correct answer (which is built
	/// from the saved task configuration).
	/// 
	/// Checks the category member list of the parameter answer against the internal category member list.
	/// </summary>
	/// 
	/// <returns>A double score of the parameter answer [0, 1], how good the answer members match this category members.</returns>
	/// 
	/// <param name="answer">answer task question.</param>
	override public double checkAnswer(TaskQuestion answer){
		//checks all answers and returns a double fraction of correct answers
		CategoryQuestion ans = (CategoryQuestion)answer;
		if(!ans.getCategoryName().Equals(categoryName)){
			//false categoryQuestion was called to check answer
			return -1;
		}

		double correct = 0;
		List<string> aw = (List<string>)ans.getAnswer();
		foreach (string s in aw) {
			if(categoryMembers.Contains(s)){
				correct++;
			}
		}
		
		return (correct / categoryMembers.Count);
	}

	/// <returns>The list of category members.</returns>
	override public object getAnswer(){
		return categoryMembers;
	}

	/// <summary>
	/// Gets the CSV representation of this TaskQuestion. Implementation is left for extention classes.
	/// This method is important for .csv string message exchange with the server.
	/// 
	/// .csv representation of a category question is the category name, followed by the cateogry members:
	/// catName,catMember1,...,catMemberN
	/// </summary>
	/// 
	/// <returns>The CSV representation.</returns>
	override public string getCSVRepresentation(){
		string csv = CSVHelper.swapEncode(categoryName);
		foreach(string s in categoryMembers){
			csv += "," + CSVHelper.swapEncode(s);
		}

		return csv;
	}

	/// <summary>
	/// Shuffles the category members.
	/// </summary>
	override public void shuffleAnswers(){
		Shuffle.shuffle (categoryMembers);
	}

	/// <summary>
	/// Adds a string member to this category. the member.
	/// </summary>
	public void addMember(string member){
		this.categoryMembers.Add (member);
	}
}