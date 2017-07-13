using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ListListDev.DAL;
using ListListDev.Models;

namespace ListListDev.Logic
{
    public class ListLogic : IDisposable
    {

        // Create connection to the list database
        private ListListContext listDb;

        // Create connection to the user database
        private ApplicationDbContext userDb;


        // Initialize database connections
        public ListLogic()
        {
            userDb = new ApplicationDbContext();
            listDb = new ListListContext();
        }

        #region ListHeader Actions

        // Get lists for a user
        // Returns a list of ListHeaders if successful
        // Returns null if there's an exception
        public List<ListHeader> GetDashboardLists(string userID)
        {
            try
            {
                /* Finds the lists that belong to the user by
                 * using the user's ID and the UserID property
                 * of the ListHeader. Also gets the information
                 * for the ListItems that belong to the list by 
                 * using the navigation property for ListHeader */
                return listDb.ListHeaders
                                .Include("ListItems")
                                .Where(x => x.UserID == userID)
                                .ToList();
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return null;
            }

        }

        // Create a new list
        // Returns true if the list is created successfully
        // Returns false if there are any issues saving it to the database
        public bool CreateListHeader(ListHeader listToSave)
        {
            try
            {
                // Add new list to the database
                listDb.ListHeaders.Add(listToSave);
                listDb.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
        }

        // Get a list from the database
        // Returns a ListHeader if retrieval is successful
        // Returns null if there's an exception
        public ListHeader GetListHeader(int listID, string userID)
        {
            try
            {
                // Find the list in the database
                ListHeader list = listDb.ListHeaders.FirstOrDefault(x => x.ID == listID);

                // Make sure list exists
                if (list != null)
                {
                    // Make sure the list belongs to this user
                    if (ListBelongsToUser(list.UserID, userID))
                    {
                        // The desired list is returned because it was found and the user has access
                        return list;
                    }
                    else
                    {
                        // User doesn't have access to this list
                        return null;
                    }
                }
                else
                {
                    // List wasn't found in the database
                    throw new DataNotFoundException(String.Format("A list with the ID: {0} could not be found.", listID));
                }
            }
            catch (DataNotFoundException ex)
            {
                // Log exception to database (not implemented yet)
                return null;
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return null;
            }
        }

        // Edit a list's title
        // Returns true if changes were saved successfully
        // Returns false if there's an exception
        public bool EditListHeader(ListHeader updatedList)
        {
            try
            {
                // Find the list that is being edited
                ListHeader listToEdit = listDb.ListHeaders.FirstOrDefault(x => x.ID == updatedList.ID);

                // Make sure a list was found
                if (listToEdit != null)
                {
                    // Make sure the list belongs to this user
                    if (ListBelongsToUser(listToEdit.UserID, updatedList.UserID))
                    {
                        // Save changes
                        listToEdit.Title = updatedList.Title;
                        listDb.SaveChanges();
                        return true;
                    }
                    else
                    {
                        // User doesn't have access to this list
                        return false;
                    }
                }
                else
                {
                    // List wasn't found in the database
                    throw new DataNotFoundException(String.Format("A list with the ID: {0} could not be found.", updatedList.ID));
                }
            }
            catch (DataNotFoundException ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
        }

        // Delete a list
        // Returns true if delete is successful
        // Returns false if there's an exception
        public bool DeleteListHeader(int listID, string userID)
        {
            try
            {
                // Find the list that is being deleted
                ListHeader listToBeDeleted = listDb.ListHeaders.FirstOrDefault(x => x.ID == listID);

                // Check that the list to be deleted actually exists
                if (listToBeDeleted != null)
                {
                    // Make sure the list belongs to this user
                    if (ListBelongsToUser(listToBeDeleted.UserID, userID))
                    {
                        // Delete all of the list items that belong to the list
                        List<ListItem> itemsToDelete = listDb.ListItems.Where(x => x.ListHeaderID == listID).ToList();
                        itemsToDelete.ForEach(x => listDb.ListItems.Remove(x));

                        //Delete list
                        listDb.ListHeaders.Remove(listToBeDeleted);
                        listDb.SaveChanges();
                        return true;
                    }
                    else
                    {
                        // User doesn't have access to this list
                        return false;
                    }
                }
                else
                {
                    // The list couldn't be found in the database
                    throw new DataNotFoundException(String.Format("A list with the ID: {0} could not be found.", listID));
                }
            }
            catch (DataNotFoundException ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
        }

        #endregion

        #region ListItem Actions

        // Creates a list item
        // Returns true if the list item was added to the database
        // Returns false if there's an exception
        public bool CreateListItem(ListItem listItem, string userID)
        {
            try
            {
                // Verify that the user has permission to add items to the list
                if(ListItemBelongsToUser(listItem.ListHeaderID, userID))
                {
                    // Add list item to the database
                    listDb.ListItems.Add(listItem);
                    listDb.SaveChanges();
                    return true;
                }
                else
                {
                    // User doesn't have access to this list
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
        }

        // Get a list item from the database
        // Returns the ListItem if it's found and the user has access to it
        // Returns null if there's an exception
        public ListItem GetListItem(int listItemID, string userID)
        {
            try
            {
                // Get the list item from the database
                ListItem listItem = listDb.ListItems.FirstOrDefault(x => x.ID == listItemID);

                // Check that list item exists
                if (listItem != null)
                {
                    // Verify that the user owns this item
                    if (ListItemBelongsToUser(listItem.ListHeaderID, userID))
                    {
                        // The desired list item is returned because it was found and the user has access
                        return listItem;
                    }
                    else
                    {
                        // User doesn't have access to the list this item belongs to
                        return null;
                    }
                }
                else
                {
                    // The list item couldn't be found in the database
                    throw new DataNotFoundException(String.Format("A list item with the ID: {0} could not be found.", listItemID));
                }
            }
            catch (DataNotFoundException ex)
            {
                // Log exception to database (not implemented yet)
                return null;
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return null;
            }
        }

        // Edit the text of a list item
        // Returns true if the changes are saved successfully
        // Returns false if there's an exception
        public bool EditListItem(ListItem updatedItem, string userID)
        {
            try
            {
                // Get the list item from the database
                ListItem editToSave = listDb.ListItems.FirstOrDefault(x => x.ID == updatedItem.ID);

                // Check that this list item exists
                if (editToSave != null)
                {
                    // Verify the user has permission to edit this item
                    if (ListItemBelongsToUser(editToSave.ListHeaderID, userID))
                    {
                        // Save changes
                        editToSave.Text = updatedItem.Text;
                        listDb.SaveChanges();
                        return true;
                    }
                    else
                    {
                        // User doesn't have access to this item
                        return false;
                    }
                }
                else
                {
                    // The list item couldn't be found in the database
                    throw new DataNotFoundException(String.Format("A list item with the ID: {0} could not be found.", updatedItem.ID));
                }
            }
            catch (DataNotFoundException ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
        }

        // Delete a list item
        // Returns true if the item is successfully deleted
        // Returns false if there's an exception
        public bool DeleteListItem(int listItemID, string userID)
        {
            try
            {
                // Get the list item from the database
                ListItem toBeDeleted = listDb.ListItems.FirstOrDefault(x => x.ID == listItemID);

                // Check that list item exists
                if (toBeDeleted != null)
                {
                    // Verify the user has permission to delete this item
                    if(ListItemBelongsToUser(toBeDeleted.ListHeaderID, userID))
                    {
                        // Delete item from database
                        listDb.ListItems.Remove(toBeDeleted);
                        listDb.SaveChanges();
                        return true;
                    }
                    else
                    {
                        // User doesn't have access for this item
                        return false;
                    }
                }
                else
                {
                    // The list item couldn't be found in the database
                    throw new DataNotFoundException(String.Format("A list item with the ID: {0} could not be found.", listItemID));
                }
            }
            catch (DataNotFoundException ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
        }

        #endregion

        #region Helper Methods

        // Checks if a user has ownership of a list
        // Returns true if the user owns the list
        // Returns false if the user doesn't own the list
        public bool ListBelongsToUser(string listOwnerID, string userID)
        {
            if (listOwnerID.Equals(userID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Checks if a user has ownership of a list item
        // Returns true if the user has access to the list item
        // Returns false if the user doesn't have access to the list item
        public bool ListItemBelongsToUser(int listID, string userID)
        {
            try
            {
                // Get list from database
                ListHeader list = listDb.ListHeaders.FirstOrDefault(x => x.ID == listID);

                // Make sure the list exists
                if (list != null)
                {
                    // Return the result of ListBelongsToUser
                    return ListBelongsToUser(list.UserID, userID);
                }
                else
                {
                    // The list wasn't found in the database
                    throw new DataNotFoundException(String.Format("A list with the ID: {0} could not be found.", listID));
                }
            }
            catch (DataNotFoundException ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
            catch (Exception ex)
            {
                // Log exception to database (not implemented yet)
                return false;
            }
        }

        #endregion

        // Clean up database connections
        public void Dispose()
        {
            listDb.Dispose();
            userDb.Dispose();
        }
    }

    [Serializable]
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException()
            : base() { }

        public DataNotFoundException(string message)
            : base(message) { }

        public DataNotFoundException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public DataNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public DataNotFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected DataNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}