﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProControlsDemo.Views.History.Columns
{
    public class IncomingColumnView : UserControl
    {
        public IncomingColumnView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
