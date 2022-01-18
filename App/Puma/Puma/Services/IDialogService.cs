using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Puma.Services
{
    public interface IDialogService
    {
        Task ShowErrorAsync(string title, string message, string buttonText);
        Task ShowErrorAsync(string title, Exception error, string buttonText);
        Task ShowMessageAsync(string title, string message);
        Task ShowMessageAsync(string title, string message, string buttonText);
    }
}
