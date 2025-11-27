using Lab1.Courses;
using Lab1.Input;
using Lab1.People;

namespace Lab1;

public class CourseManagementApp
{
    private readonly CourseManager _courseManager = new();
    private readonly Dictionary<string, Teacher> _teachers = new();
    private readonly Dictionary<string, Student> _students = new();
    private readonly Dictionary<string, (string description, Action function)> _operations;

    public IReadOnlyDictionary<string, Teacher> Teachers => _teachers;
    public IReadOnlyDictionary<string, Student> Students => _students;
    public IReadOnlyList<Course> Courses => _courseManager.GetAllCourses();
    
    
    public CourseManagementApp()
    {
        Console.WriteLine("Вас приветствует Система управления курсами и преподавателями!");
        _operations =
            new Dictionary<string, (string description, Action function)>
            {
                { "1", ("Добавить учителя", () => CreateUser("teacher")) },
                { "2", ("Добавить студента", () => CreateUser("student")) },
                { "3", ("Добавить курс", CreateCourse) },
                { "4", ("Удалить курс", DeleteCourse) },
                { "5", ("Добавить студента на курс", AddStudentToCourse) },
                { "6", ("Назначить учителя на курс", SetTeacherToCourse) },
                { "7", ("Получить все курсы преподавателя", ShowCoursesFilteredByTeacher) },
                { "8", ("Отобразить студентов записанных на курс", ShowCourseStudents) },
                { "9", ("Отобразить всех преподавателей", ShowAllTeachers) },
                { "10", ("Отобразить всех студентов", ShowAllStudents) },
                { "11", ("Отобразить все курсы", ShowAllCourses) },
            };
    }

    public void Start(string returnCode)
    {
        _operations[returnCode] = ("Завершение работы программы", () => { });
        while (true)
        {
            ShowOperations();
            var operationNumber = Console.ReadLine();
            if (operationNumber == returnCode) return;
            if (operationNumber == null || !_operations.Keys.Contains(operationNumber))
                Console.WriteLine("Неизвестная команда.");
            else
            {
                try
                {
                    Console.WriteLine();
                    _operations[operationNumber].function();
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка при выполнении операции: {e.Message}");
                }
            }
        }
    }


    private void ShowOperations()
    {
        Console.WriteLine("Список доступных операций:");
        foreach (var operationNumber in _operations.Keys)
        {
            Console.WriteLine($"{operationNumber}) {_operations[operationNumber].description}");
        }

        Console.Write("Выберите действие: ");
    }

    private void CreateUser(string type)
    {
        var userName = UserInput.ReadNotBlankLine("Введите ФИО: ");
        var userId = UserInput.ReadId("Введите ID пользователя(6 цифр).", User.IsValidId);
        while (!IsUserIdFree(userId))
            userId = UserInput.ReadId("Введите корректный ID пользователя(6 цифр).", User.IsValidId);
        if (type == "teacher")
        {
            var teacher = new Teacher(userId, userName);
            _teachers[teacher.Id] = teacher;
            Console.WriteLine($"Преподаватель добавлен: {teacher.FullName} (ID: {teacher.Id})");
        }
        else
        {
            Student student = new Student(userId, userName);
            _students[student.Id] = student;
            Console.WriteLine($"Студент добавлен: {student.FullName} (ID: {student.Id})");
        }
    }


    private void CreateCourse()
    {
        var courseId = UserInput.ReadId("Введите ID курса (6 цифр): ", Course.IsValidId);
        while (!_courseManager.IsIdFree(courseId))
            courseId = UserInput.ReadId("Введите корректный ID курса (6 цифр): ", Course.IsValidId);
        string name = UserInput.ReadNotBlankLine("Введите название курса: ");
        var capacity = UserInput.ReadPositiveInt("Введите вместимость курса: ");
        
        var courseType = UserInput.ReadPositiveInt("Тип курса (1 - онлайн, 2 - офлайн):");

        var course = courseType switch
        {
            1 => CreateOnlineCourse(courseId, name, capacity),
            2 => CreateOfflineCourse(courseId, name, capacity),
            _ => null
        };

        if (course is null)
        {
            Console.WriteLine("Неизвестный тип курса.");
            return;
        }

        _courseManager.AddCourse(course);
        Console.WriteLine($"Курс добавлен: {course.Name} (ID: {course.Id})");
    }

    private Course CreateOnlineCourse(string id, string name, int capacity)
    {
        var platform = UserInput.ReadNotBlankLine("Платформа: ");
        var url = UserInput.ReadNotBlankLine("Ссылка на занятие: ");
        return new OnlineCourse(id, name, capacity, platform, url);
    }

    private Course CreateOfflineCourse(string id, string name, int capacity)
    {
        var classroomName = UserInput.ReadNotBlankLine("Аудитория: ");
        var campus = UserInput.ReadNotBlankLine("Кампус: ");
        return new OfflineCourse(id, name, capacity, classroomName, campus);
    }

