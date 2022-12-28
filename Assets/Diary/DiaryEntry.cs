using System;

[System.Serializable]
public class DiaryEntry {
	public string name;
	public string description;
	public string hint;
	
	public DiaryEntry(string name, string description, string hint) {
		this.name = name;
		this.description = description;
		this.hint = hint;
	}
	
	public override string ToString() {
		return "Name: " + name + "\nDescription: " + description + "\nHint: " + hint + "\n";
	}
	
	public override bool Equals(object obj) {
		if (obj == null || GetType() != obj.GetType()) {
			return false;
		}
		
		DiaryEntry entry = (DiaryEntry) obj;
		return name == entry.name;
	}
}
