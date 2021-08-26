﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Avalonia.Controls.Selection
{
    public interface ITreeSelectionModel : INotifyPropertyChanged
    {
        IEnumerable? Source { get; set; }
        bool SingleSelect { get; set; }
        IndexPath SelectedIndex { get; set; }
        IReadOnlyList<IndexPath> SelectedIndexes { get; }
        object? SelectedItem { get; set; }
        IReadOnlyList<object?> SelectedItems { get; }
        IndexPath AnchorIndex { get; set; }
        int Count { get; }

        event EventHandler<TreeSelectionModelSelectionChangedEventArgs>? SelectionChanged;
        event EventHandler<SelectionModelIndexesChangedEventArgs>? IndexesChanged;
        event EventHandler? LostSelection;

        void BeginBatchUpdate();
        void Clear();
        void Deselect(IndexPath index);
        void DeselectRange(IndexPath start, IndexPath end);
        void EndBatchUpdate();
        bool IsSelected(IndexPath index);
        void Select(IndexPath index);
        void SelectAll();
        void SelectRange(IndexPath start, IndexPath end);
    }
}
