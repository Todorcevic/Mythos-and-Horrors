using System.Threading.Tasks;

namespace MythosAndHorrors.GameView
{
    public static class TaskExtension
    {
        public static async Task Join(this Task thisTask, Task anotherTask)
        {
            await Task.WhenAll(thisTask, anotherTask);
        }
    }
}
