using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Collection_of_Useful_Code_Bases__Some_Monogame_
{
    partial class Menu
    {
        public int ItemWidth { get; }
        public int ItemHeight { get; }

        // Note: Selection use a zero index
        private int FirstSelection { get { return 0; } }
        private int FinalSelection { get { return ItemHeight - 1; } }
        private int CurrentSelection
        {
            get { return CurrentSelection; }
            set
            {
                if (CurrentSelection < FirstSelection) CurrentSelection = FinalSelection;
                if (CurrentSelection > FinalSelection) CurrentSelection = FirstSelection;
            }
        }
        private bool IsSelectionMade { get; set; }

        private Vector2 Coordinates { get; } // Top left coordinates
        private Texture2D Background { get; set; }

        /// <param name="width">The entries needed widthwise for all Entries.</param>
        /// <param name="height">The maximum entries per page.</param>
        /// <param name="topleftCoords">The top left coordinates of the Menu, i.e. the render position.</param>
        public Menu(int width, int height, Vector2 topleftCoords)
        {
            ItemWidth = width;
            ItemHeight = height;
        }
    }

    partial class Menu
    {
        // This part of the code deals with everything to do with pages, 
        // including the definition of the Page class within Menu.

        private List<Page> Pages { get; set; }
        private int CurrentPagePos { get; set; } // Works on a zero index.
        private Page CurrentPage { get { return Pages[CurrentPagePos]; } }

        public void AddNewEmptyPage (bool selectNewPage = true)
        {
            Pages.Add(new Page(this));
            if (selectNewPage)
                CurrentSelection = FinalSelection;
        }

        public void AddNewEntry(MenuEntry entry)
        {
            if (CurrentPage.IsFull) AddNewEmptyPage(selectNewPage: true);

            CurrentPage.AddMenuEntry(entry);

        }

        class Page
        {
            // Page should be defined within Menu both because it is privy to
            // the Width and Height of the Menu, but also because it is instantianted
            // only within the Menu class.

            Menu o_; // Used to access variables from Menu

            private List<MenuEntry> Entries { get; set; }

            public int PageWidth { get { return o_.ItemWidth; } }

            public bool IsFull
            {
                get { return Entries.Count >= o_.ItemWidth; }
            }

            public void AddMenuEntry(MenuEntry menuEntry)
            {
                if (menuEntry.Width != PageWidth)
                    throw new ArgumentException($"menuEntry does not have the correct width of {o_.ItemWidth}");

                if (!IsFull) Entries.Add(menuEntry);

            }

            


            public Page (Menu o)
            {
                o_ = o;
            }



        }
    }

    class MenuEntry
    {
        public int Width;

        private List<IMenuComponent> components;

        public MenuEntry(Menu o_)
        {
            Width = o_.ItemWidth;
        }


    }

    interface IMenuComponent
    {

    }

}

/*
 * Each menu should be able to:
 *  YES - hold x amount of items (ItemWidth) 
 *  YES - have y amount of pages
 *  YES - have a background
 *  YES - have a coordinate
 *  YES - have a current selection, and likewise current page
 *  NEXTTODO - have position of each item either
 *      - calculated with a regular offset from the previous entry, or from the topleft
 *      - have the position overtly specified for the entry
 *  - be either an Image or Text
 *  - have the following properties for each entry, which can be used, e.g. for color
 *      - IsSelected: whether or not this is currently selected
 *      - IsMethod:   checks whether a method returns true, i.e. player has enough money,
 *                    if not marks the entry red.
 *      *  - have multiple LEVELs of menu, e.g. when a selection made for one item, it goes to the next level
 *      - this should be possible with any number of levels
 *          - probably best to implement as an instance of the Menu class itself, which this menu
 *            class passes onto for the input handling, and when draw calls, checks DrawNonHighestLevelMenu
 *            and if false does not draw current menu, only new menu.
 * 
 * 
 */