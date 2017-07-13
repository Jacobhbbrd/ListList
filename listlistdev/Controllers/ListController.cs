using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ListListDev.DAL;
using ListListDev.Models;
using ListListDev.ViewModels;
using ListListDev.Logic;

namespace ListListDev.Controllers
{
    // Require a user to be logged in before being able to view/edit lists
    [Authorize]
    public class ListController : Controller
    {
        // Access to the logic for ListList
        private ListLogic _logic = new ListLogic();

        // Show error page (default message is "Unknown Error")
        public ActionResult Error(string message = "Unknown Error")
        {
            // Create view model with the provided error message
            var viewModel = new ErrorViewModel(message);
            return View(viewModel);
        }

        // Default page is the dashboard
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        // Show all lists that belong to the user
        public ActionResult Dashboard()
        {
            // Get lists that belong to user
            List<ListHeader> lists = _logic.GetDashboardLists(User.Identity.GetUserId());

            if (lists != null)
            {
                var viewModel = new DashboardViewModel(lists);
                return View(viewModel);
            }
            else
            {
                return RedirectToAction(nameof(Error),
                                            new { message = "There was an error accessing your lists." });
            }
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
                // Create the new list and save to the database
                ListHeader list = new ListHeader(model, User.Identity.GetUserId());

                // Try to save the new list to the database
                if (_logic.CreateListHeader(list))
                {
                    return RedirectToAction(nameof(Dashboard));
                }
                else
                {
                    return RedirectToAction(nameof(Error),
                                                new { message = "There was an error creating your list." });
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
                // Get the list from the database
                ListHeader list = _logic.GetListHeader(id.Value, User.Identity.GetUserId());

                if (list != null)
                {
                    var viewModel = new EditListHeaderViewModel(list);
                    return PartialView("~/Views/List/_EditListHeaderForm.cshtml", viewModel);
                }
            }

            return PartialView("~/Views/List/_Error.cshtml");
        }

        [HttpPost]
        public ActionResult EditListHeader(EditListHeaderViewModel model)
        {
            // Check that the form submitted is valid
            if (ModelState.IsValid)
            {
                ListHeader list = new ListHeader(model, User.Identity.GetUserId());

                // Try to save changes to the list
                if (_logic.EditListHeader(list))
                {
                    return RedirectToAction(nameof(Dashboard));
                }
                else
                {
                    return RedirectToAction(nameof(Error),
                                                new { message = "There was an error saving your changes for the list name." });
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
                // Try to delete the list from the database
                if (_logic.DeleteListHeader(id.Value, User.Identity.GetUserId()))
                {
                    return RedirectToAction(nameof(Dashboard));
                }
            }

            return RedirectToAction(nameof(Error),
                                        new { message = "There was an error deleting the list." });
        }
        #endregion

        #region ListItems Actions

        [HttpGet]
        public ActionResult CreateListItemPartial(int? id)
        {
            // Make sure an id was passed in
            if (id != null)
            {
                // Make sure the user is allowed to add items to the list
                if (_logic.ListItemBelongsToUser(id.Value, User.Identity.GetUserId()))
                {
                    var viewModel = new CreateListItemViewModel(id.Value);
                    return PartialView("~/Views/List/_CreateListItemForm.cshtml", viewModel);
                }
            }

            return PartialView("~/Views/List/_Error.cshtml");
        }

        [HttpPost]
        public ActionResult CreateListItem(CreateListItemViewModel model)
        {
            // Check that the form submitted is valid
            if (ModelState.IsValid)
            {
                // Create a ListItem using the model received
                ListItem listItem = new ListItem(model);

                // Try to add list item to the database
                if (_logic.CreateListItem(listItem, User.Identity.GetUserId()))
                {
                    return RedirectToAction(nameof(Dashboard));
                }
                else
                {
                    return RedirectToAction(nameof(Error),
                                                new { message = "There was an error creating the list item." });
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
                ListItem itemToEdit = _logic.GetListItem(id.Value, User.Identity.GetUserId());

                // Check that list item exists
                if (itemToEdit != null)
                {
                    var viewModel = new EditListItemViewModel(itemToEdit);
                    return PartialView("~/Views/List/_EditListItemForm.cshtml", viewModel);
                }
            }

            return PartialView("~/Views/List/_Error.cshtml");
        }

        [HttpPost]
        public ActionResult EditListItem(EditListItemViewModel model)
        {
            // Check that the form submitted is valid
            if (ModelState.IsValid)
            {
                // Create new ListItem using the model received
                ListItem item = new ListItem(model);

                // Try to save changes to the dastabase
                if (_logic.EditListItem(item, User.Identity.GetUserId()))
                {
                    return RedirectToAction(nameof(Dashboard));
                }
                else
                {
                    return RedirectToAction(nameof(Error),
                                                new { message = "There was an error saving your changes for the list item text." });
                }
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
                // Try to delete item from the database
                if (_logic.DeleteListItem(id.Value, User.Identity.GetUserId()))
                {
                    return RedirectToAction(nameof(Dashboard));
                }
            }

            return RedirectToAction(nameof(Error),
                                    new { message = "There was an error deleting the list item." });
        }

        #endregion
    }
}