namespace Bash.Commands
{
    public static class Nginx
    {
        // sudo nginx -s reload

        public static Command Reload()
        {
            return new Command("nginx -s reload", sudo: true);
        }
    }
}
