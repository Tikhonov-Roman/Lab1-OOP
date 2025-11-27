namespace Lab1.People;

public class Teacher(string id, string fullName) : User(id, fullName)
{
    public override string ToString() => $"Преподаватель: {FullName} (ID: {Id})";
    
    
}