namespace Lab1.People;

public abstract class User
{
    public string Id { get; }

    public string FullName { get; }

    public override string ToString() => FullName;

    protected User(string id, string fullName)
    {
        if(!IsValidId(id))  throw new ArgumentException("Неверный ID пользователя. Id пользователя должен состоять только из 6 цифр", nameof(id));

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("ФИО пользователя не может быть пустым.", nameof(fullName));
        }

        Id = id ?? throw new ArgumentException("Id пользователя не может быть пустым.", nameof(id));

        FullName = fullName.Trim();
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