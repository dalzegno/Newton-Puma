using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Puma.Services
{
    public interface IDialogService
    {
        Task ShowErrorAsync(string message);
        Task ShowErrorAsync(Exception error);
        Task ShowMessageAsync(string title, string message);
        Task ShowMessageAsync(string title, string message, string buttonText);

        Task<string> ShowYesNoActionSheet(string title);
    }
}
