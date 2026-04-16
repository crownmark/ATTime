using Microsoft.AspNetCore.Components;

namespace CrownATTime.Client.CustomComponents.DialogManager
{
    public class DialogManager
    {
        public List<AppDialog> Dialogs { get; set; } = new();

        public Action? OnChange;

        private int _nextZIndex = 1000;

        // 🔥 MAIN METHOD (what you'll use everywhere)
        public void OpenOrFocus<T>(
            int id,
            string title,
            string type,
            Dictionary<string, object>? parameters = null,
            string width = "98%")
            where T : IComponent
        {
            var existing = Dialogs.FirstOrDefault(d => d.Id == id && d.Type == type);

            if (existing != null)
            {
                existing.IsMinimized = false;
                existing.ZIndex = ++_nextZIndex;
                Notify();
                return;
            }

            var dialog = new AppDialog
            {
                Id = id,
                Type = type,
                Title = title,
                Width = width,
                ZIndex = ++_nextZIndex,
                Content = CreateComponent<T>(parameters)
            };

            Dialogs.Add(dialog);
            Notify();
        }

        public void Minimize(int id, string type)
        {
            var dlg = Dialogs.FirstOrDefault(d => d.Id == id && d.Type == type);
            if (dlg != null)
            {
                dlg.IsMinimized = true;
                Notify();
            }
        }

        public void Restore(int id, string type)
        {
            var dlg = Dialogs.FirstOrDefault(d => d.Id == id && d.Type == type);
            if (dlg != null)
            {
                dlg.IsMinimized = false;
                dlg.ZIndex = ++_nextZIndex;
                Notify();
            }
        }

        public void Close(int id, string type)
        {
            Dialogs.RemoveAll(d => d.Id == id && d.Type == type);
            Notify();
        }

        public void SetActive(int id, string type)
        {
            var dlg = Dialogs.FirstOrDefault(d => d.Id == id && d.Type == type);
            if (dlg != null)
            {
                dlg.ZIndex = ++_nextZIndex;
                Notify();
            }
        }

        private void Notify()
        {
            OnChange?.Invoke();
        }

        // 🔧 Internal helper (no need to call directly anymore)
        private RenderFragment CreateComponent<T>(Dictionary<string, object>? parameters = null)
            where T : IComponent
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(T));

                if (parameters != null)
                {
                    var i = 1;
                    foreach (var param in parameters)
                    {
                        builder.AddAttribute(i++, param.Key, param.Value);
                    }
                }

                builder.CloseComponent();
            };
        }
    }
}
