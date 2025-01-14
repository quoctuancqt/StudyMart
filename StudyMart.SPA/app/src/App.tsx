
import { useEffect, useState } from 'react';
import { useAuth } from 'react-oidc-context';
import { useAddItem, useDeleteItem, useFetchItems, useUpdateItem } from './hooks/useItems';
import { useItemsStore } from './features/items/itemsStore';

function App() {
  const { data: queryItems } = useFetchItems();
  const addItemMutation = useAddItem();
  const updateItemMutation = useUpdateItem();
  const deleteItemMutation = useDeleteItem();
  const fetchItems = useItemsStore((state) => state.fetchItems);
  const { items, loading, error } = useItemsStore();

  const [editingItem, setEditingItem] = useState<{ id: number; title: string } | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [loadingButton, setLoadingButton] = useState<string | null>(null);

  const auth = useAuth();

  useEffect(() => {
    if (queryItems) {
      setIsLoading(false);
      fetchItems(queryItems);
    }
  }, [queryItems]);

  const handleAddItem = () => {
    setIsLoading(true);
    setLoadingButton('add');
    const newItem = { title: 'New Item' };
    addItemMutation.mutate(newItem, {
      // onSettled is called regardless of whether the query or mutation was successful or resulted in an error. 
      // It is always called after the request has completed.
      onSettled: () => {
        setIsLoading(false);
        setLoadingButton(null);
      },
    });
  };

  const handleUpdateItem = (id: number, title: string) => {
    setIsLoading(true);
    setLoadingButton(`update-${id}`);
    updateItemMutation.mutate({ id, title }, {
      onSettled: () => {
        setIsLoading(false);
        setLoadingButton(null);
        setEditingItem(null);
      },
    });
  };

  const handleDeleteItem = (id: number) => {
    setIsLoading(true);
    setLoadingButton(`delete-${id}`);

    // Optimistically update the UI
    const previousItems = items;

    deleteItemMutation.mutate(id, {
      onSettled: () => {
        setIsLoading(false);
        setLoadingButton(null);
      },
      onError: () => {
        // Revert the change if the mutation fails
        fetchItems(previousItems);
      },
    });
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  switch (auth.activeNavigator) {
    case "signinSilent":
      return <div>Signing you in...</div>;
    case "signoutRedirect":
      return <div>Signing you out...</div>;
  }

  if (auth.isLoading) {
    return <div>Loading...</div>;
  }

  if (auth.error) {
    return <div>Oops... {auth.error.message}</div>;
  }

  if (auth.isAuthenticated) {
    return (
      <div className='container mx-auto px-4'>
        <h1 className="text-3xl font-bold text-red-500 text-center">Items</h1>
        <ul>
          {(items).map((item) => (
            <li key={item.id}>
              {editingItem && editingItem.id === item.id ? (
                <>
                  <input
                    type="text"
                    value={editingItem.title}
                    onChange={(e) => setEditingItem({ ...editingItem, title: e.target.value })}
                  />
                  <div>
                    <button onClick={() => handleUpdateItem(item.id, editingItem.title)} disabled={isLoading}>
                      Save
                      {loadingButton === `update-${item.id}` && <div></div>}
                    </button>
                    <button onClick={() => setEditingItem(null)} disabled={isLoading}>
                      Cancel
                    </button>
                  </div>
                </>
              ) : (
                <>
                  {item.title}
                  <div>
                    <button onClick={() => setEditingItem(item)} disabled={isLoading}>
                      Edit
                    </button>
                    <button onClick={() => handleDeleteItem(item.id)} disabled={isLoading}>
                      Delete
                      {loadingButton === `delete-${item.id}` && <div></div>}
                    </button>
                  </div>
                </>
              )}
            </li>
          ))}
        </ul>
        <button onClick={handleAddItem} disabled={isLoading}>
          Add Item
          {loadingButton === 'add' && <div></div>}
        </button>
        <button onClick={() => void auth.signoutRedirect()} disabled={isLoading}>
         Logout
        </button>
      </div>
    )
  }

  return (
    <div className='container mx-auto px-4'>
      <button className='block w-full rounded-md bg-indigo-600 px-3.5 py-2.5 text-center text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600' onClick={() => void auth.signinRedirect()}>Log in</button>
    </div>
  )
}

export default App;
