using Lab1.People;

namespace Lab1.Courses;

public class CourseManager
{
    private readonly Dictionary<string, Course> _courses = new();


    public void AddCourse(Course course)
    {
        if (_courses.ContainsKey(course.Id)) throw new InvalidOperationException("Курс с данным ID уже существует");
        _courses.TryAdd(course.Id, course);
    }

    public bool RemoveCourse(string courseId) => _courses.Remove(courseId);

    public Course GetCourseById(string courseId)
    {
        if (!_courses.TryGetValue(courseId, out var course))
        {
            throw new KeyNotFoundException("Данный курс не найден");
        }

        return course;
    }

    public void AddStudent(string courseId, Student student)
    {
        var course = GetCourseById(courseId);
        course.AddStudent(student);
    }

    public void SetTeacher(string courseId, Teacher teacher)
    {
        var course = GetCourseById(courseId);
        course.SetTeacher(teacher);
    }


    public IReadOnlyList<Course> GetCoursesByTeacher(string teacherId)
    {
        return _courses.Values
            .Where(course => course.Teacher?.Id == teacherId)
            .ToList();
    }

    public IReadOnlyList<Course> GetAllCourses() => _courses.Values.ToList();

    public bool IsIdFree(string id)
    {
        return !_courses.ContainsKey(id);
    }
}