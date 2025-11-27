using Lab1.People;

namespace Lab1.Courses;

public abstract class Course
{
    private readonly List<Student> _students = [];
    public string Id { get; }

    public string Name { get; }

    public Teacher? Teacher { get; private set; }

    public int Capacity { get; }
    public IReadOnlyCollection<Student> GetStudents() => _students;
    
    protected Course(string id, string name, int capacity)
    {
      
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Имя курса является обязательным параметром и не может быть пустым.", nameof(name));
        }

        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Вместимость должна быть больше 0");
        }

        Id = id ??   throw new ArgumentException("Id курса является обязательным параметром и не может быть пустым.", nameof(id));
        Name = name.Trim();
        Capacity = capacity;
    }

    
    public void SetTeacher(Teacher teacher)
    {
        Teacher = teacher ?? throw new ArgumentNullException(nameof(teacher));
    }

    public void AddStudent(Student student)
    {
        if (student is null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (_students.Any(s => s.Id == student.Id))
        {
            throw new InvalidOperationException("Данный студент уже добавлен.");
        }

        if (_students.Count >= Capacity)
        {
            throw new InvalidOperationException("Данный курс заполнен");
        }

        _students.Add(student);
    }

    public static bool IsValidId(string id)
    {
        if (int.TryParse(id, out _))
        {
            return id.Length == 6;
        }

        return false;
    }
}