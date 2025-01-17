﻿using Avalonia.Collections;
using Avalonia.Diagnostics;

namespace Avalonia.Controls.TreeDataGridTests
{
    internal static class CollectionExtensions
    {
        public static int CollectionChangedSubscriberCount<T>(this AvaloniaList<T> list)
        {
            return ((INotifyCollectionChangedDebug)list).GetCollectionChangedSubscribers()?.Length ?? 0;
        }
    }
}
