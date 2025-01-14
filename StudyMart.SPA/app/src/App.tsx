import { useEffect, useState } from 'react';
import { useAuth } from 'react-oidc-context';
import { useAddItem, useDeleteItem, useFetchItems, useUpdateItem } from './hooks/useItems';
import { useItemsStore } from './features/items/itemsStore';
import { Button } from './components/ui/button';
import {
  Table,
  TableBody,
  TableCell,
  TableFooter,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { Input } from './components/ui/input';
import Navbar from './components/ui/header';

function App() {
  const { data: queryItems } = useFetchItems();
  const addItemMutation = useAddItem();
  const updateItemMutation = useUpdateItem();
  const deleteItemMutation = useDeleteItem();
  const fetchItems = useItemsStore((state) => state.fetchItems);
  const { items, loading, error } = useItemsStore();

  const [editingItem, setEditingItem] = useState<{ id: number; title: string } | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const auth = useAuth();

  useEffect(() => {
    if (queryItems) {
      setIsLoading(false);
      fetchItems(queryItems);
    }
  }, [queryItems]);

  const handleAddItem = () => {
    setIsLoading(true);
    const newItem = { title: 'New Item' };
    addItemMutation.mutate(newItem, {
      // onSettled is called regardless of whether the query or mutation was successful or resulted in an error. 
      // It is always called after the request has completed.
      onSettled: () => {
        setIsLoading(false);
      },
    });
  };

  const handleUpdateItem = (id: number, title: string) => {
    setIsLoading(true);
    updateItemMutation.mutate({ id, title }, {
      onSettled: () => {
        setIsLoading(false);
        setEditingItem(null);
      },
    });
  };

  const handleDeleteItem = (id: number) => {
    setIsLoading(true);

    // Optimistically update the UI
    const previousItems = items;

    deleteItemMutation.mutate(id, {
      onSettled: () => {
        setIsLoading(false);
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

  return (
    <div className='container mx-auto'>
      <Navbar />
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead className="w-[100px]">Id</TableHead>
            <TableHead>Title</TableHead>
            <TableHead>Action</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {items.map((item) => (
            <TableRow key={item.id}>
              <TableCell className="font-medium">{item.id}</TableCell>
              {editingItem && editingItem.id === item.id ? (
                <>
                  <TableCell>
                    <Input type="text" placeholder="Item title" value={editingItem.title}
                      onChange={(e) => setEditingItem({ ...editingItem, title: e.target.value })}
                    />
                  </TableCell>
                  <TableCell>
                    <Button onClick={() => handleUpdateItem(item.id, editingItem.title)} disabled={isLoading}>
                      Save
                    </Button>
                    <Button onClick={() => setEditingItem(null)} disabled={isLoading}>
                      Cancel
                    </Button>
                  </TableCell>
                </>
              ) : (
                <>
                  <TableCell>{item.title}</TableCell>
                  <TableCell>
                    <Button onClick={() => setEditingItem(item)} disabled={isLoading}>
                      Edit
                    </Button>
                    <Button onClick={() => handleDeleteItem(item.id)} disabled={isLoading}>
                      Delete
                    </Button>
                  </TableCell>
                </>
              )}
            </TableRow>
          ))}
        </TableBody>
        <TableFooter>
          <TableRow>
            <TableCell>
              <Button onClick={handleAddItem} disabled={isLoading}>Add Item</Button>
            </TableCell>
          </TableRow>
        </TableFooter>
      </Table>
    </div>
  )
}

export default App;
