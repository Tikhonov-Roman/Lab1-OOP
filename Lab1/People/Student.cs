namespace Lab1.People;

public class Student(string id, string fullName) : User(id, fullName)
{
    public override string ToString() => $"Студент: {FullName} (ID: {Id})";
    
  
}