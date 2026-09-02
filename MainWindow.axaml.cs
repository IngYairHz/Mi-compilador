using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;

namespace AvaloniaApplication3
{
    public partial class MainWindow : Window
    {
        private string? _currentFilePath;

        public MainWindow()
        {
            InitializeComponent();

            this.FindControl<MenuItem>("MenuNuevo")!.Click += OnNuevoClick;
            this.FindControl<MenuItem>("MenuAbrir")!.Click += OnAbrirClick;
            this.FindControl<MenuItem>("MenuGuardar")!.Click += OnGuardarClick;
            this.FindControl<MenuItem>("MenuCompilar")!.Click += OnCompilarClick;
            this.FindControl<MenuItem>("MenuSalir")!.Click += (s, e) => Close();
        }

        private void OnNuevoClick(object? sender, RoutedEventArgs e)
        {
            var textBox = this.FindControl<TextBox>("MainTextBox");
            if (textBox != null)
            {
                textBox.Text = string.Empty;
                _currentFilePath = null;
            }
        }

        private async void OnAbrirClick(object? sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
            {
                Title = "Abrir archivo de texto",
                AllowMultiple = false
            });

            if (files.Count > 0)
            {
                _currentFilePath = files[0].Path.LocalPath;
                var textBox = this.FindControl<TextBox>("MainTextBox");
                if (textBox != null)
                {
                    textBox.Text = await File.ReadAllTextAsync(_currentFilePath);
                }
            }
        }

        private async void OnGuardarClick(object? sender, RoutedEventArgs e)
        {
            var textBox = this.FindControl<TextBox>("MainTextBox");
            if (textBox == null) return;

            if (string.IsNullOrEmpty(_currentFilePath))
            {
                var file = await StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
                {
                    Title = "Guardar archivo de texto"
                });

                if (file != null)
                {
                    _currentFilePath = file.Path.LocalPath;
                }
            }

            if (!string.IsNullOrEmpty(_currentFilePath))
            {
                await File.WriteAllTextAsync(_currentFilePath, textBox.Text ?? string.Empty);
            }
        }

        private void OnCompilarClick(object? sender, RoutedEventArgs e)
        {
            var textBox = this.FindControl<TextBox>("MainTextBox");
            if (textBox != null)
            {
                // Aquí puedes agregar la lógica que desees al hacer clic en compilar
                textBox.Text += "\n\n// [Sistema]: ¡Texto compilado con éxito!";
            }
        }
    }
}
