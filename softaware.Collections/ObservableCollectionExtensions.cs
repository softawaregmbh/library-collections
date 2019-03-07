using System;
using System.Collections.ObjectModel;

namespace softaware.Collections
{
    /// <summary>
    /// Provides extension methods for ObservableCollection&lt;T&gt;.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Registers an event handler for the CollectionChangedEvent of an ObservableCollection 
        /// and calls the specified delegates, when the collection changes.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="collection">The ObservableCollection.</param>
        /// <param name="removed">An action that is called when an element is removed from the collection.</param>
        /// <param name="added">An action that is called when an element is added to the collection.</param>
        /// <param name="before">An action that is called once per change before <paramref name="removed"/> or <paramref name="added"/> are called for every element.</param>
        /// <param name="after">An action that is called once per change after <paramref name="removed"/> or <paramref name="added"/> were called for every element.</param>
        /// <returns>An IDisposable, whose Dispose method deregisters the event handler.</returns>
        public static IDisposable HandleChanges<T>(this ObservableCollection<T> collection, Action<T> removed = null, Action<T> added = null, Action before = null, Action after = null)
        {
            return new CollectionChangedHandler<T>(collection, removed, added, before, after);
        }

        private class CollectionChangedHandler<T> : IDisposable
        {
            private ObservableCollection<T> collection;
            private Action<T> removedItems;
            private Action<T> addedItems;
            private Action before;
            private Action after;

            public CollectionChangedHandler(ObservableCollection<T> collection, Action<T> removedItems, Action<T> addedItems, Action before, Action after)
            {
                this.collection = collection;
                this.removedItems = removedItems;
                this.addedItems = addedItems;
                this.before = before;
                this.after = after;

                collection.CollectionChanged += this.OnCollectionChanged;
            }

            public void Dispose()
            {
                this.collection.CollectionChanged -= this.OnCollectionChanged;
            }

            private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                this.before?.Invoke();

                if (e.OldItems != null)
                {
                    foreach (T item in e.OldItems)
                    {
                        this.removedItems?.Invoke(item);
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (T item in e.NewItems)
                    {
                        this.addedItems?.Invoke(item);
                    }
                }

                this.after?.Invoke();
            }
        }
    }
}
