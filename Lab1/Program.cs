using Lab1.Courses;
using Lab1.People;

namespace Lab1;

public static class Program
{
    private static void Main()
    {
        CourseManagementApp app = new CourseManagementApp();
        app.Start("0");
    }
}

