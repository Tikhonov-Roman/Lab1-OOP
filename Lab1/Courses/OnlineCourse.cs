namespace Lab1.Courses;

public sealed class OnlineCourse : Course
{
    
    public string Platform { get; }

    public string MeetingUrl { get; }
    
    public override string ToString() =>
        $"Онлайн-курс (ID: {Id}) на платформе {Platform}. Ссылка: {MeetingUrl}.";
    
    
    public OnlineCourse(string id, string name, int capacity, string platform, string meetingUrl)
        : base(id, name, capacity)
    {
        if (string.IsNullOrEmpty(platform))
        {
            throw new ArgumentException("Платформа является обязательным параметром и не может быть пустой.", nameof(platform));
        }

        if (string.IsNullOrEmpty(meetingUrl))
        {
            throw new ArgumentException("Ссылка является обязательным параметром и не может быть пустой.", nameof(meetingUrl));
        }

        Platform = platform.Trim();
        MeetingUrl = meetingUrl.Trim();
    }


}

