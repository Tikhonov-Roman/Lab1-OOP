using System;
using JetBrains.Annotations;
using Lab1.People;
using Xunit;

namespace Lab1.Tests.People;

[TestSubject(typeof(User))]
public class UserTest
{

    [Fact]
    public void IsValidId_ValidSixDigitId_ReturnsTrue()
    {
        var result = User.IsValidId("123456");
        Assert.True(result);
    }
    
    [Fact]
    public void IsValidId_InvalidSixDigitId_ReturnsFalse()
    {
        var result = User.IsValidId("123");
        Assert.False(result);
    }
    
    [Fact]
    public void Student_ValidData_CreatesSuccessfully()
    {

        var id = "123456";
        var fullName = "Иван Иванов";
        
        var student = new Student(id, fullName);

        Assert.Equal(id, student.Id);
        Assert.Equal(fullName, student.FullName);
    }

    [Fact]
    public void Student_InvalidId_ThrowsArgumentException()
    {

        var invalidId = "12345";
        var fullName = "Иван Иванов";

       
        Assert.Throws<ArgumentException>(() => new Student(invalidId, fullName));
    }

    [Fact]
    public void Student_EmptyFullName_ThrowsArgumentException()
    {
      
        var id = "123456";
        var emptyName = "";
        
        Assert.Throws<ArgumentException>(() => new Student(id, emptyName));
    }
    
}