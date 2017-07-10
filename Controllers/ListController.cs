using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ListListDev.DAL;
using ListListDev.Models;

namespace ListListDev.Controllers
{
    // Require a user to be logged in before being able to view/edit lists
    [Authorize]
    public class ListController : Controller
    {
        #region Database Connections

        // Create connection to list database
        private ListListContext listDb = new ListListContext();

        // Create connection to user database
        private ApplicationDbContext userDb = new ApplicationDbContext();

        #endregion

        public ActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        // Show all lists that belong to the user
        public ActionResult Dashboard()
        {
            var user = User.Identity.GetUserId();
            List<ListHeader> lists = listDb.ListHeaders
                                            .Include("ListItems")
                                            .Where(x => x.UserID == user)
                                            .ToList();
            var viewModel = new DashboardViewModel(lists);
            return View(viewModel);
        }

        // Show error page (default message is "Unknown Error")
        public ActionResult Error(string message = "Unknown Error")
        {
            // Create view model with the provided error message
            var viewModel = new ErrorViewModel(message);
            return View(viewModel);
        }

        // Show error partial (default message is "Unknown Error")
        public ActionResult ErrorPartial(string message = "Unknown Error")
        {
            // Create view model with the provided error message
            var viewModel = new ErrorViewModel(message);
            return PartialView("~/Views/List/_Error.cshtml", viewModel);
        }

        #region ListHeader Actions

