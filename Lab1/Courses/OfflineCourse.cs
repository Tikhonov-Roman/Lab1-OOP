namespace Lab1.Courses;

public sealed class OfflineCourse : Course
{
    public string ClassroomName { get; }

    public string Building { get; }

    public override string ToString() => $"Очный курс (ID: {Id}) {Name}, аудитория {ClassroomName}, адресс {Building}.";

    public OfflineCourse(string id, string name, int capacity, string classroomName, string building)
        : base(id, name, capacity)
    {
        if (string.IsNullOrEmpty(classroomName))
        {
            throw new ArgumentException("Аудитория является обязательным параметром и не может быть пустой.", nameof(classroomName));
        }

        if (string.IsNullOrEmpty(building))
        {
            throw new ArgumentException("Здание является обязательным параметром и не может быть пустым.", nameof(building));
        }

        ClassroomName = classroomName.Trim();
        Building = building.Trim();
    }
}