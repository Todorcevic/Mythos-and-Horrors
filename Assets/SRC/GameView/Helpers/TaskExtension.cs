using System.Threading.Tasks;

namespace MythosAndHorrors.GameView
{
    public static class TaskExtension
    {
        public static Task Join(this Task thisTask, Task anotherTask)
        {
            return Task.WhenAll(thisTask, anotherTask);
        }
    }
}
