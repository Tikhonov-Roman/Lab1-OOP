using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lab1.Courses;
using Lab1.People;
using Xunit;

namespace Lab1.Tests.Courses;

[TestSubject(typeof(CourseManager))]
public class CourseManagerTest
{
    private readonly CourseManager _courseManager;

    public CourseManagerTest()
    {
        _courseManager = new CourseManager();
    }

    [Fact]
    public void AddCourse_ValidData_Success()
    {
        Course course = new OnlineCourse("412342", "TEST COURSE", 52, "ZOOM", "https://temp");
        _courseManager.AddCourse(course);

        var returnedCourse = _courseManager.GetCourseById(course.Id);
        Assert.Same(course, returnedCourse);
    }

    [Fact]
    public void AddCourse_WithDuplicateId_ThrowsException()
    {
        Course course = new OnlineCourse("412342", "TEST COURSE", 52, "ZOOM", "https://temp");
        _courseManager.AddCourse(course);

        Assert.Throws<InvalidOperationException>(() =>
            _courseManager.AddCourse(new OnlineCourse(course.Id, "Duplicate course", 10, "ZOOM", "https://temp")));
    }

    [Fact]
    public void AddStudentToCourse_WithDuplicateId_ThrowsException()
    {
        Course course = new OnlineCourse("412342", "TEST COURSE", 52, "ZOOM", "https://temp");
        _courseManager.AddCourse(course);
        Student student = new Student("123456", "Вася");
        _courseManager.AddStudent(course.Id, student);

        Assert.Throws<InvalidOperationException>(() =>
            _courseManager.AddStudent(course.Id, new Student(student.Id, "Name")));
    }

    [Fact]
    public void GetCourse_ValidCourseId_GetsSuccessfully()
    {
        Course course = new OnlineCourse("123456", "TEST COURSE", 52, "ZOOM", "https://temp");
        _courseManager.AddCourse(course);

        Assert.Same(course, _courseManager.GetCourseById("123456"));
    }

    [Fact]
    public void GetCourse_NonExistentId_ThrowsException()
    {
        Assert.Throws<KeyNotFoundException>(() =>
            _courseManager.GetCourseById("123456"));
    }

    [Fact]
    public void RemoveCourse_ValidCourse_RemovesSuccessfully()
    {
        var id = "123456";
        Course course = new OnlineCourse(id, "Курс", 5, "Google Meet", "https://GoogleMeet.com");
        _courseManager.RemoveCourse(id);
        Assert.DoesNotContain(course, _courseManager.GetAllCourses());
    }

    [Fact]
    public void RemoveCourse_NonExistentCourse_ThrowsException()
    {
        Assert.False(_courseManager.RemoveCourse("654321"));
    }

    [Fact]
    public void AddStudentToCourse_OverCapacity_ThrowsException()
    {
        Course course = new OnlineCourse("412342", "TEST COURSE", 1, "ZOOM", "https://temp");
        _courseManager.AddCourse(course);
        Student student = new Student("123456", "Вася");
        _courseManager.AddStudent(course.Id, student);

        Assert.Throws<InvalidOperationException>(() =>
            _courseManager.AddStudent(course.Id, new Student("654321", "Петя")));
    }
}