        [HttpGet]
        public ActionResult CreateListHeaderPartial()
        {
            var viewModel = new CreateListHeaderViewModel();
            return PartialView("~/Views/List/_CreateListHeaderForm.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult CreateListHeader(CreateListHeaderViewModel model)
        {
            // Check that the form submitted is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Create the new list and save to the database
                    ListHeader list = new ListHeader(model, User.Identity.GetUserId());
                    listDb.ListHeaders.Add(list);
                    listDb.SaveChanges();

                    return RedirectToAction(nameof(Dashboard));
                }
                catch (Exception ex)
                {
                    // There was an error saving changes to the database
                    return RedirectToAction(nameof(ErrorPartial), 
                                            new { message = String.Format("There was an error saving your changes. Please try again.\nError message: {0}", 
                                            ex.Message) });
                }
            }
            else
            {
                // Form data wasn't valid so return user to the form
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult EditListHeaderPartial(int? id)
        {
            // Make sure an id was passed in
            if (id != null)
            {
                // Attempt to get list from database
                ListHeader list = listDb.ListHeaders.FirstOrDefault(x => x.ID == id);

                // Make sure list exists
                if (list != null)
                {
                    // Make sure the list belongs to this user
                    if (!list.UserID.Equals(User.Identity.GetUserId()))
                    {
                        return RedirectToAction(nameof(ErrorPartial),
                                                new { message = "There was an error accessing the list you were looking for." });
                    }

                    var viewModel = new EditListHeaderViewModel(list);
                    return PartialView("~/Views/List/_EditListHeaderForm.cshtml", viewModel);
                }
            }
            return RedirectToAction(nameof(Error), 
                                    new { message = "A list with the provided ID could not be found!" });
        }

        [HttpPost]
        public ActionResult EditListHeader(EditListHeaderViewModel model)
        {
            // Check that the form submitted is valid
            if (ModelState.IsValid)
            {
                // Find the list that is being edited
                ListHeader editToSave = listDb.ListHeaders.FirstOrDefault(x => x.ID == model.ID);

                // Make sure a list was found
                if (editToSave != null)
                {
                    // Make sure this list belongs to the current user
                    if (!editToSave.UserID.Equals(User.Identity.GetUserId()))
                    {
                        return RedirectToAction(nameof(ErrorPartial), 
                                                new { message = "There was an error accessing the list you were looking for." });
                    }

                    try
                    {
                        // Save changes
                        editToSave.Title = model.Title;
                        listDb.SaveChanges();
                        return RedirectToAction(nameof(Dashboard));
                    }
                    catch (Exception ex)
                    {
                        // There was an error saving changes to the database
                        return RedirectToAction(nameof(ErrorPartial),
                                                new { message = String.Format("There was an error saving your changes. Please try again.\nError message: {0}", 
                                                ex.Message) });
                    }
                }
                else
                {
                    return RedirectToAction(nameof(ErrorPartial), 
                                            new { message = "There was an error accessing the list you were looking for." });
                }
            }
            else
            {
                // Form data wasn't valid so return user to the form
                return View(model);
            }
        }

        public ActionResult DeleteListHeader(int? id)
        {
            // Make sure an id was passed in
            if (id != null)
            {
                // Find the list that is being deleted
                ListHeader toBeDeleted = listDb.ListHeaders.FirstOrDefault(x => x.ID == id);

                // Check that the list to be deleted actually exists
                if (toBeDeleted != null)
                {
                    // Make sure this list belongs to the current user
                    if (!toBeDeleted.UserID.Equals(User.Identity.GetUserId()))
                    {
                        return RedirectToAction(nameof(Error),
                                                new { message = "There was an error accessing the list you were looking for." });
                    }

                    try
                    {
                        // Delete all list items belonging to list
                        List<ListItem> itemsToDelete = listDb.ListItems.Where(x => x.ListHeaderID == id).ToList();
                        itemsToDelete.ForEach(x => listDb.ListItems.Remove(x));

                        //Delete list
                        listDb.ListHeaders.Remove(toBeDeleted);
                        listDb.SaveChanges();
                        return RedirectToAction(nameof(Dashboard));
                    }
                    catch (Exception ex)
                    {
                        // There was an error saving changes to the database
                        return RedirectToAction(nameof(Error),
                                                new { message = String.Format("There was an error saving your changes. Please try again.\nError message: {0}",
                                                ex.Message) });
                    }
                }
            }
            return RedirectToAction(nameof(Error), 
                                    new { message = "The list you're trying to delete could not be found. Please try again." });
        }
        #endregion

        #region ListItems Actions

        [HttpGet]
        public ActionResult CreateListItemPartial(int? id)
        {
            // Make sure an id was passed in
            if (id != null)
            {
                // Get list from database
                ListHeader list = listDb.ListHeaders.FirstOrDefault(x => x.ID == id.Value);

                // Make sure the list exists
                if (list != null)
                {
                    // Make sure the list belongs to this user
                    if (!list.UserID.Equals(User.Identity.GetUserId()))
                    {
                        return RedirectToAction(nameof(ErrorPartial), 
                                                new { message = "there was an error accessing the list you were looking for." });
                    }

                    var viewModel = new CreateListItemViewModel(id.Value);
                    return PartialView("~/Views/List/_CreateListItemForm.cshtml", viewModel);
                }
            }

            return RedirectToAction(nameof(ErrorPartial), 
                                    new { message = "There was an error accessing the list you were looking for." });
        }

        [HttpPost]
        public ActionResult CreateListItem(CreateListItemViewModel model)
        {
            // Check that the form submitted is valid
            if (ModelState.IsValid)
            {
                // Create a ListItem using the model received
                ListItem listItem = new ListItem(model);

                // Get the list that the list item is in
                ListHeader list = listDb.ListHeaders.FirstOrDefault(x => x.ID == listItem.ListHeaderID);

                // Make sure list exists
                if (list != null)
                {
                    // Make sure this list belongs to the current user
                    if (!list.UserID.Equals(User.Identity.GetUserId()))
                    {
                        return RedirectToAction(nameof(ErrorPartial),
                                                new { message = "There was an error accessing the list you were looking for." });
                    }

                    try
                    {
                        // Add list item to the database
                        listDb.ListItems.Add(listItem);
                        listDb.SaveChanges();
                        return RedirectToAction(nameof(Dashboard));
                    }
                    catch (Exception ex)
                    {
                        // There was an error saving changes to the database
                        return RedirectToAction(nameof(ErrorPartial), 
                                                new { message = String.Format("There was an error saving your changes. Please try again.\nError message: {0}",
                                                ex.Message) });
                    }
                }
                else
                {
                    return RedirectToAction(nameof(ErrorPartial), 
                                            new { message = "There was an error accessing the list you were looking for." });
                }
            }
            else
            {
                // Form data wasn't valid so return user to the form
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult EditListItemPartial(int? id)
        {
            // Make sure an id was passed in
            if (id != null)
            {
                // Get the list item from the database
                ListItem itemToEdit = listDb.ListItems.FirstOrDefault(x => x.ID == id);

                // Check that list item exists
                if (itemToEdit != null)
                {
                    // Get the list that the list item belongs to from the database
                    ListHeader list = listDb.ListHeaders.FirstOrDefault(x => x.ID == itemToEdit.ListHeaderID);

                    // Make sure the list exists
                    if (list != null)
                    {
                        // Make sure this list belongs to the current user
                        if (!list.UserID.Equals(User.Identity.GetUserId()))
                        {
                            return RedirectToAction(nameof(ErrorPartial), 
                                                    new { message = "There was an error accessing the list you were looking for." });
                        }

                        var viewModel = new EditListItemViewModel(itemToEdit);
                        return PartialView("~/Views/List/_EditListItemForm.cshtml", viewModel);
                    }
                }
            }

            return RedirectToAction(nameof(ErrorPartial), 
                                    new { message = "There was an error accessing the list item you were looking for." });
        }

        [HttpPost]
        public ActionResult EditListItem(EditListItemViewModel model)
        {
            // Check that the form submitted is valid
            if (ModelState.IsValid)
            {
                // Get the list item from the database
                ListItem editToSave = listDb.ListItems.FirstOrDefault(x => x.ID == model.ID);

                // Check that this list item exists
                if (editToSave != null)
                {
                    // Get the list that the list item belongs to from the database
                    ListHeader list = listDb.ListHeaders.FirstOrDefault(x => x.ID == editToSave.ListHeaderID);

                    // Make sure list exists
                    if (list != null)
                    {
                        // Make sure this list belongs to the current user
                        if (!list.UserID.Equals(User.Identity.GetUserId()))
                        {
                            return RedirectToAction(nameof(ErrorPartial), 
                                                    new { message = "There was an error accessing the list you were looking for." });
                        }

                        try
                        {
                            editToSave.Text = model.Text;
                            listDb.SaveChanges();
                            return RedirectToAction(nameof(Dashboard));
                        }
                        catch (Exception ex)
                        {
                            return RedirectToAction(nameof(ErrorPartial), 
                                                    new { message = String.Format("There was an error save your changes. Please try again.\nError message: {0}", 
                                                    ex.Message) });
                        }
                    }
                }

                // This is reached if the list item or list don't exist
                return RedirectToAction(nameof(ErrorPartial),
                                        new { message = "There was an error accessing the list you were looking for." });
            }
            else
            {
                // Form data wasn't valid so return user to the form
                return View(model);
            }
        }

        public ActionResult DeleteListItem(int? id)
        {
            // Make sure an id was passed in
            if (id != null)
            {
                // Get the list item from the database
                ListItem toBeDeleted = listDb.ListItems.FirstOrDefault(x => x.ID == id);

                // Check that list item exists
                if (toBeDeleted != null)
                {
                    // Get the list from the database
                    ListHeader list = listDb.ListHeaders.FirstOrDefault(x => x.ID == toBeDeleted.ListHeaderID);

                    // Make sure the list exists
                    if (list != null)
                    {
                        // Make sure this list belongs to the current user
                        if (!list.UserID.Equals(User.Identity.GetUserId()))
                        {
                            return RedirectToAction(nameof(Error),
                                                    new { message = "There was an error accessing the list you were looking for." });
                        }

                        try
                        {
                            // Delete item from database
                            listDb.ListItems.Remove(toBeDeleted);
                            listDb.SaveChanges();
                            return RedirectToAction(nameof(Dashboard));
                        }
                        catch (Exception ex)
                        {
                            // There was an error saving changes to the database
                            return RedirectToAction(nameof(Error), 
                                                    new { message = String.Format("There was an error save your changes. Please try again.\nError message: {0}",
                                                    ex.Message) });
                        }
                    }
                }
            }

            // This is reached if either the id passed in is null, the item doesn't exist, or the list doesn't exist
            return RedirectToAction(nameof(Error), 
                                    new { message = "There was an error accessing the list you were looking for." });
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            listDb.Dispose();
            base.Dispose(disposing);
        }
    }
}