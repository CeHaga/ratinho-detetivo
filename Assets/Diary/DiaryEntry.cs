using System;

[System.Serializable]
public class DiaryEntry {
	public string name;
	public string description;
	public string hint;
	public DiaryEntryType type;
	
	public DiaryEntry(string name, string description, string hint, DiaryEntryType type) {
		this.name = name;
		this.description = description;
		this.hint = hint;
		this.type = type;
	}
	
	public override string ToString() {
		string typeName = "";
		switch (type) {
			case DiaryEntryType.DIALOG:
				typeName = "Dialog";
				break;
			case DiaryEntryType.ITEM:
				typeName = "Item";
				break;
		}
		return "Name: " + name + "\nDescription: " + description + "\nHint: " + hint + "\nType: " + typeName + "\n";
	}
	
	public override bool Equals(object obj) {
		if (obj == null || GetType() != obj.GetType()) {
			return false;
		}
		
		DiaryEntry entry = (DiaryEntry) obj;
		return name == entry.name && type == entry.type;
	}
}
