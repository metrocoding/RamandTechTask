namespace WebApi.Application.Utils;

public static class Utils
{
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException();
        }

        return password;
    }
}