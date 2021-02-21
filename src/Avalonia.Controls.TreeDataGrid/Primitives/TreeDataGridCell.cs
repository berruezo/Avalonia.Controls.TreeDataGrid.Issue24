﻿using System;
using System.ComponentModel;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Controls.Primitives
{
    [PseudoClasses(":selected", ":editing")]
    public abstract class TreeDataGridCell : TemplatedControl, ITreeDataGridCell
    {
        public static readonly DirectProperty<TreeDataGridCell, bool> IsSelectedProperty =
            AvaloniaProperty.RegisterDirect<TreeDataGridCell, bool>(
                nameof(IsSelected),
                o => o.IsSelected,
                (o, v) => o.IsSelected = v);

        private bool _isEditing;
        private bool _isSelected;

        static TreeDataGridCell()
        {
            FocusableProperty.OverrideDefaultValue<TreeDataGridCell>(true);
        }

        public int ColumnIndex { get; private set; } = -1;
        public int RowIndex { get; private set; } = -1;

        int ITreeDataGridCell.ColumnIndex
        {
            get => ColumnIndex;
            set => ColumnIndex = value;
        }

        int ITreeDataGridCell.RowIndex
        {
            get => RowIndex;
            set => RowIndex = value;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetAndRaise(IsSelectedProperty, ref _isSelected, value);
        }

        protected virtual bool CanEdit => false;

        protected void BeginEdit()
        {
            if (!_isEditing)
            {
                _isEditing = true;
                (DataContext as IEditableObject)?.BeginEdit();
                PseudoClasses.Add(":editing");
            }
        }

        protected void CancelEdit()
        {
            if (EndEditCore())
                (DataContext as IEditableObject)?.CancelEdit();
        }

        protected void EndEdit()
        {
            if (EndEditCore())
                (DataContext as IEditableObject)?.EndEdit();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!_isEditing && CanEdit && !e.Handled && e.Key == Key.F2)
            {
                BeginEdit();
                e.Handled = true;
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (!_isEditing && CanEdit && !e.Handled && IsSelected)
            {
                BeginEdit();
                e.Handled = true;
            }
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            if (change.Property == IsSelectedProperty)
            {
                PseudoClasses.Set(":selected", change.NewValue.GetValueOrDefault<bool>());
            }
        }

        private bool EndEditCore()
        {
            if (_isEditing)
            {
                var restoreFocus = IsKeyboardFocusWithin;
                _isEditing = false;
                PseudoClasses.Remove(":editing");
                if (restoreFocus)
                    Focus();
                return true;
            }

            return false;
        }
    }
}