    private void DeleteCourse()
    {
        var course = SelectCourse();
        if (course is null)
        {
            return;
        }

        Console.WriteLine(
            _courseManager.RemoveCourse(course.Id)
                ? $"Курс с ID={course.Id} удалён."
                : $"Ошибка при удалении курса с ID={course.Id}");
    }

    private void SetTeacherToCourse()
    {
        var teacher = SelectTeacher();
        if (teacher is null)
        {
            return;
        }

        Course? course = SelectCourse();
        if (course is null)
        {
            return;
        }


        _courseManager.SetTeacher(course.Id, teacher);
        Console.WriteLine($"Преподаватель {teacher.FullName} назначен на  {course}.");
    }

    private void AddStudentToCourse()
    {
        var course = SelectCourse();
        if (course is null)
        {
            return;
        }

        ShowAllStudents();
        var studentId = UserInput.ReadId("Введите ID студента: ", User.IsValidId);
        while (!_students.ContainsKey(studentId))
        {
            ShowAllStudents();
            studentId = UserInput.ReadId("Введите ID студента из списка: ", User.IsValidId);
        }

        var student = _students[studentId];
        _courseManager.AddStudent(course.Id, student);
        Console.WriteLine($"Студент {student.FullName} записан на курс {course.Name}.");
    }

    private void ShowCourseStudents()
    {
        var course = SelectCourse();
        if (course is null)
        {
            return;
        }

        if (course.GetStudents().Count == 0)
        {
            Console.WriteLine($"На курсе с (ID: {course.Id}) нет студентов");
            return;
        }

        Console.WriteLine($"Студенты курса {course.Name}:");
        foreach (var student in course.GetStudents())
        {
            Console.WriteLine($"- {student.FullName} (ID: {student.Id})");
        }
    }

    private void ShowCoursesFilteredByTeacher()
    {
        var teacher = SelectTeacher();
        if (teacher is null) return;


        var courses = _courseManager.GetCoursesByTeacher(teacher.Id);
        if (courses.Count == 0)

        {
            Console.WriteLine($"За данным преподавателем (ID: {teacher.Id} не закреплены курсы.");
            return;
        }

        Console.WriteLine($"Курсы преподавателя {teacher.FullName} (ID: {teacher.Id}):");
        foreach (var course in courses)
        {
            Console.WriteLine($"- {course}");
        }
    }

    private void ShowAllCourses()
    {
        if (_courseManager.GetAllCourses().Count == 0)
        {
            Console.WriteLine("Курсы отсутствуют.");
            return;
        }

        Console.WriteLine("Все курсы:");
        foreach (var course in _courseManager.GetAllCourses())
        {
            var teacherName = course.Teacher?.FullName ?? "не назначен";
            if (course.Teacher != null)
                Console.WriteLine($"- {course} | Преподаватель: {course.Teacher.FullName} (ID: {course.Teacher.Id})");
            else Console.WriteLine($"- {course} | Преподаватель: неназначен");
        }
    }

    private void ShowAllTeachers()
    {
        if (_teachers.Count == 0)
        {
            Console.WriteLine("Преподаватели отсутствуют.");
            return;
        }

        Console.WriteLine("Все преподаватели:");
        foreach (var teacher in _teachers.Values)
        {
            Console.WriteLine($"- {teacher.FullName} (ID: {teacher.Id})");
        }
    }

    private void ShowAllStudents()
    {
        if (_students.Count == 0)
        {
            Console.WriteLine("Студенты отсутствуют.");
            return;
        }

        Console.WriteLine("Все студенты:");
        foreach (var student in _students.Values)
        {
            Console.WriteLine($"- {student.FullName} (ID: {student.Id})");
        }
    }

    private Course? SelectCourse()
    {
        List<Course> courses = _courseManager.GetAllCourses().ToList();
        if (courses.Count == 0)
        {
            Console.WriteLine("Курсы отсутствуют.");
            return null;
        }

        Console.WriteLine("Выберите курс:");
        ShowAllCourses();

        while (true)
        {
            var id = UserInput.ReadId("Введите ID курса: ", Course.IsValidId);
            try
            {
                return _courseManager.GetCourseById(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }

    private Teacher? SelectTeacher()
    {
        if (_teachers.Count == 0)
        {
            Console.WriteLine("Сначала добавьте преподавателя.");
            return null;
        }

        Console.WriteLine("Выберите преподавателя:");

        ShowAllTeachers();


        var id = UserInput.ReadId("Введите ID преподавателя: ", User.IsValidId);
        while (!_teachers.ContainsKey(id))
        {
            id = UserInput.ReadId("Введите ID преподавателя: ", User.IsValidId);
        }

        return _teachers[id];
    }

    private bool IsUserIdFree(string userId)
    {
        return !(_teachers.ContainsKey(userId) || _students.ContainsKey(userId));
    }
}